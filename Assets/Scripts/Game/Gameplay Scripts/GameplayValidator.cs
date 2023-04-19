using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static Enums;

public static class GameplayValidator
{
    public static void CheckStartOfGameEffects(Deck playerOneDeck, Deck playerTwoDeck, Player player, EnemyAI opponent, GameplayManager GM)
    {
        //Check each hand and trigger any end of turn effects
        playerOneDeck.GetStartingDeck().ForEach(card => { card.PlayCard(player, opponent, GM, true, Trigger.START_OF_GAME); });
        playerTwoDeck.GetStartingDeck().ForEach(card => { card.PlayCard(player, opponent, GM, false, Trigger.START_OF_GAME); });
    }
    public static void CheckInHandEffects(Player player, EnemyAI opponent, GameplayManager GM)
    {
        //Check each hand and trigger any end of turn effects
        player.Hand.ForEach(card => { card.PlayCard(player, opponent, GM, true, Trigger.IN_HAND); });
        opponent.Hand.ForEach(card => { card.PlayCard(player, opponent, GM, false, Trigger.IN_HAND); });
    }
}
