using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Player
{
    private int CurrentHealth;
    private readonly int MaxHealth;
    private readonly Deck Deck;
    private List<Card> Hand;

    private int CurrentShield = 0;
    private int StoredDamage = 0;
    private int StoredHealing = 0;

    private GameplayManager GM;

    public Player(Deck deck, int maxHealth, GameplayManager gM)
    {
        CurrentHealth = maxHealth;
        MaxHealth = maxHealth;
        Deck = deck;
        Hand = deck.DrawStartingHand();
        GM = gM;
    }

    public void UpdateStored(Data values)
    {
        StoredDamage += values.DamageValue;
        StoredHealing += values.HealValue;
        if (GM.EnabledShield() && values.ShieldValue != 0)
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
        if (GM.EnabledShield())
            CurrentShield = shieldValue;
    }
    /// <summary>
    /// Called at the end of the turn to trigger any damage or healing that have been stored
    /// </summary>
    /// <returns>The current health value after the effects trigger</returns>
    public int TriggerStored()
    {
        //Debug.Log($"Trigger Stored Variabes (Player) [Health: {StoredHealing}, Damage: {StoredDamage}, Shield: {CurrentShield}]");
        //Trigger Healing
        if (GM.EnabledHeal())
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
    /// Return the hand
    /// </summary>
    /// <returns></returns>
    public List<Card> GetHand()
    {
        return Hand;
    }
    /// <summary>
    /// Returns the current health of the player
    /// </summary>
    /// <returns></returns>
    public int GetHealth()
    {
        return CurrentHealth;
    }
    /// <summary>
    /// Get a string value to display your health, and shield if you have one
    /// </summary>
    /// <returns></returns>
    public string GetHealthDisplay()
    {
        string ret = CurrentHealth.ToString();
        if(CurrentShield > 0)
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
                if(card.index == toRemove.index)
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
