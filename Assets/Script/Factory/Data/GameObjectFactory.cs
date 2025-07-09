using UnityEngine;

public interface ICreateObject
{
    public GameObject CreateObj(string id);
}

public class GameObjectFactory
{
    private ICreateObject m_Creator;
    public GameObjectFactory(ICreateObject creator)
    {
        m_Creator = creator;
    }
    public GameObject CreateObject(string id)
    {
        return m_Creator.CreateObj(id);
    }
}

public class CardObjFactory : ICreateObject
{
    private Transform m_ObjFather;

    public CardObjFactory()
    {
    }

    public GameObject CreateObj(string id)
    {
        GameObject cardObjRes = ResMgr.Instance.Load<GameObject>("Prefabs/Card/Obj/CardObj");   //从磁盘中读取Prefabs的数据

        if(m_ObjFather == null)
            m_ObjFather = GameObject.Find("CardObj").transform;

        GameObject obj = GameObject.Instantiate(cardObjRes, m_ObjFather, false);    //实例化Obj，并放到Obj数据的父节点下

        //获取model，赋值给实例化的obj
        CardModelComponent modelComponent= obj.GetComponent<CardModelComponent>();
        CardModel model = CardModelContainer.Instance.GetModelData(id);
        modelComponent.cardModel = model;   //拷贝赋值

        //设置Card的UnitState
        UnitProperty property = new UnitProperty(model.max_hp, model.attack, model.defense);
        UnitResource resource = new UnitResource(model.max_hp, 0);

        UnitState state = obj.GetComponent<UnitState>();
        state.Init(property, resource);

        return obj;
    }
}

public class EnemyFactory : ICreateObject
{
    public GameObject CreateObj(string id)
    {
        throw new System.NotImplementedException();
    }
}
