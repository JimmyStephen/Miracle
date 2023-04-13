using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI
{
    private int CurrentHealth;
    private readonly int MaxHealth;
    private readonly Deck Deck;
    private List<Card> Hand;

    private int CurrentShield = 0;
    private int StoredDamage = 0;
    private int StoredHealing = 0;

    private GameplayManager GM;

    public EnemyAI(Deck deck, int maxHealth, GameplayManager gm)
    {
        CurrentHealth = maxHealth;
        MaxHealth = maxHealth;
        Deck = deck;
        Hand = deck.DrawStartingHand();
        GM = gm;
    }

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
        if(selection == -1) selection = Random.Range(0, Hand.Count-1);

        //save the card
        Card retCard = Hand[selection];

        //remove card from hand
        RemoveCard(retCard);

        //draw new card
        Hand.Add(Deck.DrawCard(GM));

        //return the selection
        return retCard;
    }
    public int GetHealth()
    {
        return CurrentHealth;
    }
    public string GetHealthDisplay()
    {
        string ret = CurrentHealth.ToString();
        if (CurrentShield > 0)
            ret += " (" + CurrentShield + ")";
        return ret;
    }
    public List<Card> GetHand()
    {
        return Hand;
    }

    public void UpdateStored(Data values)
    {
        StoredDamage += values.DamageValue;
        StoredHealing += values.HealValue;
        if (GM.CheckForEvent(Enums._Event.NO_SHIELDS) && values.ShieldValue != 0)
            CurrentShield = values.ShieldValue;
    }
    /// <summary>
    /// Updates the stored values that will be triggered at the end of the turn
    /// </summary>
    /// <param name="healValue">How much this character will heal</param>
    /// <param name="damageValue">How much damage this character will take</param>
    /// <param name="shieldValue">Set this characters shield value</param>
    public void UpdateStored(int healValue = 0, int damageValue = 0, int shieldValue = 0)
    {
        //add to heal
        StoredHealing += healValue;
        //add to damage
        StoredDamage += damageValue;
        //set the shield // If you already have a shield override it
        if (GM.CheckForEvent(Enums._Event.NO_SHIELDS))
            CurrentShield = shieldValue;
    }
    /// <summary>
    /// Called at the end of the turn to trigger any damage or healing that have been stored
    /// </summary>
    /// <returns>The current health value after the effects trigger</returns>
    public int TriggerStored()
    {
        //Debug.Log($"Trigger Stored Variabes (AI) [Health: {StoredHealing}, Damage: {StoredDamage}, Shield: {CurrentShield}]");
        //Trigger Healing
        if (GM.CheckForEvent(Enums._Event.NO_HEALS))
        {
            CurrentHealth += StoredHealing;
            if (CurrentHealth > MaxHealth)
                CurrentHealth = MaxHealth;
        }

        //Trigger Damage
        //Subtract damage from shield until the shield hits 0
        //Then subtract damage from health
        int postShieldDamage = CurrentShield - StoredDamage;
        if (postShieldDamage < 0)
        {
            CurrentHealth += postShieldDamage;
            CurrentShield = 0;
        }
        else
            CurrentShield = postShieldDamage;

        if (CurrentHealth < 0)
            CurrentHealth = 0;

        //Reset
        StoredDamage = 0;
        StoredHealing = 0;

        return CurrentHealth;
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


    /// <summary>
    /// Removes a card from your hand
    /// </summary>
    /// <param name="toRemove">The card to remove from your hand</param>
    /// <returns>If the removal was successful</returns>
    public bool RemoveCard(Card toRemove)
    {
        try
        {
            foreach (Card card in Hand)
            {
                if (card.index == toRemove.index)
                {
                    Hand.Remove(card);
                    Deck.AddToNewDeck(card);
                    return true;
                }
            }
        }
        catch
        {
            Debug.Log("Unable to remove card");
            return false;
        }
        return true;
    }
}
