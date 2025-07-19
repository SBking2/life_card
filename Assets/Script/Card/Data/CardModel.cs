using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CardModel
{
    public CardModel(string id, string name, Sprite tex, int hp, int attack, int defense, string timelineName)
    {
        this.id = id;
        this.card_name = name;
        this.max_hp = hp;
        this.card_tex = tex;
        this.attack = attack;
        this.defense = defense;
        this.timelineModel = TimelineModelContainer.Instance.GetModelData(timelineName);
    }
    public string id;
    public string card_name;
    public Sprite card_tex;
    public int max_hp;
    public int attack;
    public int defense;
    public TimelineModel timelineModel;
}
