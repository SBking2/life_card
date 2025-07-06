using UnityEngine;

public class UnitState : MonoBehaviour
{
    public bool IsDead { get; private set; }
    private UnitProperty m_Property;
    private UnitResource m_Resource;

    /// <summary>
    /// 这个DamageInfo是否能杀死我
    /// </summary>
    /// <param name="damageInfo"></param>
    /// <returns></returns>
    public bool IsDamageCanKillMe(DamageInfo damageInfo)
    {
        return m_Resource.hp <= damageInfo.damage.value;
    }
}
