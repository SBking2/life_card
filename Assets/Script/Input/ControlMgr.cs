using DG.Tweening;
using LC;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class CardControlMgr : MonoBehaviour
{
    private void OnEnable()
    {
        InputMgr.Instance.onExit += Exit;
        InputMgr.Instance.onSpaceAction += AddCard;
        InputMgr.Instance.onQuitAction += RemoveCard;
    }

    private void OnDisable()
    {
        InputMgr.Instance.onExit -= Exit;
        InputMgr.Instance.onSpaceAction -= AddCard;
        InputMgr.Instance.onQuitAction -= RemoveCard;
    }

    void Update()
    {
    }

    private void Exit()
    {
        UIMgr.Instance.HideUI<DrawcardPipleUI>();
        UIMgr.Instance.HideUI<DiscardPipleUI>();
    }

    public void ShowDrawCardPiplePanel()
    {
        UIMgr.Instance.ShowUI<DrawcardPipleUI>();
    }

    public void ShowDisCardPiplePanel()
    {
        UIMgr.Instance.ShowUI<DiscardPipleUI>();
    }

    private void AddCard()
    {
        CardMgr.Instance.DrawCard(4);
    }

    private void RemoveCard()
    {
        CardMgr.Instance.DisCardAllCard();
    }
}
