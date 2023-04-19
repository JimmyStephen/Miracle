using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public static class GameplayDebug
{
    /// <summary>
    /// Output the hands of the player, and opponent to the console
    /// </summary>
    public static void OutputHands(Player player, EnemyAI opponent)
    {
        string pStr = $"Player Hand ({player.Hand.Count}): ";
        player.Hand.ForEach(card => { pStr += card.name + ", "; });
        string oStr = $"Opponent Hand: ({opponent.Hand.Count})";
        opponent.Hand.ForEach(card => { oStr += card.name + ", "; });
        Debug.Log($"{pStr}\n{oStr}");
    }
}
