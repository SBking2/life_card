using System;
using UnityEngine;

public class CardItemUIFactory
{
    private GameObject m_CardItemUIPrefabs;
    public void Init()
    {
        m_CardItemUIPrefabs = ResMgr.Instance.Load<GameObject>("Prefabs/Card/UI/CardItemUI");
    }
    public CardSlotUI CreateCardItemUI(GameObject cardObj, Action<GameObject> callback = null)
    {
        GameObject obj = GameObject.Instantiate(m_CardItemUIPrefabs);
        CardSlotUI cardui = obj.GetComponent<CardSlotUI>();

        SkillCardModel model = cardObj.GetComponent<SkillCardModelComponent>().model;
        cardui.Init(model.card_name, model.card_tex, cardObj);     //设置cardView的名字和图片

        if(callback != null )
        {
            callback(cardui.gameObject);
        }

        return cardui;
    }
}
