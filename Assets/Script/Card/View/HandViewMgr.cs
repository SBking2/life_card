using DG.Tweening;
using QF;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Splines;

/// <summary>
/// HandView管理手牌的位置
/// </summary>
public class HandViewMgr : MonoSingleton<HandViewMgr>
{
    //需要挂载的组件
    [SerializeField] private SplineContainer m_Spline;
    [SerializeField] private Transform m_DiscardTrans;
    [SerializeField] private Transform m_DrawPipleTrans;

    //卡牌间距的限制
    private float m_MinCardSpace = 0.05f;
    private float m_MaxCardSpace = 0.15f;

    private float duration = 0.15f;
    private Vector3 m_FatherOffset;
    private List<CardView> m_HandCards = new List<CardView>();

    private bool m_IsDrawing = false;
    private bool m_IsSelecting = false;

    private CardView m_HorverdCardView;
    private CardView m_SelectCardView;

    [SerializeField] private LayerMask m_CardLayerMask;
    [SerializeField] private Collider m_RealeaseCardZone;   //卡牌的释放区域

    private bool m_IsCasting = false;
    private Queue<CardView> m_CasteQueue = new Queue<CardView>();
    private Queue<List<GameObject>> m_DrawCardQueue = new Queue<List<GameObject>>(); 

    /// <summary>
    /// 能够抽的卡数
    /// </summary>
    public int HandCardCount
    {
        get
        {
            return m_HandCards.Count;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        m_FatherOffset = m_Spline.transform.localPosition;

        //监听数据层
        CardMgr.Instance.onDrawCard += DrawCard;
        CardMgr.Instance.onDiscardCard += RemoveCard;
    }

    private void Update()
    {
        CardInteractHandle();
    }

    /// <summary>
    /// 处理卡牌的交互
    /// </summary>
    private void CardInteractHandle()
    {
        //检测手牌
        if (!m_IsSelecting)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, 100.0f, m_CardLayerMask);

            if (hits.Length > 0)
            {
                //根据能见排序
                RaycastHit topHit = hits.OrderByDescending(hit => hit.transform.GetComponent<SortingGroup>().sortingOrder).First();

                CardView cardView = topHit.collider.gameObject.GetComponent<CardView>();
                // 处理topHit的对象
                if (cardView.CurrentState == CardView.CardState.None)       //None状态的卡牌才能进入被Hover
                {
                    m_HorverdCardView?.EnterState(CardView.CardState.None, true);
                    m_HorverdCardView = cardView;
                    m_HorverdCardView.EnterState(CardView.CardState.Horvered);
                }
            }
            else if (m_HorverdCardView)
            {
                m_HorverdCardView.EnterState(CardView.CardState.None, true);
                m_HorverdCardView = null;
            }
        }

