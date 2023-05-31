using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OpponentDrawRule : Rule
{
    public OpponentDrawRule(string name, string desc, Player player, EnemyAI AI) : base(name, desc, player, AI) { }

    public override int CheckRule()
    {
        if (GameplayEventManager.CheckForEvent(AI.GM.OngoingEvents, Enums._Event.LIMITED_DECK, Enums.PlayerOption.BOTH))
        {
            foreach(Gameplay_Card card in AI.Hand)
            {
                var CardData = card.CardEffectType();
                if(CardData.Item1 == "DRAW" && CardData.Item2 == Enums.Target.OPPONENT_HAND)
                {
                    //Return a random number between 45-55 if this is a valid play
                    return UnityEngine.Random.Range(45, 56);
                }
            }
        }
        //If It isnt Limited Deck || There are no cards that draw or your opponent, return 0
        return 0;
    }

    public override Gameplay_Card RunRule()
    {
        foreach (Gameplay_Card card in AI.Hand)
        {
            var CardData = card.CardEffectType();
            if (CardData.Item1 == "DRAW" && CardData.Item2 == Enums.Target.OPPONENT_HAND)
            {
                //Return the first card that draws for your opponent
                return card;
            }
        }
        return null;
    }
}

