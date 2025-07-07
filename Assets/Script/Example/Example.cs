using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    public GameObject cardObjRes;
    private void OnEnable()
    {
        LC.InputMgr.Instance.onSpaceAction += AddCard;
        LC.InputMgr.Instance.onQuitAction += RemoveCard;
    }

    private void OnDisable()
    {
        LC.InputMgr.Instance.onSpaceAction -= AddCard;
        LC.InputMgr.Instance.onQuitAction -= RemoveCard;
    }

    /// <summary>
    /// ≤‚ ‘£¨Õ˘HandView¿Ôº”ø®≈∆
    /// </summary>
    private void AddCard()
    {
        GameObject obj = GameObject.Instantiate(cardObjRes);
        HandMgr.Instance.AddCard(obj.GetComponent<CardView>());
    }

    private void RemoveCard()
    {
        HandMgr.Instance.RemoveCard();
    }
}
