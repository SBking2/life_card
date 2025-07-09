
using UnityEngine;

public class CardViewFactory
{
    private GameObject m_CardViewPrefabs;
    public CardViewFactory()
    {
        m_CardViewPrefabs = ResMgr.Instance.Load<GameObject>("Prefabs/Card/View/CardView");
    }

    public CardView CreateCardView(GameObject cardObj)
    {
        GameObject obj = GameObject.Instantiate(m_CardViewPrefabs);
        CardView cardView = obj.GetComponent<CardView>();

        CardModel model = cardObj.GetComponent<CardModelComponent>().cardModel;
        cardView.Init(model.card_name, model.card_tex, cardObj);     //设置cardView的名字和图片

        return cardView;
    }
}
