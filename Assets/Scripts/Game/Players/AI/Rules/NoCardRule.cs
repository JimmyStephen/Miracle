using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoCardRule : Rule
{
    public NoCardRule(string name, string desc, Player player, EnemyAI AI) : base(name, desc, player, AI) { }

    public override int CheckRule()
    {
        return AI.Hand.Count == 0 ? 100 : 1;
    }

    public override Gameplay_Card RunRule()
    {
        Debug.Log("AI NoCardRule | HandSize: " + AI.Hand.Count);
        if (AI.Hand.Count == 0)
            return CardConnector.GetGameplayCard("Hand Empty!");
        else
            return AI.Hand[Random.Range(0, AI.Hand.Count)];
    }
}
