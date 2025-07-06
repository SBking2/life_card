using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAction
{
    public List<GameAction> PreActions = new List<GameAction>();
    public List<GameAction> CurActions = new List<GameAction>();
    public List<GameAction> PostActions = new List<GameAction>();
}
