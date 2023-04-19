using Sisus.ComponentNames;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] Card[] startingDeck;

    private List<Card> currentDeck;
    private List<Card> usedCards;

    public void Init()
    {
        currentDeck = new List<Card>();
        usedCards = new List<Card>();
//        int index = 0;
        foreach (var card in startingDeck)
        {
            //card.index = index++;
            card._Init();
            currentDeck.Add(card);
        }
        ShuffleDeck();
    }

    /// <summary>
    /// Draw a card from the deck, removing it from the deck
    /// </summary>
    /// <returns>The card that you got</returns>
    public Card DrawCard(GameplayManager GM)
    {
        if(currentDeck.Count == 0)
        {
            if(!GM.CheckForEvent(Enums._Event.LIMITED_DECK) && usedCards.Count != 0)
                NewDeck();
            else return null;
        }
        Card retCard = currentDeck[0];
        currentDeck.RemoveAt(0);
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

    public List<Card> DrawStartingHand()
    {
        List<Card> retCards = new();
        for(int i = 0; i < 5; i++)
        {
            retCards.Add(DrawCard(null));
        }
        return retCards;
    }
    public void AddToNewDeck(Card toAdd)
    {
        usedCards.Add(toAdd);
    }

    public List<Card> GetStartingDeck()
    {
        return startingDeck.ToList<Card>();
    }

    /// <summary>
    /// Shuffle the deck of cards
    /// </summary>
    void ShuffleDeck()
    {
        int n = currentDeck.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            (currentDeck[n], currentDeck[k]) = (currentDeck[k], currentDeck[n]);
        }
    }
    void NewDeck()
    {
        foreach(var card in usedCards)
        {
            currentDeck.Add(card);
        }
        usedCards.Clear();
        ShuffleDeck();
    }
}
