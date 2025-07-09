using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;


public class CardView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_CardBKSprite;
    [SerializeField] private TextMeshPro m_CardNameText;
    [SerializeField] private TextMeshPro m_CardContentText;
    [SerializeField] private TextMeshPro m_CardLevelText;
    [SerializeField] private float m_AnimDuration;

    public GameObject cardObj;

    private Vector3 m_OriginalPos;
    private Quaternion m_OriginalQuat;
    private int m_SortIndex;

    private SortingGroup m_SortGroup;

    private void Awake()
    {
        m_SortGroup = GetComponent<SortingGroup>();
    }

    public void Init(string cardName, Sprite cardBK, GameObject cardObj)
    {
        m_CardNameText.text = cardName;
        m_CardBKSprite.sprite = cardBK;
        this.cardObj = cardObj;
    }

    public void SetTrans(Vector3 pos, Quaternion quat, int sortIndex)
    {
        m_OriginalPos = pos;
        m_OriginalQuat = quat;
        m_SortIndex = sortIndex;
        m_SortGroup.sortingOrder = sortIndex;
        transform.DOMove(pos, m_AnimDuration);
        transform.DORotateQuaternion(quat, m_AnimDuration);
    }

    private void OnMouseEnter()
    {
        Vector3 pos = m_OriginalPos;
        pos.y = -1.5f;
        Quaternion rot = Quaternion.LookRotation(Vector3.forward, Vector3.up);

        transform.DOMove(pos, m_AnimDuration);
        transform.DORotateQuaternion(rot, m_AnimDuration);
        transform.DOScale(1.3f, m_AnimDuration);
        m_SortGroup.sortingOrder = HandViewMgr.Instance.HandCardCount;
    }

    private void OnMouseExit()
    {
        transform.DOMove(m_OriginalPos, m_AnimDuration);
        transform.DORotateQuaternion(m_OriginalQuat, m_AnimDuration);
        transform.DOScale(1.0f, m_AnimDuration);
        m_SortGroup.sortingOrder = m_SortIndex;
    }
}
