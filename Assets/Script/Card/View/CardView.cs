using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cardBKSprite;
    [SerializeField] private TextMeshPro cardNameText;
    [SerializeField] private TextMeshPro cardContentText;
    [SerializeField] private TextMeshPro cardLevelText;

    public GameObject cardObj;

    public void Init(string cardName, Sprite cardBK, GameObject cardObj)
    {
        cardNameText.text = cardName;
        cardBKSprite.sprite = cardBK;
        this.cardObj = cardObj;
    }
}
