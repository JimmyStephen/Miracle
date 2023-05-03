using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ShieldRule : Rule
{
    public ShieldRule(string name, string desc, Player player, EnemyAI AI) : base(name, desc, player, AI) { }

    public override int CheckRule()
    {
        bool validShieldCard = false;
        AI.Hand.ForEach(card => {
            if (card.CardEffectType().Item1 == "SHIELD" && AI_CardHelper.GetCardEffectValue(card, Enums.Effect.SHIELD) > AI.CurrentShield)
                validShieldCard = true;
        });

        if (!validShieldCard)
            return -1;

        float missingHealth = AI.MaxHealth - AI.CurrentHealth;
        return (int)MathF.Truncate((missingHealth / AI.MaxHealth) * 100f);
    }

    public override Gameplay_Card RunRule()
    {
        Gameplay_Card retCard = null;
        int maxShield = -1;
        AI.Hand.ForEach(card =>
        {
            int newDamage = AI_CardHelper.GetCardEffectValue(card, Enums.Effect.SHIELD);
            (maxShield, retCard) = newDamage > maxShield ? (newDamage, card) : (maxShield, retCard);
        });
        return retCard;
    }
}
