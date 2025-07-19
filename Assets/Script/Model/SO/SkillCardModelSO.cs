using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Model/Skill Card", fileName = "Skill Card Model SO")]
public class SkillCardModelSO : ScriptableObject
{
    public string id;
    public string card_name;
    public string card_tex;

    public string timeline_name;
}
