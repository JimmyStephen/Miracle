using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

[Serializable]
public class Card_CardEffect
{
    [Header("Card Draw/Destruction Effects"), Tooltip("Effects effect the number of cards in play")]
    [SerializeField] CardEffectTarget baseCardTarget = CardEffectTarget.NONE;
    [SerializeField] Trigger baseEventTrigger = Trigger.NONE;
    [SerializeField, Tooltip("How many cards to draw, or discard")] DrawDiscard Value;

    public CardEffectTarget _eventTarget { get; private set; }
    public Trigger EffectTrigger { get; private set; }
    private string OnPlayText;


    /// <summary>
    /// Initalizes the Effect so that it can be called later without issue
    /// </summary>
    public void _Init(string Text)
    {
        EffectTrigger = baseEventTrigger;
        OnPlayText = Text;
    }

    public string TriggerEffect(Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer)
    {
        switch (baseCardTarget)
        {
            case CardEffectTarget.SELF_HAND:
                if (PlayedByPlayer)
                    player.HandEffect(Value);
                else
                    AI.HandEffect(Value);
                break;
            case CardEffectTarget.SELF_DECK:
                if (PlayedByPlayer)
                    player.DeckEffect(Value.ToDiscard);
                else
                    AI.DeckEffect(Value.ToDiscard);
                break;
            case CardEffectTarget.OPPONENT_HAND:
                if (PlayedByPlayer)
                    AI.HandEffect(Value);
                else
                    player.HandEffect(Value);
                break;
            case CardEffectTarget.OPPONENT_DECK:
                if (PlayedByPlayer)
                    player.DeckEffect(Value.ToDiscard);
                else
                    AI.DeckEffect(Value.ToDiscard);
                break;
            case CardEffectTarget.BOTH_HAND:
                player.HandEffect(Value);
                AI.HandEffect(Value);
                break;
            case CardEffectTarget.BOTH_DECK:
                player.DeckEffect(Value.ToDiscard);
                AI.DeckEffect(Value.ToDiscard);
                break;
            default:
                throw new System.Exception("Effect Target was not Both, Self, or Opponent");
        }
        return OnPlayText;
    }
}
