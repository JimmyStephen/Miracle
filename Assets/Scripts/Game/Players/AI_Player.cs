using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Player : Player
{
    public AI_Player(Deck deck, int maxHealth, GameplayManager gM) : base(deck, maxHealth, gM) { }

    public Card Play()
    {
        int selection;

        //check if healing is needed
        if (CheckShouldHeal())
        {
            //find a healing card
            selection = FindHealCard();
            //if no healing was found, find a shield instead
            if (selection == -1)
            {
                selection = FindShieldCard();
            }
        }
        else
        {
            //choose a random non healing card
            selection = FindNonHealCard();
        }

        //if nothing was found, choose randomly
        if (selection == -1) selection = Random.Range(0, Hand.Count - 1);

        //save the card
        Card retCard = Hand[selection];

        //remove card from hand
        RemoveCard(retCard);


        //return the selection
        return retCard;
    }
    private bool CheckShouldHeal()
    {
        int missingHealth = (MaxHealth - CurrentHealth);
        float rand = Random.Range(0, 10.0f);
        return missingHealth >= rand;
    }
    private int FindHealCard()
    {
        // int retVal = -1;
        // List<int> healingCards = new();
        //
        // for(int i = 0; i < Hand.Count; i++)
        // {
        //     if (Hand[i].GetCardType().Equals("HEAL"))
        //     {
        //         healingCards.Add(i);
        //     }
        // }
        // int neededHealing = MaxHealth - CurrentHealth;
        // int currentOver = 100;
        // foreach (var v in healingCards)
        // {
        //
        //     int over = neededHealing - Hand[v].GetValue();
        //     if(over < currentOver)
        //     {
        //         currentOver = over;
        //         retVal = v;
        //     }
        // }
        //
        // return retVal;
        return 0;
    }
    private int FindNonHealCard()
    {
        // int retVal = -1;
        // List<int> retCards = new();
        //
        // for (int i = 0; i < Hand.Count; i++)
        // {
        //     if (!Hand[i].GetCardType().Equals("HEAL"))
        //     {
        //         retCards.Add(i);
        //     }
        // }
        //
        // if (retCards.Count > 0)
        // {
        //     int retIndex = Random.Range(0, retCards.Count);
        //     retVal = retCards[retIndex];
        // }
        //
        // return retVal;
        return 0;
    }
    private int FindShieldCard()
    {
        //int retVal = -1;
        //List<int> healingCards = new();
        //
        //for (int i = 0; i < Hand.Count; i++)
        //{
        //    if (Hand[i].GetCardType().Equals("SHIELD"))
        //    {
        //        healingCards.Add(i);
        //    }
        //}
        //
        //int currentMax = -1;
        //foreach (var v in healingCards)
        //{
        //    if (currentMax < Hand[v].GetValue())
        //    {
        //        currentMax = Hand[v].GetValue();
        //        retVal = v;
        //    }
        //}

        //return retVal;
        return 0;
    }
}
