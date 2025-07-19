using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;


public class SkillCardView : BaseCardView
{
    [SerializeField] private TextMeshPro m_CardContentText;

    public override void UpdateView(GameObject cardObj = null)
    {
        if (cardObj != null)
            this.cardObj = cardObj;
        SkillCardModel model = this.cardObj.GetComponent<SkillCardModelComponent>().model;

        //设置cardView的名字和图片
        m_CardNameText.text = model.card_name;
            m_CardBKSprite.sprite = model.card_tex;
            this.cardObj = cardObj;
    }
}
