using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

[Serializable]
public class Card_CardEffect
{
    [Header("Card Draw/Destruction Effects"), Tooltip("Effects effect the number of cards in play")]
    [SerializeField] Target baseCardTarget = Target.NONE;
    [SerializeField] Trigger baseEventTrigger = Trigger.NONE;
    [SerializeField, Tooltip("How many cards to draw, or discard")] DrawDiscard Value;

    public Target CardTarget { get; private set; }
    public Trigger EffectTrigger { get; private set; }
    public string EffectType { get; private set; }
    private string OnPlayText;

    /// <summary>
    /// Initalizes the Effect so that it can be called later without issue
    /// </summary>
    public void _Init(string Text)
    {
        EffectTrigger = baseEventTrigger;
        EffectType = "DRAW";
        CardTarget = baseCardTarget;
        OnPlayText = Text;
    }

    public string TriggerEffect(Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer)
    {
        switch (baseCardTarget)
        {
            case Target.SELF_HAND:
                if (PlayedByPlayer)
                    player.HandEffect(Value);
                else
                    AI.HandEffect(Value);
                break;
            case Target.SELF_DECK:
                if (PlayedByPlayer)
                    player.DeckEffect(Value.ToDiscard);
                else
                    AI.DeckEffect(Value.ToDiscard);
                break;
            case Target.OPPONENT_HAND:
                if (PlayedByPlayer)
                    AI.HandEffect(Value);
                else
                    player.HandEffect(Value);
                break;
            case Target.OPPONENT_DECK:
                if (PlayedByPlayer)
                    player.DeckEffect(Value.ToDiscard);
                else
                    AI.DeckEffect(Value.ToDiscard);
                break;
            case Target.BOTH_HAND:
                player.HandEffect(Value);
                AI.HandEffect(Value);
                break;
            case Target.BOTH_DECK:
                player.DeckEffect(Value.ToDiscard);
                AI.DeckEffect(Value.ToDiscard);
                break;
            default:
                throw new Exception("Effect Target was not Both, Self, or Opponent");
        }
        return OnPlayText;
    }
}
