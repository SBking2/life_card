using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Model/HeroCard", fileName = "Hero Card Model SO")]
public class HeroCardModelSO : ScriptableObject
{
    public string id;
    public string card_name;
    public string card_tex;

    public int max_hp;
    public int attack;
    public int defense;
}
