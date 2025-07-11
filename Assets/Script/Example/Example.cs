using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

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

    private CardView m_HorverdCardView;

    private void Start()
    {
        print(HandViewMgr.Instance);
    }

    /// <summary>
    /// 测试，往HandView里加卡牌
    /// </summary>
    private void Update()
    {
        if(Keyboard.current.escapeKey.isPressed)
        {
            UIMgr.Instance.HideUI<DrawcardPipleUI>();
            UIMgr.Instance.HideUI<DiscardPipleUI>();
        }

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction);

        if (hits.Length > 0)
        {
            RaycastHit topHit = hits.OrderByDescending(h => h.transform.GetComponent<SortingGroup>().sortingOrder).First();

            CardView cardView = topHit.collider.gameObject.GetComponent<CardView>();
            // 处理topHit的对象
            if (m_HorverdCardView == null || m_HorverdCardView != cardView)
            {
                m_HorverdCardView?.OnUnHorvered();
                m_HorverdCardView = cardView;
                m_HorverdCardView.OnHorvered();
            }
        }else if(m_HorverdCardView)
        {
            m_HorverdCardView.OnUnHorvered();
            m_HorverdCardView = null;
        }
    }
    private void AddCard()
    {
        CardMgr.Instance.DrawCard(4);
    }

    private void RemoveCard()
    {
        CardMgr.Instance.DisCardAllCard();
    }

    public void ShowDrawCardPiplePanel()
    {
        UIMgr.Instance.ShowUI<DrawcardPipleUI>();
    }

    public void ShowDisCardPiplePanel()
    {
        UIMgr.Instance.ShowUI<DiscardPipleUI>();
    }
}
