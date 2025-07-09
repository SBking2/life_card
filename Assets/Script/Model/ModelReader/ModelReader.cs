using System.Collections.Generic;
using UnityEngine;

public interface IModelReader
{
    public Dictionary<string, CardModel> Read();
}

/// <summary>
/// 用于读取磁盘表数据
/// </summary>
public class ModelReader
{
    public ModelReader(IModelReader modelReader)
    {
        m_ModelReader = modelReader;
    }

    private IModelReader m_ModelReader;

    public Dictionary<string, CardModel> Read() { return m_ModelReader.Read(); }
}

/// <summary>
/// 从SO中读取数据
/// </summary>
public class SOModelReader : IModelReader
{
    private const string m_TexPath = "Tex/Card/";

    public Dictionary<string, CardModel> Read()
    {
        Dictionary<string, CardModel> dic = new Dictionary<string, CardModel>();
        CardModelSO[] modelSO = Resources.LoadAll<CardModelSO>("Model/Card");
        foreach(var so in modelSO)
        {
            Sprite tex = ResMgr.Instance.Load<Sprite>(m_TexPath + so.card_tex);
            CardModel model = new CardModel(so.id, so.card_name, tex, so.max_hp, so.attack, so.defense);
            dic.Add(model.id, model);
        }

        return dic;
    }
}

/// <summary>
/// 从Excel中读取数据
/// </summary>
public class ExcelModelReader : IModelReader
{
    public Dictionary<string, CardModel> Read()
    {
        throw new System.NotImplementedException();
    }
}