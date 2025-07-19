using QF;
using System.Collections.Generic;
using UnityEngine;

public class HeroCardModelContainer : Singleton<HeroCardModelContainer>
{
    private Dictionary<string, HeroCardModel> m_ModelDic = new Dictionary<string, HeroCardModel>();

    private ModelReader m_Reader = new ModelReader(new SOCardModelReader(), new SOHeroCardModelReader());    //这是选择读取SO的数据

    public HeroCardModelContainer()
    {
        m_ModelDic = m_Reader.ReadHeroCardModel();   //从表中读数据
    }

    public HeroCardModel GetModelData(string id)
    {
        if (m_ModelDic.ContainsKey(id))
            return m_ModelDic[id];

        Debug.LogError(string.Format("Hero Card Model Error : {0} is not exist!", id));
        return default;
    }
}