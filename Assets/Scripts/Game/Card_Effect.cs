using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Enums;

public class Card_Effect : MonoBehaviour
{
    [Header("Card Effects"), Tooltip("Effects are (normally) single use and trigger when activated")]
    [SerializeField] Target baseCardEffectTarget = Target.NONE;
    [SerializeField] Trigger baseEffectTrigger = Trigger.NONE;
    [SerializeField] Effect baseCardEffectType = Effect.NONE;
    [SerializeField] Data EffectValue;

    //the actual effect values in case they got changed from the base in some way
    public Target effectTarget { get; private set; }
    public Trigger effectTrigger { get; private set; }

    /// <summary>
    /// Initalizes the Effect so that it can be called later without issue
    /// </summary>
    public void _Init()
    {
        //Set the base variables for the effect
        //set the effect target
        if (baseCardEffectTarget == Target.RANDOM)
        {
            //choose a random target
            int temp = UnityEngine.Random.Range(1, 4);
            effectTarget = temp switch
            {
                1 => Target.SELF,
                2 => Target.OPPONENT,
                _ => Target.BOTH
            };
        }
        else
            effectTarget = baseCardEffectTarget;

        effectTrigger = baseEffectTrigger;
    }
    public string TriggerEffect(Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer)
    {
        switch (effectTarget)
        {
            case Target.SELF:
                if (PlayedByPlayer)
                    player.UpdateStored(EffectValue);
                else
                    AI.UpdateStored(EffectValue);
                break;
            case Target.OPPONENT:
                if (PlayedByPlayer)
                    AI.UpdateStored(EffectValue);
                else
                    player.UpdateStored(EffectValue);
                break;
            case Target.BOTH:
                AI.UpdateStored(EffectValue);
                player.UpdateStored(EffectValue);
                break;
            default:
                throw new System.Exception("Effect Target was not Both, Self, or Opponent");
        }
        return "Effect Triggered";
    }
}

[Serializable]
public struct Data
{
    public int DamageValue;
    public int HealValue;
    public int ShieldValue;
    public int DrawNumber;
}

