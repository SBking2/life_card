using DG.Tweening;
using QF;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Splines;

/// <summary>
/// HandView管理手牌的位置,直接管理CardObj
/// </summary>
public class HandViewMgr : MonoSingleton<HandViewMgr>
{
    private SplineContainer m_Spline;
    private Transform m_DiscardTrans;
    private Transform m_DrawPipleTrans;

    private float m_MinCardSpace = 0.05f;
    private float m_MaxCardSpace = 0.15f;

    private float duration = 0.15f;
    private Vector3 m_FatherOffset;
    private List<CardView> m_HandCards = new List<CardView>();

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

        m_DiscardTrans = GameObject.Find("DiscardPoint").transform;
        m_DrawPipleTrans = GameObject.Find("DrawPiplePoint").transform;

        Transform splieTrans = GameObject.Find("HandView").transform.Find("Spline");
        m_Spline = splieTrans.GetComponent<SplineContainer>();
        m_FatherOffset = splieTrans.localPosition;

        //监听数据层
        CardMgr.Instance.onDrawCard += DrawCard;
        CardMgr.Instance.onDiscardCard += RemoveCard;
    }

    private void DrawCard(List<GameObject> cardObjs)
    {
        StartCoroutine(StartDrawCard(cardObjs));
    }

    private IEnumerator StartDrawCard(List<GameObject> cardObjs)
    {
        foreach(var obj in cardObjs)
        {
            AddCard(obj);
            yield return new WaitForSeconds(0.15f);
        }
    }

    /// <summary>
    /// 在DrawPiplePoint生成一个CardView，然后Update
    /// </summary>
    /// <param name="cardView"></param>
    private void AddCard(GameObject cardObj)
    {
        //激活GameObjec的View层
        CardView cardView = cardObj.GetComponentInChildren<CardView>(true);
        cardView.gameObject.SetActive(true);

        m_HandCards.Add(cardView);

        cardView.transform.position = m_DrawPipleTrans.position;
        cardView.transform.localScale = Vector3.zero;
        cardView.transform.DOScale(1.0f, duration);

        UpdateCardPosition();
    }

    /// <summary>
    /// 把CardView移除，并移动到DiscardPoint
    /// </summary>
    /// <param name="cardView"></param>
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
        obj.transform.DOMove(m_DiscardTrans.position, duration);
        obj.transform.DOScale(0.0f, duration).OnComplete(() =>
        {
            obj.SetActive(false);
        });
        UpdateCardPosition();
    }
    
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
