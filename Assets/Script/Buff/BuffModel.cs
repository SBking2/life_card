﻿using UnityEngine;

public delegate void OnGet(BuffObj buffObj, int modifyStack);
public delegate void OnRemove(BuffObj buffObj);
public delegate void OnTurnStart(BuffObj buffObj);
public delegate void OnTurnEnd(BuffObj buffObj);
public delegate void OnHit(BuffObj buffObj, ref DamageInfo damageInfo, GameObject target);
public delegate void OnBeHurt(BuffObj buffObj, ref DamageInfo damageInfo, GameObject attacker);
public delegate void OnKill(BuffObj buffObj, ref DamageInfo damageInfo, GameObject target);
public delegate void OnBeKill(BuffObj buffObj, ref DamageInfo damageInfo, GameObject attacker);
public struct BuffModel
{
    public BuffModel(string id, int priority, int max_stack, bool isPermanent, int durationTurn
        , string onGetFun, string onRemoveFun, string onTurnStartFunc, string onTurnEndFunc
        , string onHitFunc, string onBeHurtFunc, string onKillFunc, string onBeKillFunc)
    {
        this.id = id;
        this.priority = priority;
        this.max_stack = max_stack;
        this.is_permanent = isPermanent;
        this.duration_turn = durationTurn;

        //TODO:根据string加载脚本
        this.onGet = null;
        this.onRemove = null;
        this.onTurnStart = null;
        this.onTurnEnd = null;
        this.onHit = null;
        this.onBeHurt = null;
        this.onKill = null;
        this.onBeKill = null;
}

    public string id;
    public int priority;
    public int max_stack;
    public bool is_permanent;
    public int duration_turn;  //生命周期，可以存在几个回合(一个玩家回合 + 一个敌人回合当作一个回合)
    //TODO:角色属性

    public OnGet onGet;
    public OnRemove onRemove;
    public OnTurnStart onTurnStart;
    public OnTurnEnd onTurnEnd;
    public OnHit onHit;
    public OnBeHurt onBeHurt;
    public OnKill onKill;
    public OnBeKill onBeKill;
}
