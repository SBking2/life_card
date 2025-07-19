using QF;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ����ս���ڼ俨�Ƶ�����
/// </summary>
public class CardMgr : MonoSingleton<CardMgr>
{
    public List<string> m_InitCards = new List<string>();

    public Action<List<GameObject>> onDrawCard;
    public Action<GameObject> onDiscardCard;

    [SerializeField] public List<GameObject> m_DrawPipleCards = new List<GameObject>();
    [SerializeField] public List<GameObject> m_DiscardCards = new List<GameObject>();
    [SerializeField] private List<GameObject> m_HandCards = new List<GameObject>();


    public int CanDrawCardCount
    {
        get
        {
            return m_DrawPipleCards.Count + m_DiscardCards.Count;
        }
    }

    private GameObjectFactory m_CardObjFactory = new GameObjectFactory(new CardObjFactory());
    private GameObjectFactory m_HeroCardObjFactory = new GameObjectFactory(new HeroCardObjFactory());

    protected override void Awake()
    {
        base.Awake();
        //���ݳ�ʼ��Card�����ݣ�����CardObj
        foreach(var id in m_InitCards)
        {
            GameObject obj;
            if (id[1] == '0')
                obj = m_CardObjFactory.CreateObject(id);
            else
                obj = m_HeroCardObjFactory.CreateObject(id);

            m_DrawPipleCards.Add(obj);
        }
    }

    public void DrawCard(int count)
    {
        count = Mathf.Min(CanDrawCardCount, count);     //���Գ��Ƶ�����

        List<GameObject> drawCards = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            if (m_DrawPipleCards.Count < 1)
            {
                Discard2Drawpiple();    //���������ƶѷ�����ƶ���
            }
            m_HandCards.Add(m_DrawPipleCards[0]);
            drawCards.Add(m_DrawPipleCards[0]);
            m_DrawPipleCards.RemoveAt(0);
        }
        onDrawCard?.Invoke(drawCards);
    }

    /// <summary>
    /// ��CardObj����
    /// </summary>
    /// <param name="cardObj"></param>
    public void DiscardCard(GameObject cardObj)
    {
        onDiscardCard?.Invoke(cardObj);
        m_HandCards.Remove(cardObj);
        m_DiscardCards.Add(cardObj);
    }

    /// <summary>
    /// ������ȫ������
    /// </summary>
    public void DisCardAllCard()
    {
        for (int i = m_HandCards.Count - 1; i >= 0; i--)
        {
            DiscardCard(m_HandCards[i]);
        }
    }

    /// <summary>
    /// �����ƶѵ���ת�Ƶ����ƶ�,���Գ��ƶ�ϴ��
    /// </summary>
    private void Discard2Drawpiple()
    {
        for(int i = m_DiscardCards.Count - 1; i >= 0; i--)
        {
            m_DrawPipleCards.Add(m_DiscardCards[i]);
        }
        m_DiscardCards.Clear();
        ShuffleCard();
    }

    /// <summary>
    /// �Գ��ƶѽ���ϴ��
    /// </summary>
    private void ShuffleCard()
    {
        for (int i = 0; i < m_DrawPipleCards.Count - 1; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i + 1, m_DrawPipleCards.Count - 1);
            GameObject obj = m_DrawPipleCards[randomIndex];
            m_DrawPipleCards[randomIndex] = m_DrawPipleCards[i];
            m_DrawPipleCards[i] = obj;
        }
    }
}
