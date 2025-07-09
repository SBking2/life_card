using QF;
using System.Collections.Generic;
using UnityEngine;

public class CardModelContainer : Singleton<CardModelContainer>
{
    private Dictionary<string, CardModel> m_ModelDic = new Dictionary<string, CardModel>();

    private ModelReader m_Reader = new ModelReader(new SOModelReader());    //这是选择读取SO的数据
    
    public CardModelContainer()
    {
        m_ModelDic = m_Reader.Read();   //从表中读数据
    }

    public CardModel GetModelData(string id)
    {
            if (m_ModelDic.ContainsKey(id))
                return m_ModelDic[id];

            Debug.LogError(string.Format("Card Model Error : {0} is not exist!", id));
            return default;
    }
}