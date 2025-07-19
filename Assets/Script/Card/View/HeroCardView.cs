using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCardView : BaseCardView
{
    public override void UpdateView(GameObject cardObj = null)
    {
        if (cardObj != null)
            this.cardObj = cardObj;
        HeroCardModel model = this.cardObj.GetComponent<HeroCardModelComponent>().model;

        //����cardView�����ֺ�ͼƬ
        m_CardNameText.text = model.card_name;
        m_CardBKSprite.sprite = model.card_tex;
        this.cardObj = cardObj;
    }
}
