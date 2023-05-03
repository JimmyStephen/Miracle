using Sisus.ComponentNames;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] Gameplay_Card[] startingDeck;

    public List<Gameplay_Card> CurrentDeck { get; private set; }
    public List<Gameplay_Card> UsedCards { get; private set; }

    public void Init()
    {
        CurrentDeck = new List<Gameplay_Card>();
        UsedCards = new List<Gameplay_Card>();
//        int index = 0;
        foreach (var card in startingDeck)
        {
            //card.index = index++;
            card._Init();
            CurrentDeck.Add(card);
        }
        ShuffleDeck();
    }

    /// <summary>
    /// Draw a card from the deck, removing it from the deck
    /// </summary>
    /// <returns>The card that you got</returns>
    public Gameplay_Card DrawCard(GameplayManager GM)
    {
        if(CurrentDeck.Count == 0)
        {
            if(!GameplayEventManager.CheckForEvent(GM.OngoingEvents, Enums._Event.LIMITED_DECK, Enums.PlayerOption.BOTH) && UsedCards.Count != 0)
                NewDeck();
            else return null;
        }
        Gameplay_Card retCard = CurrentDeck[0];
        CurrentDeck.RemoveAt(0);
        return retCard;
    }
    
    /// <summary>
    /// Discard a number of cards from the top of the deck based on the int input
    /// </summary>
    /// <param name="ToDiscard">How many cards to discard</param>
    /// <param name="GM">The gameplay manager</param>
    public void DiscardCards(int ToDiscard, GameplayManager GM)
    {
        for (int i = 0; i < ToDiscard; i++)
        {
            AddToNewDeck(DrawCard(GM));
        }
    }

    public List<Gameplay_Card> DrawStartingHand()
    {
        List<Gameplay_Card> retCards = new();
        for(int i = 0; i < 5; i++)
        {
            retCards.Add(DrawCard(null));
        }
        return retCards;
    }
    public void AddToNewDeck(Gameplay_Card toAdd)
    {
        UsedCards.Add(toAdd);
    }

    public List<Gameplay_Card> GetStartingDeck()
    {
        return startingDeck.ToList<Gameplay_Card>();
    }

    /// <summary>
    /// Shuffle the deck of cards
    /// </summary>
    void ShuffleDeck()
    {
        int n = CurrentDeck.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            (CurrentDeck[n], CurrentDeck[k]) = (CurrentDeck[k], CurrentDeck[n]);
        }
    }
    void NewDeck()
    {
        foreach(var card in UsedCards)
        {
            CurrentDeck.Add(card);
        }
        UsedCards.Clear();
        ShuffleDeck();
    }
}
