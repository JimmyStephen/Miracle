using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

[Serializable]
public class Card_SwapEffect
{
    [Header("Event Effects"), Tooltip("Events are duration based and will continue to effect the game until the duration runs out")]
    [SerializeField] Target SwapTarget = Target.NONE;
    [SerializeField] Trigger SwapTrigger = Trigger.NONE;

    //the actual event values, in case they got changed from the base in some way
    private string OnPlayText;
    public Trigger EffectTrigger { get; private set; }
    public string EffectType { get; private set; }
    public Target CardTarget { get; private set; }

    /// <summary>
    /// Initalizes the Effect so that it can be called later without issue
    /// </summary>
    public void _Init(string Text)
    {
        EffectTrigger = SwapTrigger;
        EffectType = "SWAP";
        CardTarget = SwapTarget;
        OnPlayText = Text;
    }

    /// <summary>
    /// Called if the cards effect should trigger
    /// </summary>
    public string TriggerEffect(Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer)
    {
        switch (SwapTarget)
        {
            case Target.SWAP_DECK:
                (AI.Deck, player.Deck) = (player.Deck, AI.Deck);
                break;
            case Target.SWAP_HAND:
                (AI.Hand, player.Hand) = (player.Hand, AI.Hand);
                break;
            case Target.SWAP_HEALTH:
                (AI.CurrentHealth, player.CurrentHealth) = (player.CurrentHealth, AI.CurrentHealth);
                break;
            default:
                throw new Exception("This should never be reached");
        }
        return OnPlayText;
    }
}
