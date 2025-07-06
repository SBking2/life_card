using System.Collections.Generic;
using UnityEngine;

public class DamageInfo
{
    public GameObject attacker;
    public GameObject target;
    public DamageTag tag;        //记录该Damage的Tag
    public Damge damage;
    public float critical_rate;     //暴击率
    public List<BuffObj> add_buffs; //本次Damage要给target上什么buff

    /// <summary>
    /// 这个Damage是否是治疗
    /// </summary>
    /// <returns></returns>
    public bool isHead()
    {
        if (tag == DamageTag.DirectHeal || tag == DamageTag.PeriodHeal)
            return true;
        return false;
    }
}

public struct Damge
{
    public float value;
    public DamageType type;     //伤害类型：例如火属性、雷属性等
}

public enum DamageType
{
    Physics = 0,    //物理
    Fire = 1,       //火
    Electric = 2,   //电
    Magic = 3      //魔法
}

public enum DamageTag
{
    DirectDamage = 0,   //直接伤害
    PeriodDamage = 1,   //间接伤害
    ReflectDamage = 2,  //反噬伤害
    DirectHeal = 3,     //直接治疗
    PeriodHeal = 4      //间接治疗
}


