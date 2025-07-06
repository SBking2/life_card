
using UnityEngine;

public struct BuffObj
{
    public BuffModel model;     //Buff的预设数据
    public int ticked_time;     //buff Tick过的次数
    public int elapsed_time;    //存在的回合数
    public int stack;
    public GameObject caster;   //Buff的施加者
    public GameObject carrier;  //Buff的持有者
}