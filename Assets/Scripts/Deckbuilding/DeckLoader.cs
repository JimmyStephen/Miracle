using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DeckLoader
{
    public static Deck LoadDeck(string DeckName)
    {
        CustomDeck cd = Inventory.Instance.GetCustomDeck(DeckName);
        Deck customDeck = new();
        customDeck.SetStartingDeck(GetCards(cd));
        return customDeck;
    }

    private static Gameplay_Card[] GetCards(CustomDeck cd)
    {
        List<Gameplay_Card> retCards = new();
        cd.Cards.ForEach(cardID =>
        {
            retCards.Add(CardConnector.GetGameplayCard(cardID));
        });
        return retCards.ToArray();
    }
}
