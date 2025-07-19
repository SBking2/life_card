using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;


public class CardView : MonoBehaviour
{
    public enum CardState
    {
        None = 0,
        Horvered = 1,
        Selected = 2,
        Caste = 3
    }

    [SerializeField] private SpriteRenderer m_CardBKSprite;
    [SerializeField] private TextMeshPro m_CardNameText;
    [SerializeField] private TextMeshPro m_CardContentText;
    [SerializeField] private TextMeshPro m_CardLevelText;

    private float m_AnimDuration = 0.15f;
    private float m_HorveredScale = 1.2f;

    [HideInInspector] public GameObject cardObj;
    public CardState CurrentState { get; private set; }

    //卡牌在手牌序列中应该存在的属性
    private Vector3 m_OriginalPos;      
    private Quaternion m_OriginalQuat;
    private int m_SortIndex;

    private SortingGroup m_SortGroup;

    private void Awake()
    {
        m_SortGroup = GetComponent<SortingGroup>();
    }

    private void OnEnable()     //用于刷新卡牌view的状态
    {
        EnterState(CardState.None, true);
    }

    /// <summary>
    /// 根据Card数据更新View
    /// </summary>
    /// <param name="cardObj"></param>
    public void UpdateView(GameObject cardObj = null)
    {
        if (cardObj != null)
            this.cardObj = cardObj;
        CardModel model = this.cardObj.GetComponent<CardModelComponent>().cardModel;

        //设置cardView的名字和图片
        m_CardNameText.text = model.card_name;
        m_CardBKSprite.sprite = model.card_tex;
        this.cardObj = cardObj;
    }

    /// <summary>
    /// 仅仅设置Card如果在手牌中，应该在哪个位置
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="quat"></param>
    /// <param name="sortIndex"></param>
    public void SetTrans(Vector3 pos, Quaternion quat, int sortIndex)
    {
        m_OriginalPos = pos;
        m_OriginalQuat = quat;
        m_SortIndex = sortIndex;
        m_SortGroup.sortingOrder = sortIndex;

        EnterState(CardState.None);     //更新位置
    }

    /// <summary>
    /// 切换Card状态
    /// </summary>
    /// <param name="state"></param>
    /// <param name="isForce">是否强制切换</param>
    public void EnterState(CardState state, bool isForce = false)
    {
        //Caste和Selecet状态下，不允许切换到None状态
        if (!isForce && CurrentState != CardState.None && state == CardState.None) return;

        CurrentState = state;
        switch(state)
        {
            case CardState.None:
                OnNone();
                break;
            case CardState.Horvered:
                OnHorvered();
                break;
            case CardState.Selected:
                OnSelected();
                break;
            case CardState.Caste:
                OnCaste();
                break;
        }
    }

    private void OnNone()
    {
        transform.DOMove(m_OriginalPos, m_AnimDuration);
        transform.DORotateQuaternion(m_OriginalQuat, m_AnimDuration);
        transform.DOScale(1.0f, m_AnimDuration);
        m_SortGroup.sortingOrder = m_SortIndex;
    }

    private void OnHorvered()
    {
        Vector3 pos = m_OriginalPos;
        pos.y = -2.0f;
        Quaternion rot = Quaternion.LookRotation(Vector3.forward, Vector3.up);

        transform.DOMove(pos, m_AnimDuration);
        transform.DORotateQuaternion(rot, m_AnimDuration);
        transform.DOScale(m_HorveredScale, m_AnimDuration);
        m_SortGroup.sortingOrder = HandViewMgr.Instance.HandCardCount;
    }

    private void OnSelected()
    {
        Sequence bounceSequence = DOTween.Sequence();
        bounceSequence.Append(transform.DOScale(m_HorveredScale * 0.8f, 0.1f));
        bounceSequence.Append(transform.DOScale(m_HorveredScale, 0.1f)
            .SetEase(Ease.OutElastic, 1, 0.1f));

        // 播放序列
        bounceSequence.Play();
    }

    private void OnCaste()
    {

    }
}
