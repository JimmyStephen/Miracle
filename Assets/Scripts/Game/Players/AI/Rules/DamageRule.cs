using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DamageRule : Rule
{
    public DamageRule(string name, string desc, Player player, EnemyAI AI) : base(name, desc, player, AI) { }

    public override int CheckRule()
    {
        int maxDamage = 0;
        foreach(Gameplay_Card c in AI.Hand)
        {
            int newDamage = AI_CardHelper.GetCardEffectValue(c, Enums.Effect.DAMAGE);
            maxDamage = newDamage > maxDamage ? newDamage : maxDamage;
        }
        float healthAfterDamage = player.CurrentHealth - maxDamage;
        if (healthAfterDamage <= 0) return 100;
        return 100 - (int)MathF.Floor((healthAfterDamage / player.MaxHealth) * 100f);
    }

    public override Gameplay_Card RunRule()
    {
        Gameplay_Card retCard = null;
        int maxDamage = -1;
        AI.Hand.ForEach(card =>
        {
            int newDamage = AI_CardHelper.GetCardEffectValue(card, Enums.Effect.DAMAGE);
            (maxDamage, retCard) = newDamage > maxDamage ? (newDamage, card) : (maxDamage, retCard);
        });
        return retCard;
    }
}

