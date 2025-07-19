using QF;
using System.Collections.Generic;
using UnityEngine;

public class TimelineModelContainer : Singleton<TimelineModelContainer>
{
    private Dictionary<string, TimelineModel> m_ModelDic = new Dictionary<string, TimelineModel>();


    public TimelineModelContainer()
    {
        m_ModelDic.Add("attack",
            new TimelineModel(
                    "attack",
                    0.15f,
                    new List<TimelineNode>
                    {
                        new TimelineNode(0.0f, "Dash", new object[]{  }),
                    }
                )
            );
    }

    public TimelineModel GetModelData(string id)
    {
        if (m_ModelDic.ContainsKey(id))
            return m_ModelDic[id];
        else
            return default;
    }
}