using QF;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineMgr : MonoSingleton<TimelineMgr>
{
    private List<TimelineObj> m_Timelines = new List<TimelineObj>();


    private void Update()
    {
        float delta = Time.deltaTime;

        List<TimelineObj> removeTimeline = new List<TimelineObj>();

        foreach(var timline in m_Timelines)
        {
            float wasTimeElapsed = timline.timeElapsed;
            timline.timeElapsed += delta;
            foreach(var node in timline.model.timeline_nodes)
            {
                if(node.timeElapsed < timline.timeElapsed &&
                    node.timeElapsed >= wasTimeElapsed)
                {
                    node.onEventOccur(timline, node.event_args);        //触发事件结点
                }
            }

            if(timline.timeElapsed >= timline.model.duration)
                removeTimeline.Add(timline);
        }

        //移除已经到达生命周期的timeline
        foreach(var timeline in removeTimeline)
        {
            m_Timelines.Remove(timeline);
        }
    }

    public void AddTimeline(TimelineObj timelineObj)
    {
        m_Timelines.Add(timelineObj);
    }

    public void AddTimeline(TimelineModel model, GameObject caster)
    {
        TimelineObj obj = new TimelineObj();
        obj.model = model;
        obj.caster = caster;
        obj.timeElapsed = 0.0f;

        m_Timelines.Add(obj);
    }
}
