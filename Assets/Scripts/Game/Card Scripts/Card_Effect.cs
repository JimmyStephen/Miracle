using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using static Enums;

[Serializable]
public class Card_Effect
{
    [Header("Card Effects"), Tooltip("Effects are (normally) single use and trigger when activated")]
    [SerializeField] Target baseCardEffectTarget = Target.NONE;
    [SerializeField] Trigger baseEffectTrigger = Trigger.NONE;
    [SerializeField] Effect baseCardEffectType = Effect.NONE;
    [SerializeField] Data EffectValue;

    //the actual effect values in case they got changed from the base in some way
    public Target effectTarget { get; private set; }
    public Trigger EffectTrigger { get; private set; }
    //public Effect CurrentEffect { get; private set; }
    private string OnPlayText;

    /// <summary>
    /// Initalizes the Effect so that it can be called later without issue
    /// </summary>
    public void _Init(string Text)
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

        EffectTrigger = baseEffectTrigger;
        OnPlayText = SetOnPlayText(Text);

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
        return OnPlayText;
    }

    private string SetOnPlayText(string text)
    {
        text = text.Replace("[target]", GetEnumAsString(effectTarget.ToString()));
        text = text.Replace("[trigger]",GetEnumAsString(EffectTrigger.ToString()));
        text = text.Replace("[type]",   GetEnumAsString(baseCardEffectType.ToString()));
        text = text.Replace("[damage]", (EffectValue.DamageValue.GetValue() != 0) ? EffectValue.DamageValue.GetValue().ToString() : "");
        text = text.Replace("[shield]", (EffectValue.ShieldValue.GetValue() != 0) ? EffectValue.ShieldValue.GetValue().ToString() : "");
        text = text.Replace("[heal]",   (EffectValue.HealValue  .GetValue() != 0) ? EffectValue.HealValue  .GetValue().ToString() : "");
        text = text.Replace("[unknown]", ReplaceUnknown());
        text = text.Replace("\\n", "\n");
        return text;
    }

    private string ReplaceUnknown()
    {
        string ret = "";
        if (EffectValue.DamageValue.GetValue() != 0) { ret += "\nDamage: " + EffectValue.DamageValue.GetValue().ToString(); }
        if (EffectValue.ShieldValue.GetValue() != 0) { ret += "\nShield: " + EffectValue.ShieldValue.GetValue().ToString(); }
        if (EffectValue.HealValue.GetValue() != 0)   { ret += "\nHeal: "   + EffectValue.HealValue.GetValue().ToString(); }
        return ret;
    }
}

