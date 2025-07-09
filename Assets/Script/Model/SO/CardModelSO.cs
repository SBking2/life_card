using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Model/Card", fileName = "Card Model SO")]
public class CardModelSO : ScriptableObject
{
    public string id;
    public string card_name;
    public string card_tex;

    public int max_hp;
    public int attack;
    public int defense;
}
