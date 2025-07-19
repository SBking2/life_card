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
/// HandView�������Ƶ�λ��
/// </summary>
public class HandViewMgr : MonoSingleton<HandViewMgr>
{
    //��Ҫ���ص����
    [SerializeField] private SplineContainer m_Spline;
    [SerializeField] private Transform m_DiscardTrans;
    [SerializeField] private Transform m_DrawPipleTrans;

    //���Ƽ�������
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
    [SerializeField] private Collider m_RealeaseCardZone;   //���Ƶ��ͷ�����

    private bool m_IsCasting = false;
    private Queue<CardView> m_CasteQueue = new Queue<CardView>();
    private Queue<List<GameObject>> m_DrawCardQueue = new Queue<List<GameObject>>(); 

    /// <summary>
    /// �ܹ���Ŀ���
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

        //�������ݲ�
        CardMgr.Instance.onDrawCard += DrawCard;
        CardMgr.Instance.onDiscardCard += RemoveCard;
    }

    private void Update()
    {
        CardInteractHandle();
    }

    /// <summary>
    /// �����ƵĽ���
    /// </summary>
    private void CardInteractHandle()
    {
        //�������
        if (!m_IsSelecting)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, 100.0f, m_CardLayerMask);

            if (hits.Length > 0)
            {
                //�����ܼ�����
                RaycastHit topHit = hits.OrderByDescending(hit => hit.transform.GetComponent<SortingGroup>().sortingOrder).First();

                CardView cardView = topHit.collider.gameObject.GetComponent<CardView>();
                // ����topHit�Ķ���
                if (cardView.CurrentState == CardView.CardState.None)       //None״̬�Ŀ��Ʋ��ܽ��뱻Hover
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

        //�����,ѡ�����߼�
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!m_IsSelecting && m_HorverdCardView != null)
            {
                //����Select״̬
                m_IsSelecting = true;
                m_SelectCardView = m_HorverdCardView;
                m_SelectCardView.EnterState(CardView.CardState.Selected);
                m_HorverdCardView = null;
            }
            else if (m_IsSelecting)
            {
                //�˳�Select״̬
                m_IsSelecting = false;
                m_SelectCardView.EnterState(CardView.CardState.None, true);
                m_SelectCardView = null;
            }
        }

        //���ֿ���������ͬ��
        if (m_IsSelecting)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 1.0f));
            pos.z = m_SelectCardView.transform.position.z;
            m_SelectCardView.transform.position = Vector3.Lerp(m_SelectCardView.transform.position, pos, 20.0f * Time.deltaTime);
        }

        //�����ͷ��߼�
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
                CardMgr.Instance.DiscardCard(data);       //�������Ž���ʱ�������ݲ��Ƴ�����
            });

            yield return twe.WaitForCompletion();
        }
        m_IsCasting = false;
    }

    /// <summary>
    /// һ�γ���ſ���
    /// </summary>
    /// <param name="cardObjs"></param>
    private void DrawCard(List<GameObject> cardObjs)
    {
        m_DrawCardQueue.Enqueue(cardObjs);
        if (m_IsDrawing) return;
        StartCoroutine(StartDrawCard());
    }

    /// <summary>
    /// �鿨����
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
        //����GameObjec��View��
        CardView cardView = cardObj.GetComponentInChildren<CardView>(true);

        cardView.gameObject.SetActive(true);
        m_HandCards.Add(cardView);
        cardView.transform.position = m_DrawPipleTrans.position;
        cardView.transform.localScale = Vector3.zero;

        //ΪCardView��Ӷ���
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


        m_HandCards.Remove(cardView);   //���������Ƴ����cardview
        cardView.transform.DOMove(m_DiscardTrans.position, duration);
        cardView.transform.DOScale(0.0f, duration).OnComplete(() =>
        {
            obj.SetActive(false);
        });
        UpdateCardPosition();
    }
    
    /// <summary>
    /// ���������½���
    /// </summary>
    private void UpdateCardPosition()
    {
        if (m_HandCards.Count == 0) return;
        float cardSpacing = 1f / m_HandCards.Count;
        cardSpacing = Mathf.Clamp(cardSpacing, m_MinCardSpace, m_MaxCardSpace);     //���ƿ��Ƽ��
        float firstPos = 0.5f - (m_HandCards.Count - 1) * cardSpacing / 2;

        for(int i = 0; i < m_HandCards.Count; i++)
        {
            float cardT = firstPos + i * cardSpacing;
            Vector3 pos = m_Spline.Spline.EvaluatePosition(cardT);      //���ߵ�localPosition
            pos += m_FatherOffset;
            Vector3 up = m_Spline.Spline.EvaluateUpVector(cardT);       //�������ߵĴ���
            Vector3 tangent = m_Spline.Spline.EvaluateTangent(cardT);   //��������
            Quaternion quat = Quaternion.LookRotation(Vector3.Cross(tangent, up), up);

            m_HandCards[i].SetTrans(pos, quat, i);
        }
    }
}
