using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static Enums;

public static class GameplayEventManager
{
    //Event Updates and Validation
    /// <summary>
    /// Checks if the event exists in the current game
    /// </summary>
    /// <param name="OngoingEvents">The list of events to update</param>
    /// <param name="SearchEvent">What event to search for</param>
    /// <param name="EffectedPlayer">Who is effected by the event</param>
    /// <returns></returns>
    public static bool CheckForEvent(List<EventDictionary> OngoingEvents, Enums._Event SearchEvent, Enums.PlayerOption EffectedPlayer)
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
    /// <summary>
    /// Update all of the events to reduce duration, and then remove the event if the duration reaches 0
    /// </summary>
    /// <param name="OngoingEvents">The list of events to update</param>
    public static void UpdateEvents(GameplayManager gm)
    {
        List<EventDictionary> temp = new();
        gm.OngoingEvents.ForEach(evnt => {
            if (evnt.EventDuration - 1 >= 0)
                temp.Add(new(evnt.EventType, evnt.EventTarget, evnt.EventDuration - 1));;
        });
        gm.OngoingEvents = temp;
    }

    //Trigger Passive Card Effects/Events
    /// <summary>
    /// Checks to see if there are any cards in either deck that need to trigger before the start of the game, and triggers them
    /// </summary>
    /// <param name="GM">The GameplayManager that contains required variables</param>
    public static void CheckStartOfGameEffects(GameplayManager GM)
    {
        //Check each hand and trigger any end of turn effects
        GM.playerOneDeck.GetStartingDeck().ForEach(card => { card.PlayCard(GM.player, GM.opponent, GM, true, Trigger.START_OF_GAME); });
        GM.playerTwoDeck.GetStartingDeck().ForEach(card => { card.PlayCard(GM.player, GM.opponent, GM, false, Trigger.START_OF_GAME); });
        GM.player.TriggerStored(PlayerOption.PLAYER_ONE);
        GM.opponent.TriggerStored(PlayerOption.PLAYER_TWO);
    }
    /// <summary>
    /// Checks to see if there are any cards in either players hand that trigger at the end of turn, and triggers them
    /// </summary>
    /// <param name="GM">The GameplayManager that contains required variables</param>
    public static void CheckInHandEffects(GameplayManager GM)
    {
        //Check each hand and trigger any end of turn effects
        GM.player.Hand.ForEach(card => { card.PlayCard(GM.player, GM.opponent, GM, true, Trigger.IN_HAND); });
        GM.opponent.Hand.ForEach(card => { card.PlayCard(GM.player, GM.opponent, GM, false, Trigger.IN_HAND); });
    }
}
