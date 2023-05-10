using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Enums;

static class AI_CardHelper
{
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
