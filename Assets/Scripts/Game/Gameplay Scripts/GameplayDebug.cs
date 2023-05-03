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

    /// <summary>
    /// Output the remaining decks of the player, and opponent to the console
    /// </summary>    
    public static void OutputRemainingDecks(Player player, EnemyAI opponent)
    {
        string pStr = $"Player Remaining Deck:";
        player.Deck.CurrentDeck.ForEach(card => { pStr += card.name + ", "; });
        string oStr = $"Opponent Remaining Deck:";
        opponent.Deck.CurrentDeck.ForEach(card => { oStr += card.name + ", "; });
        Debug.Log($"{pStr}\n{oStr}");
    }

    /// <summary>
    /// Output the dead decks of the player, and opponent to the console
    /// </summary>    
    public static void OutputUsedDecks(Player player, EnemyAI opponent)
    {
        string pStr = $"Player Dead Deck:";
        player.Deck.UsedCards.ForEach(card => { pStr += card.name + ", "; });
        string oStr = $"Opponent Dead Deck:";
        opponent.Deck.UsedCards.ForEach(card => { oStr += card.name + ", "; });
        Debug.Log($"{pStr}\n{oStr}");
    }

    /// <summary>
    /// Output a list of all the events currently effecting the game
    /// </summary>
    /// <param name="events"></param>
    public static void OutputEvents(List<EventDictionary> events)
    {
        Debug.Log("Events: ");
        events.ForEach(e => { Debug.Log($"Event: {Enums.GetEnumAsString(e.EventType.ToString())}, Target: {Enums.GetEnumAsString(e.EventTarget.ToString())}, Duration: {e.EventDuration}"); });
    }

    public static void GiveCardsID(GameObject[] AllCards)
    {
        //Give cards ID's
        int id = 0;
        foreach (var v in AllCards)
        {
            v.GetComponent<Gameplay_Card>().CardID = id++;
        }
    }
}
