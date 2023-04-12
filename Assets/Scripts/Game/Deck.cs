using Sisus.ComponentNames;
using System.Collections;
using System.Collections.Generic;
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
        int index = 0;
        foreach (var card in startingDeck)
        {
            card.index = index++;
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
            if(GM.EnabledDraw())
                NewDeck();
            else return null;
        }
        Card retCard = currentDeck[0];
        currentDeck.RemoveAt(0);
        return retCard;
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

/*    public string GetCurrentDeck()
    {
        string result = "";
        foreach (var v in currentDeck)
        {
            result += v.GetName() + " ";
        }
        return result;
    }*/
/*    public string GetStartingDeck()
    {
        string result = "";
        foreach(var v in startingDeck)
        {
            result += v.GetName() + " ";
        }
        return result;
    }*/

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
