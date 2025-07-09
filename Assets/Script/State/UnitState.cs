using UnityEngine;

public class UnitState : MonoBehaviour
{
    public bool IsDead { get; private set; }
    private UnitProperty m_Property;
    private UnitResource m_Resource;

    /// <summary>
    /// 根据model数据初始化unit state
    /// </summary>
    /// <param name="property"></param>
    /// <param name="resource"></param>
    public void Init(UnitProperty property, UnitResource resource)
    {
        m_Property = property;
        m_Resource = resource;
    }

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
