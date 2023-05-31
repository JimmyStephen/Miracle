using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HealRule : Rule
{
    public HealRule(string name, string desc, Player player, EnemyAI AI) : base(name, desc, player, AI) { }

    public override int CheckRule()
    {
        if (AI.CurrentHealth == AI.MaxHealth)
            return -1;

        bool healCard = false;
        AI.Hand.ForEach(card => {
            if (card.CardEffectType().Item1 == "HEAL")
                healCard = true;
        });

        if (!healCard)
            return -1;

        float missingHealth = AI.MaxHealth - AI.CurrentHealth;
        return (int)MathF.Truncate((missingHealth / AI.MaxHealth) * 100f) + 5;
    }

    public override Gameplay_Card RunRule()
    {
        //find each heal card
            //give each card a number value:
                //the value is = to the % of the card used to heal...
                //if the AI is missing 5 hp and it has cards that can heal between 2-7
                //the values assigned will be
                    //2: 30%
                    //3: 60%
                    //4: 80%
                    //5: 100%
                    //6: 120%
                    //7: 140%
                //The card selected will be 5 because it is the closest to 100%. If two numbers are equal then it will randomly select one to use
        Gameplay_Card retCard = null;
        float currentPercent = 0;
        int missingHealth = AI.MaxHealth - AI.CurrentHealth;
        AI.Hand.ForEach(card => {
            if (card.CardEffectType().Item1 == "HEAL")
            {
                float tempPercentage = (((missingHealth + AI_CardHelper.GetCardEffectValue(card, Enums.Effect.HEAL)) / (float)AI.MaxHealth) * 100f);
                (retCard, currentPercent) = CloserDistanceTo100(currentPercent, tempPercentage) ? (retCard, currentPercent) : (card, tempPercentage);
            }
        });

        return retCard;
    }

    public bool CloserDistanceTo100(float original, float newNum)
    {
        return Math.Abs(original - 100) < Math.Abs(newNum - 100);
    }
}
