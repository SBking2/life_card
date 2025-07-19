using DG.Tweening;
using QF;
using System.Collections.Generic;
using UnityEngine;

public class TimelineScript : Singleton<TimelineScript>
{
    public Dictionary<string, onTimelineNode> m_TimeNodeEventDic = new Dictionary<string, onTimelineNode>();

    public TimelineScript()
    {
        m_TimeNodeEventDic.Add("Dash", Dash);
    }

    public onTimelineNode GetTimelineEvent(string name)
    {
        return m_TimeNodeEventDic[name];
    }

    /// <summary>
    /// 攻击动画，往前冲刺，然后回到原位
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="args"></param>
    private void Dash(TimelineObj obj, params object[] args)
    {
        Vector3 pos = obj.caster.transform.position + new Vector3(2.0f, 0.0f, 0.0f);
        Vector3 originalPosition = obj.caster.transform.position;

        Sequence sequence = DOTween.Sequence();

        // 1. 向前冲刺
        sequence.Append(
            obj.caster.transform.DOMove(pos, 0.1f)
            .SetEase(Ease.OutQuad)  // 加速冲刺效果
        );

        // 2. 返回原位
        sequence.Append(
            obj.caster.transform.DOMove(originalPosition, 0.1f)
            .SetEase(Ease.InQuad)  // 减速返回效果
        );

        sequence.Play();
    }
}
