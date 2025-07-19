using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SkillCardModel
{
    public SkillCardModel(string id, string name, Sprite tex, string timelineName)
    {
        this.id = id;
        this.card_name = name;
        this.card_tex = tex;
        this.timelineModel = TimelineModelContainer.Instance.GetModelData(timelineName);
    }
    public string id;
    public string card_name;
    public Sprite card_tex;
    public TimelineModel timelineModel;
}
