using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscardPipleUI : MonoBehaviour, IUIBase
{
    [SerializeField] private Transform m_CardItemContent;
    private List<CardSlotUI> m_CardItemUIObjs = new List<CardSlotUI>();
    private CardItemUIFactory m_Factory = new CardItemUIFactory();
    private void Awake()
    {
        m_Factory.Init();
        UIMgr.Instance.RegisterUI(this.GetType(), this);
        HideMe();
    }

    private void OnDestroy()
    {
        UIMgr.Instance.UnRegisterUI<DrawcardPipleUI>();
    }

    private void UpdateItemUI()
    {
        foreach (var ui in m_CardItemUIObjs)
        {
            DestroyImmediate(ui.gameObject);
        }
        m_CardItemUIObjs.Clear();

        foreach (var obj in CardMgr.Instance.m_DiscardCards)
        {
            m_CardItemUIObjs.Add(m_Factory.CreateCardItemUI(obj, (obj) =>
            {
                obj.transform.SetParent(m_CardItemContent, false);
            }));
        }
    }

    public void HideMe()
    {
        gameObject.SetActive(false);
    }

    public void ShowMe()
    {
        UpdateItemUI();
        gameObject.SetActive(true);
        ScrollRect scrollRect = this.gameObject.GetComponent<ScrollRect>();
        scrollRect.content.position -= new Vector3(0.0f, 50.0f, 0.0f);
    }
}
