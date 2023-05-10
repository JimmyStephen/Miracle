using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoCardRule : Rule
{
    public NoCardRule(string name, string desc, Player player, EnemyAI AI) : base(name, desc, player, AI) { }

    public override int CheckRule()
    {
        return AI.Hand.Count == 0 ? 100 : 0;
    }

    public override Gameplay_Card RunRule()
    {
        return CardConnector.GetGameplayCard("Hand Empty!");
    }
}
