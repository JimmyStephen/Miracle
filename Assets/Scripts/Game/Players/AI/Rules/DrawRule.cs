using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DrawRule : Rule
{
    public DrawRule(string name, string desc, Player player, EnemyAI AI) : base(name, desc, player, AI) { } 

    public override int CheckRule()
    {
        //Return 0 if there are no valid cards or if the hand size is already => 5
        if(AI.Hand.Count >= 5) return 0;
        foreach (Gameplay_Card card in AI.Hand)
        {
            var CardData = card.CardEffectType();
            if (CardData.Item1 == "Draw" && (CardData.Item2 == Enums.Target.SELF_HAND || CardData.Item2 == Enums.Target.BOTH_HAND))
            {
                //Return a number scaling the smaller the current hand size is
                return 100 - (AI.Hand.Count * 20);
            }
        }
        return 0;
    }

    public override Gameplay_Card RunRule()
    {
        foreach (Gameplay_Card card in AI.Hand)
        {
            var CardData = card.CardEffectType();
            if (CardData.Item1 == "Draw" && (CardData.Item2 == Enums.Target.SELF_HAND || CardData.Item2 == Enums.Target.BOTH_HAND))
            {
                return card;
            }
        }
        return null;
    }
}
