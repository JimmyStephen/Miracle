using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Player
{
    public int MaxHealth {get; private set;}
    public Deck Deck { get; private set; }
    public List<Card> Hand { get; private set; }
    public int CurrentHealth { get; private set; }
    public GameplayManager GM { get; private set; }

    private int CurrentShield = 0;
    private int StoredDamage = 0;
    private int StoredHealing = 0;


    public Player(Deck deck, int maxHealth, GameplayManager gM)
    {
        CurrentHealth = maxHealth;
        MaxHealth = maxHealth;
        Deck = deck;
        Hand = deck.DrawStartingHand();
        GM = gM;
    }

    //Move to Abstract_Player
    /// <summary>
    /// Updates the stored values that will be triggered at the end of the turn
    /// </summary>
    /// <param name="values">A struct that holds the values that will be changed</param>
    public void UpdateStored(Data values)
    {
        StoredDamage += values.DamageValue;
        StoredHealing += values.HealValue;
        if (GM.CheckForEvent(Enums._Event.NO_SHIELDS) && values.ShieldValue != 0)
            CurrentShield = values.ShieldValue;
    }
    /// <summary>
    /// Called at the end of the turn to trigger any damage or healing that have been stored
    /// </summary>
    /// <returns>The current health value after the effects trigger</returns>
    public int TriggerStored()
    {
        //Debug.Log($"Trigger Stored Variabes (Player) [Health: {StoredHealing}, Damage: {StoredDamage}, Shield: {CurrentShield}]");
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
    /// <summary>
    /// Get a string value to display your health, and shield if you have one
    /// </summary>
    /// <returns></returns>
    public string GetHealthDisplay()
    {
        string ret = CurrentHealth.ToString();
        if (CurrentShield > 0)
            ret += " (" + CurrentShield + ")";
        return ret;
    }
    /// <summary>
    /// Draw a card to the hand
    /// </summary>
    public void Draw()
    {
        Card card = Deck.DrawCard(GM);
        if (card == null)
        {
            CurrentHealth -= 1;
            Debug.Log("Deck Empty! You start to feel tipsy");
        }
        else
        {
            Hand.Add(card);
        }
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
                if (card.CardID == toRemove.CardID)
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
