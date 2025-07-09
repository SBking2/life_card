using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
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
        CardMgr.Instance.DrawCard(4);
    }

    private void RemoveCard()
    {
        CardMgr.Instance.DisCardAllCard();
    }
}
