using System;
using UnityEngine;
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
    public Target CardTarget { get; private set; }
    public Trigger EffectTrigger { get; private set; }
    public string EffectType { get; private set; }
    private string OnPlayText;

    /// <summary>
    /// Initalizes the Effect so that it can be called later without issue
    /// </summary>
    public void _Init(string Text)
    {
        //Set the base variables for the effect
        //set the effect target
        if (baseCardEffectTarget == Target.RANDOM_HEALTH)
        {
            //choose a random target
            int temp = UnityEngine.Random.Range(1, 4);
            CardTarget = temp switch
            {
                1 => Target.SELF_HEALTH,
                2 => Target.OPPONENT_HEALTH,
                _ => Target.BOTH_HEALTH
            };
        }
        else
            CardTarget = baseCardEffectTarget;

        EffectTrigger = baseEffectTrigger;
        EffectType = baseCardEffectType.ToString();
        OnPlayText = SetOnPlayText(Text);

    }
    public string TriggerEffect(Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer)
    {
        switch (CardTarget)
        {
            case Target.SELF_HEALTH:
                if (PlayedByPlayer)
                    player.UpdateStored(EffectValue, PlayerOption.PLAYER_ONE);
                else
                    AI.UpdateStored(EffectValue, PlayerOption.PLAYER_TWO);
                break;
            case Target.OPPONENT_HEALTH:
                if (PlayedByPlayer)
                    AI.UpdateStored(EffectValue, PlayerOption.PLAYER_TWO);
                else
                    player.UpdateStored(EffectValue, PlayerOption.PLAYER_ONE);
                break;
            case Target.BOTH_HEALTH:
                AI.UpdateStored(EffectValue, PlayerOption.PLAYER_TWO);
                player.UpdateStored(EffectValue, PlayerOption.PLAYER_ONE);
                break;
            default:
                throw new System.Exception("Effect Target was not Both, Self, or Opponent");
        }
        return OnPlayText;
    }

    private string SetOnPlayText(string text)
    {
        text = text.Replace("[target]", GetEnumAsString(CardTarget.ToString()));
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
    public Data GetEffectValue() { return EffectValue; }
}

