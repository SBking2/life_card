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

    private CardView m_HorverdCardView;
    private CardView m_SelectCardView;

    private bool m_IsSelecting = false;
    [SerializeField] private LayerMask m_CardLayerMask;
    [SerializeField] private Collider m_RealeaseCardZone;

    void Update()
    {
        //�������
        if(!m_IsSelecting)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, 100.0f, m_CardLayerMask);

            if (hits.Length > 0)
            {
                RaycastHit topHit = hits.OrderByDescending(h => h.transform.GetComponent<SortingGroup>().sortingOrder).First();

                CardView cardView = topHit.collider.gameObject.GetComponent<CardView>();
                // ����topHit�Ķ���
                if (m_HorverdCardView == null || m_HorverdCardView != cardView)
                {
                    m_HorverdCardView?.OnUnHorvered();
                    m_HorverdCardView = cardView;
                    m_HorverdCardView.OnHorvered();
                }
            }
            else if (m_HorverdCardView)
            {
                m_HorverdCardView.OnUnHorvered();
                m_HorverdCardView = null;
            }
        }

        //�����,ѡ�����߼�
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            if(!m_IsSelecting && m_HorverdCardView != null)
            {
                //����Select״̬
                m_IsSelecting = true;
                m_SelectCardView = m_HorverdCardView;
            }else if(m_IsSelecting)
            {
                //�˳�Select״̬
                m_IsSelecting = false;
                m_SelectCardView.OnUnHorvered();
                m_SelectCardView = null;
            }
        }

        //���ֿ���������ͬ��
        if(m_IsSelecting)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 1.0f));
            pos.z = m_SelectCardView.transform.position.z;
            m_SelectCardView.transform.position = Vector3.Lerp(m_SelectCardView.transform.position, pos, 20.0f * Time.deltaTime);
        }

        //�����ͷ��߼�
        if(m_IsSelecting && Mouse.current.rightButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if(m_RealeaseCardZone.Raycast(ray, out hit, 100.0f))
            {
                Debug.Log("�ͷţ�");
            }
        }
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
