using DG.Tweening;
using QF;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Splines;

/// <summary>
/// HandView�������Ƶ�λ��,ֱ�ӹ���CardObj
/// </summary>
public class HandViewMgr : Singleton<HandViewMgr>
{
    private SplineContainer m_Spline;
    private Transform m_DiscardTrans;
    private Transform m_DrawPipleTrans;

    private float duration = 0.15f;
    private Vector3 m_FatherOffset;
    private List<CardView> m_HandCards = new List<CardView>();
    private CardViewFactory m_CardViewFactory = new CardViewFactory();

    public int HandCardCount
    {
        get
        {
            return m_HandCards.Count;
        }
    }

    public HandViewMgr()
    {
        m_DiscardTrans = GameObject.Find("DiscardPoint").transform;
        m_DrawPipleTrans = GameObject.Find("DrawPiplePoint").transform;

        Transform splieTrans = GameObject.Find("HandView").transform.Find("Spline");
        m_Spline = splieTrans.GetComponent<SplineContainer>();
        m_FatherOffset = splieTrans.localPosition;
    }

    /// <summary>
    /// ��DrawPiplePoint����һ��CardView��Ȼ��Update
    /// </summary>
    /// <param name="cardView"></param>
    public void AddCard(GameObject cardObj)
    {
        CardView cardView = m_CardViewFactory.CreateCardView(cardObj);      //����CardView

        m_HandCards.Add(cardView);

        cardView.transform.position = m_DrawPipleTrans.position;
        cardView.transform.localScale = Vector3.zero;
        cardView.transform.DOScale(1.0f, duration);

        UpdateCardPosition();
    }

    /// <summary>
    /// ��CardView�Ƴ������ƶ���DiscardPoint
    /// </summary>
    /// <param name="cardView"></param>
    public void RemoveCard(GameObject cardObj)
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
        obj.transform.DOMove(m_DiscardTrans.position, duration);
        obj.transform.DOScale(0.0f, duration).OnComplete(() =>
        {
            GameObject.Destroy(obj);
        });
        UpdateCardPosition();
    }
    
    private void UpdateCardPosition()
    {
        if (m_HandCards.Count == 0) return;
        float cardSpacing = 1f / m_HandCards.Count;
        float firstPos = cardSpacing / 2;

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
