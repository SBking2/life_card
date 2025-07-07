using QF;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleMgr : Singleton<BattleMgr>
{
    //开放给外界的UI
    public Action onBattleStartAction;
    public Action onBattleEndAction;
    public Action onPlayerTurnStartAction;
    public Action onPlayerTurnEndAction;
    public Action onEnemyTurnStartAction;
    public Action onEnemyTurnEndAction;

    private List<GameObject> m_Players = new List<GameObject>();    //正方角色
    private List<GameObject> m_Enemys = new List<GameObject>();     //反方角色

    public void OnBattleStart()
    {

    }
    public void OnBattleEnd()
    {

    }

    public void OnPlayerTurnStart()
    {

    }

    public void OnPlayerTurnEnd()
    {

    }

    public void OnEnemyTurnStart()
    {

    }

    public void OnEnemyTurnEnd()
    {

    }

}