using System.Collections.Generic;

public delegate void onTimelineNode(TimelineObj obj, params object[] args);     //TimelineNode事件的委托

public struct TimelineNode
{
    public TimelineNode(float timeElapsed, string funcName, params object[] args)
    {
        this.timeElapsed = timeElapsed;
        this.event_args = args;
        this.onEventOccur = TimelineScript.Instance.GetTimelineEvent(funcName);    //TODO:从脚本字典里面找到函数并赋值
    }
    public float timeElapsed;
    public object[] event_args;
    public onTimelineNode onEventOccur;
}

public struct TimelineModel
{
    public TimelineModel(string id, float duration, List<TimelineNode> nodes)
    {
        this.id = id;
        this.duration = duration;
        this.timeline_nodes = nodes;
    }

    public string id;
    public float duration;      //持续时间
    public List<TimelineNode> timeline_nodes;
}
