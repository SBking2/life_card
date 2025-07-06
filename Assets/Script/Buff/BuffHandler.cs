using System.Collections.Generic;
using UnityEngine;

public class BuffHandler : MonoBehaviour
{
    public List<BuffObj> buffs = new List<BuffObj>();

    public void AddBuff(BuffObj buff)
    {
        //加Buff逻辑
        buffs.Add(buff);
    }
}
