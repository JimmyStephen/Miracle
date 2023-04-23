using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static Enums;

public static class GameplayValidator
{
    public static bool CheckWinner(Player player, EnemyAI opponent)
    {
        return (player.CurrentHealth <= 0 || opponent.CurrentHealth <= 0 || CheckUNO(player, opponent));
    }

    public static bool CheckUNO(Player player, EnemyAI opponent)
    {
        return false;
    }
    
    public static string GetWinner(Player player, EnemyAI opponent)
    {
        if (player.CurrentHealth <= 0 && opponent.CurrentHealth <= 0)
            return "Draw";
        else if (player.CurrentHealth <= 0)
            return "AI Wins";
        else
            return "You Win!";
    }

    //public static void GameOver(Player player, EnemyAI opponent)
    //{
    //    //Delete all the cards in hand
    //    ClearHand();
    //
    //    if (GetWinner() == "You Win!")
    //    {
    //        int reward = Random.Range(victoryRewardMin, victoryRewardMax + 1);
    //        UpdateDisplay("You Win!", "Gain " + reward + " Money");
    //        Inventory.Instance.UpdateFunds(reward);
    //    }
    //    else
    //        UpdateDisplay("AI Wins");
    //}


    public static bool FindEvent(List<EventDictionary> OngoingEvents, Enums._Event SearchEvent, Enums.PlayerOption EffectedPlayer)
    {
        foreach (var _event in OngoingEvents)
        {
            if (_event.EventType == SearchEvent && _event.EventTarget == EffectedPlayer)
            {
                return true;
            }
        }
        return false;
    }
}
