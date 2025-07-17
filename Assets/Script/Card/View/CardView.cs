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

    private float m_HorveredScale = 1.2f;


    public GameObject cardObj;

    public bool IsHorvered { get; private set; }
    public bool IsSelected { get; private set; }
    public bool IsCanInteract { get; private set; } = true;

    private Vector3 m_OriginalPos;
    private Quaternion m_OriginalQuat;
    private int m_SortIndex;

    private SortingGroup m_SortGroup;

    private void Awake()
    {
        m_SortGroup = GetComponent<SortingGroup>();
    }

    private void OnEnable()     //����ˢ�¿���view��״̬
    {
        IsCanInteract = true;
        IsSelected = false;
        IsHorvered = false;
    }

    /// <summary>
    /// ����Card���ݸ���View
    /// </summary>
    /// <param name="cardObj"></param>
    public void UpdateView(GameObject cardObj = null)
    {
        if (cardObj != null)
            this.cardObj = cardObj;
        CardModel model = this.cardObj.GetComponent<CardModelComponent>().cardModel;
        UpdateView(model.card_name, model.card_tex, cardObj);     //����cardView�����ֺ�ͼƬ
    }

    private void UpdateView(string cardName, Sprite cardBK, GameObject cardObj)
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

        if (IsSelected || !IsCanInteract)
            return;

        transform.DOMove(pos, m_AnimDuration);
        transform.DORotateQuaternion(quat, m_AnimDuration);
    }

    public void OnHorvered()
    {
        if (IsHorvered || !IsCanInteract) return;
        IsHorvered = true;
        Vector3 pos = m_OriginalPos;
        pos.y = -2.0f;
        Quaternion rot = Quaternion.LookRotation(Vector3.forward, Vector3.up);

        transform.DOMove(pos, m_AnimDuration);
        transform.DORotateQuaternion(rot, m_AnimDuration);
        transform.DOScale(m_HorveredScale, m_AnimDuration);
        m_SortGroup.sortingOrder = HandViewMgr.Instance.HandCardCount;
    }

    public void OnUnHorvered()
    {
        if (!IsHorvered || !IsCanInteract) return;
        IsHorvered = false;
        transform.DOMove(m_OriginalPos, m_AnimDuration);
        transform.DORotateQuaternion(m_OriginalQuat, m_AnimDuration);
        transform.DOScale(1.0f, m_AnimDuration);
        m_SortGroup.sortingOrder = m_SortIndex;
    }

    public void OnSelected()
    {
        if (IsSelected) return;
        IsSelected = true;
        Sequence bounceSequence = DOTween.Sequence();
        bounceSequence.Append(transform.DOScale(m_HorveredScale * 0.8f, 0.1f));
        bounceSequence.Append(transform.DOScale(m_HorveredScale, 0.1f)
            .SetEase(Ease.OutElastic, 1, 0.1f));

        // ��������
        bounceSequence.Play();
    }

    public void OnUnSelected()
    {
        if (!IsSelected) return;
        IsSelected = false;
    }

    /// <summary>
    /// �ͷſ���
    /// </summary>
    public void Caste()
    {
        IsCanInteract = false;
    }
}
