using QF;
using System.Collections.Generic;
using UnityEngine;

public class SkillCardModelContainer : Singleton<SkillCardModelContainer>
{
    private Dictionary<string, SkillCardModel> m_ModelDic = new Dictionary<string, SkillCardModel>();

    private ModelReader m_Reader = new ModelReader(new SOCardModelReader(), new SOHeroCardModelReader());    //这是选择读取SO的数据

    public SkillCardModelContainer()
    {
        m_ModelDic = m_Reader.ReadCardModel();   //从表中读数据
    }

    public SkillCardModel GetModelData(string id)
    {
            if (m_ModelDic.ContainsKey(id))
                return m_ModelDic[id];

            Debug.LogError(string.Format("Card Model Error : {0} is not exist!", id));
            return default;
    }
}