using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static Enums;

public static class GameplayValidator
{
    //Winner Validation
    /// <summary>
    /// Check to see if someone won the game
    /// </summary>
    /// <param name="OngoingEvents">The current effects effecting the game</param>
    /// <param name="player">Player One</param>
    /// <param name="opponent">Player Two</param>
    /// <returns></returns>
    public static bool CheckWinner(List<EventDictionary> OngoingEvents, Player player, EnemyAI opponent)
    {
        return (player.CurrentHealth <= 0 || opponent.CurrentHealth <= 0 || CheckUNO(OngoingEvents, player, opponent));
    }
    /// <summary>
    /// Check to see if there is a winner by Un-Oh
    /// </summary>
    /// <param name="OngoingEvents">The current effects effecting the game</param>
    /// <param name="player">Player One</param>
    /// <param name="opponent">Player Two</param>
    /// <returns></returns>
    public static bool CheckUNO(List<EventDictionary> OngoingEvents, Player player, EnemyAI opponent)
    {
        if (GameplayEventManager.CheckForEvent(OngoingEvents, _Event.UN_OH, PlayerOption.BOTH))
            return (player.Hand.Count == 0 || opponent.Hand.Count == 0);
        return false;
    }
    /// <summary>
    /// Get the winner of the game, if there is a winner
    /// </summary>
    /// <param name="OngoingEvents">The current effects effecting the game</param>
    /// <param name="player">Player One</param>
    /// <param name="opponent">Player Two</param>
    /// <returns></returns>
    public static string GetWinner(List<EventDictionary> OngoingEvents, Player player, EnemyAI opponent)
    {
        if ((player.CurrentHealth <= 0 && opponent.CurrentHealth <= 0) || (CheckUNO(OngoingEvents, player, opponent) && player.Hand.Count == 0 && opponent.Hand.Count == 0))
            return "Draw";
        else if (player.CurrentHealth <= 0 || (CheckUNO(OngoingEvents, player, opponent) && opponent.Hand.Count == 0))
            return "AI Wins";
        else
            return "You Win!";
    }
}