        //鼠标点击,选择卡牌逻辑
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!m_IsSelecting && m_HorverdCardView != null)
            {
                //进入Select状态
                m_IsSelecting = true;
                m_SelectCardView = m_HorverdCardView;
                m_SelectCardView.EnterState(CardView.CardState.Selected);
                m_HorverdCardView = null;
            }
            else if (m_IsSelecting)
            {
                //退出Select状态
                m_IsSelecting = false;
                m_SelectCardView.EnterState(CardView.CardState.None, true);
                m_SelectCardView = null;
            }
        }

        //保持卡牌与鼠标的同步
        if (m_IsSelecting)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 1.0f));
            pos.z = m_SelectCardView.transform.position.z;
            m_SelectCardView.transform.position = Vector3.Lerp(m_SelectCardView.transform.position, pos, 20.0f * Time.deltaTime);
        }

        //卡牌释放逻辑
        if (m_IsSelecting && Mouse.current.rightButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (m_RealeaseCardZone.Raycast(ray, out hit, 100.0f))
            {
                m_SelectCardView.EnterState(CardView.CardState.Caste);
                CasteCard(m_SelectCardView);
                m_IsSelecting = false;
                m_SelectCardView = null;
            }
        }
    }

    private void CasteCard(CardView cardView)
    {
        m_CasteQueue.Enqueue(cardView);
        if (m_IsCasting) return;
        else
            StartCoroutine(StartCasteQueue());
    }

    private IEnumerator StartCasteQueue()
    {
        m_IsCasting = true;
        while(m_CasteQueue.Count > 0)
        {
            CardView cardView = m_CasteQueue.Dequeue();
            GameObject data = cardView.cardObj;
            Tween twe = cardView.transform.DOMove(Vector3.zero, 1.0f).OnComplete(() =>
            {
                CardMgr.Instance.DiscardCard(data);       //动画播放结束时，从数据层移除手牌
            });

            yield return twe.WaitForCompletion();
        }
        m_IsCasting = false;
    }

    /// <summary>
    /// 一次抽多张卡，
    /// </summary>
    /// <param name="cardObjs"></param>
    private void DrawCard(List<GameObject> cardObjs)
    {
        m_DrawCardQueue.Enqueue(cardObjs);
        if (m_IsDrawing) return;
        StartCoroutine(StartDrawCard());
    }

    /// <summary>
    /// 抽卡动画
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartDrawCard()
    {
        m_IsDrawing = true;
        while(m_DrawCardQueue.Count > 0)
        {
            List<GameObject> cardObjs = m_DrawCardQueue.Dequeue();
            foreach (var obj in cardObjs)
            {
                AddCard(obj);
                yield return new WaitForSeconds(0.15f);
            }
        }
        m_IsDrawing = false;
    }

    private void AddCard(GameObject cardObj)
    {
        //激活GameObjec的View层
        CardView cardView = cardObj.GetComponentInChildren<CardView>(true);

        cardView.gameObject.SetActive(true);
        m_HandCards.Add(cardView);
        cardView.transform.position = m_DrawPipleTrans.position;
        cardView.transform.localScale = Vector3.zero;

        //为CardView添加动画
        cardView.transform.DOScale(1.0f, duration);

        UpdateCardPosition();
    }

    private void RemoveCard(GameObject cardObj)
    {
        if (m_HandCards.Count < 1) return;

        GameObject obj = null;
        CardView cardView = null;
        foreach(var view in m_HandCards)
        {
            if (view.cardObj == cardObj)
            {
                obj = view.gameObject;
                cardView = view;
            }
        }

        if(obj == null) return;


        m_HandCards.Remove(cardView);   //从手牌中移除这个cardview
        cardView.transform.DOMove(m_DiscardTrans.position, duration);
        cardView.transform.DOScale(0.0f, duration).OnComplete(() =>
        {
            obj.SetActive(false);
        });
        UpdateCardPosition();
    }
    
    /// <summary>
    /// 把手牌重新紧凑
    /// </summary>
    private void UpdateCardPosition()
    {
        if (m_HandCards.Count == 0) return;
        float cardSpacing = 1f / m_HandCards.Count;
        cardSpacing = Mathf.Clamp(cardSpacing, m_MinCardSpace, m_MaxCardSpace);     //限制卡牌间距
        float firstPos = 0.5f - (m_HandCards.Count - 1) * cardSpacing / 2;

        for(int i = 0; i < m_HandCards.Count; i++)
        {
            float cardT = firstPos + i * cardSpacing;
            Vector3 pos = m_Spline.Spline.EvaluatePosition(cardT);      //曲线的localPosition
            pos += m_FatherOffset;
            Vector3 up = m_Spline.Spline.EvaluateUpVector(cardT);       //曲线切线的垂线
            Vector3 tangent = m_Spline.Spline.EvaluateTangent(cardT);   //曲线切线
            Quaternion quat = Quaternion.LookRotation(Vector3.Cross(tangent, up), up);

            m_HandCards[i].SetTrans(pos, quat, i);
        }
    }
}
