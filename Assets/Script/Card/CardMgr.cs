using QF;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ս���ڼ俨�Ƶ�����
/// </summary>
public class CardMgr : Singleton<CardMgr>
{
    private List<GameObject> m_DrawPipleCards = new List<GameObject>();
    private List<GameObject> m_DiscardCards = new List<GameObject>();
    private List<GameObject> m_HandCards = new List<GameObject>();
}
