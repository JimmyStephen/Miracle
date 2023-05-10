using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CardConnector
{
    /// <summary>
    /// Initializes all the Card ID's
    /// </summary>
    public static void InitCardID()
    {
        var GameplayCards = GameManager.Instance.GetGameplayCards_Cards();

        for(int i = 1; i < GameplayCards.Length; i++)
        {
            GameplayCards[i].CardID = i;
            try { GetGatchaCard(GameplayCards[i].GetCardName(), true).CardID = i; }
            catch { Debug.Log($"No Gatcha Card \"{GameplayCards[i].GetCardName()}\" Found"); }
        }
    }

    /// <summary>
    /// Gets the Gameplay card based on its name
    /// </summary>
    /// <param name="ID">The id to find</param>
    /// <returns>The card</returns>
    public static Gameplay_Card GetGameplayCard(int ID)
    {
        var cards = GameManager.Instance.GetGameplayCards_Cards();
        foreach(Gameplay_Card card in cards)
        {
            if(card.CardID == ID)
                return card;
        }
        throw new System.Exception("Card not found");
    }
    /// <summary>
    /// Gets the Gameplay card based on its name
    /// </summary>
    /// <param name="name">The name to find</param>
    /// <returns>The card</returns>
    public static Gameplay_Card GetGameplayCard(string name)
    {
        var cards = GameManager.Instance.GetGameplayCards_Cards();
        foreach (Gameplay_Card card in cards)
        {
            if (card.GetCardName() == name)
                return card;
        }
        throw new System.Exception("Card not found");
    }
    /// <summary>
    /// Gets the Gameplay card based on the Gatcha card its connected to
    /// </summary>
    /// <param name="gatchaCard">The Gatcha card its connected to to find</param>
    /// <returns>The card</returns>
    public static Gameplay_Card GetGameplayCard(GatchaCard gatchaCard)
    {
        var cards = GameManager.Instance.GetGameplayCards_Cards();
        foreach (Gameplay_Card card in cards)
        {
            if (card.CardID == gatchaCard.CardID)
                return card;
        }
        throw new System.Exception("Card not found");
    }

    /// <summary>
    /// Gets the Gatcha card based on its name
    /// </summary>
    /// <param name="ID">The id to find</param>
    /// <returns>The card</returns>
    public static GatchaCard GetGatchaCard(int ID)
    {
        var cards = GameManager.Instance.GetGatchaCards_Cards();
        foreach (GatchaCard card in cards)
        {
            if (card.CardID == ID)
                return card;
        }
        throw new System.Exception("Card not found");
    }
    /// <summary>
    /// Gets the Gatcha card based on its name
    /// </summary>
    /// <param name="name">The name to look for</param>
    /// <param name="ConnectionName">If you looking for the card based on its connection name</param>
    /// <returns>The card</returns>
    public static GatchaCard GetGatchaCard(string name, bool ConnectionName)
    {
        var cards = GameManager.Instance.GetGatchaCards_Cards();
        foreach (GatchaCard card in cards)
        {
            if (ConnectionName)
            {
                if (card.GetConnectionName() == name)
                    return card;
            }
            else
            {
                if (card.GetCardName() == name)
                    return card;
            }
           
        }
        throw new System.Exception("Card not found");
    }
    /// <summary>
    /// Gets the Gatcha card based on the gameplay card its connected to
    /// </summary>
    /// <param name="gameplayCard">The gameplay card connection</param>
    /// <returns>The card</returns>
    public static GatchaCard GetGatchaCard(Gameplay_Card gameplayCard)
    {
        var cards = GameManager.Instance.GetGatchaCards_Cards();
        foreach (GatchaCard card in cards)
        {
            if (card.CardID == gameplayCard.CardID)
                return card;
        }
        throw new System.Exception("Card not found");
    }
}
