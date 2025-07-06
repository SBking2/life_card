using QF;

public class DamageMgr : Singleton<DamageMgr>
{
    public void Submit(DamageInfo damageInfo)
    {
        if (damageInfo.target == null) return;

        //Target死了，则不再需要处理当前DamgeInfo
        UnitState targetState = damageInfo.target.GetComponent<UnitState>();
        if (targetState.IsDead) return;

        BuffHandler attackerBuffHandler = null;
        BuffHandler targetBuffHandler = null;

        if(damageInfo.attacker != null) attackerBuffHandler = damageInfo.attacker.GetComponent<BuffHandler>();
        targetBuffHandler = damageInfo.target.GetComponent<BuffHandler>();

        //处理Attacker的OnHit
        if (attackerBuffHandler != null)
        {
            foreach(var buff in attackerBuffHandler.buffs)
            {
                if(buff.model.onHit != null)
                    buff.model.onHit(buff, ref damageInfo, damageInfo.target);
            }
        }

        //处理Target的OnBeHurt
        foreach (var buff in targetBuffHandler.buffs)
        {
            if (buff.model.onBeHurt != null)
                buff.model.onBeHurt(buff, ref damageInfo, damageInfo.attacker);
        }

        //判断target是否会死亡，如果死了，就触发OnKill和OnBeKill
        if (targetState.IsDamageCanKillMe(damageInfo))
        {
            //处理Attacker的OnKill
            if (attackerBuffHandler != null)
            {
                foreach (var buff in attackerBuffHandler.buffs)
                {
                    if (buff.model.onKill != null)
                        buff.model.onKill(buff, ref damageInfo, damageInfo.target);
                }
            }

            //处理Target的OnBeKill
            foreach (var buff in targetBuffHandler.buffs)
            {
                if (buff.model.onBeKill != null)
                    buff.model.onBeKill(buff, ref damageInfo, damageInfo.attacker);
            }
        }

        //TODO:伤害跳字之类的
        HandleDamageInfo(damageInfo);

        //走完伤害流程之后，如果Target没死，再加buff
        if(!targetState.IsDead)
        {
            foreach (var buff in damageInfo.add_buffs)
                targetBuffHandler.AddBuff(buff);
        }
    }

    /// <summary>
    /// 结算Damage
    /// </summary>
    /// <param name="damageInfo"></param>
    private void HandleDamageInfo(DamageInfo damageInfo)
    {

    }
}
