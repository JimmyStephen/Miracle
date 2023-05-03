using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Enums;

static class AI_CardHelper
{
    public static (string, Target) CardEffectType(Gameplay_Card card)
    {
        foreach (var e in card.GetEffects()) { return (e.EffectType, e.CardTarget); }
        foreach (var e in card.GetEvents()) { return (e.EffectType, e.CardTarget); }
        foreach (var e in card.GetCardEffects()) { return (e.EffectType, Target.NONE); }
        foreach (var e in card.GetSwapEffects()) { return (e.EffectType, Target.NONE); }
        return ("Unknown", Target.NONE);
    }

    public static int GetCardEffectValue(Gameplay_Card card, Effect effect)
    {
        foreach (var e in card.GetEffects()) 
        {
            switch (effect)
            {
                case Effect.DAMAGE:
                    return e.GetEffectValue().DamageValue.GetValue();
                case Effect.SHIELD:
                    return e.GetEffectValue().ShieldValue.GetValue();
                case Effect.HEAL:
                    return e.GetEffectValue().HealValue.GetValue();
            }
        }
        return -1;
    }
}
