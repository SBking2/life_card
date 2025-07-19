using System.Collections.Generic;
using UnityEngine;

public interface ISkillCardModelReader
{
    public Dictionary<string, SkillCardModel> Read();
}

public interface IHeroCardModelReader
{
    public Dictionary<string, HeroCardModel> Read();
}

/// <summary>
/// 用于读取磁盘表数据
/// </summary>
public class ModelReader
{
    public ModelReader(ISkillCardModelReader skillCardModelReader, IHeroCardModelReader heroCardModelReader)
    {
        m_SkillCardModelReader = skillCardModelReader;
        m_HeroCardModelReader = heroCardModelReader;
    }

    private ISkillCardModelReader m_SkillCardModelReader;
    private IHeroCardModelReader m_HeroCardModelReader;

    public Dictionary<string, SkillCardModel> ReadCardModel() { return m_SkillCardModelReader.Read(); }
    public Dictionary<string, HeroCardModel> ReadHeroCardModel() { return m_HeroCardModelReader.Read(); }
}

/// <summary>
/// 从SO中读取数据
/// </summary>
public class SOCardModelReader : ISkillCardModelReader
{
    private const string m_TexPath = "Tex/Card/";

    public Dictionary<string, SkillCardModel> Read()
    {
        Dictionary<string, SkillCardModel> dic = new Dictionary<string, SkillCardModel>();
        SkillCardModelSO[] modelSO = Resources.LoadAll<SkillCardModelSO>("Model/Card");
        foreach(var so in modelSO)
        {
            Sprite tex = ResMgr.Instance.Load<Sprite>(m_TexPath + so.card_tex);
            SkillCardModel model = new SkillCardModel(so.id, so.card_name, tex, so.timeline_name);
            dic.Add(model.id, model);
        }

        return dic;
    }
}

public class SOHeroCardModelReader : IHeroCardModelReader
{
    private const string m_TexPath = "Tex/Card/";

    public Dictionary<string, HeroCardModel> Read()
    {
        Dictionary<string, HeroCardModel> dic = new Dictionary<string, HeroCardModel>();
        HeroCardModelSO[] modelSO = Resources.LoadAll<HeroCardModelSO>("Model/HeroCard");
        foreach (var so in modelSO)
        {
            Sprite tex = ResMgr.Instance.Load<Sprite>(m_TexPath + so.card_tex);
            HeroCardModel model = new HeroCardModel(so.id, so.card_name, tex, so.max_hp, so.attack, so.defense);
            dic.Add(model.id, model);
        }

        return dic;
    }
}

/// <summary>
/// 从Excel中读取数据
/// </summary>
public class ExcelCardModelReader : ISkillCardModelReader
{
    public Dictionary<string, SkillCardModel> Read()
    {
        throw new System.NotImplementedException();
    }
}