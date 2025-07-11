using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSlotUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_CardNameText;
    [SerializeField] private Image m_CardBKSprite;

    public GameObject cardObj;

    public void Init(string cardName, Sprite cardBK, GameObject cardObj)
    {
        m_CardNameText.text = cardName;
        m_CardBKSprite.sprite = cardBK;
        this.cardObj = cardObj;
    }
}
