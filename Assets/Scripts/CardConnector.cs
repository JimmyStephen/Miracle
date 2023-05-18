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

        for(int i = 0; i < GameplayCards.Length; i++)
        {
            GameplayCards[i].CardID = i+1;
            try { GetGatchaCard(GameplayCards[i].GetCardName(), true).CardID = i+1; }
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
            if (card.GetCardName().Equals(name))
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
            if (card.GetCardID() == ID)
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
                if (card.GetConnectionName().ToLower().Replace(" ", "").Equals(name.ToLower().Replace(" ", "")))
                    return card;
            }
            else
            {
                if (card.GetCardName().ToLower().Replace(" ", "").Equals(name.ToLower().Replace(" ", "")))
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

    /// <summary>
    /// Gets the GameplayCard's GameObject based on the GameplayCard script
    /// </summary>
    /// <param name="gameplayCard">The script you are looking for</param>
    /// <returns></returns>
    public static GameObject GetGameplayCardObj(Gameplay_Card gameplayCard)
    {
        var CardsObj = GameManager.Instance.GetGameplayCards_GameObjects();
        foreach (var cardObj in CardsObj)
        {
            if(cardObj.GetComponent<Gameplay_Card>().GetCardID() == gameplayCard.GetCardID())
                return cardObj;
        }
        throw new System.Exception("Card not found, this shouldn't happen");
    }
    /// <summary>
    /// Gets the GameplayCard's GameObject based on the GameplayCard script
    /// </summary>
    /// <param name="gameplayCard">The script you are looking for</param>
    /// <returns></returns>
    public static GameObject GetGameplayCardObj(int CardID)
    {
        var CardsObj = GameManager.Instance.GetGameplayCards_GameObjects();
        foreach (var cardObj in CardsObj)
        {
            if (cardObj.GetComponent<Gameplay_Card>().GetCardID() == CardID)
                return cardObj;
        }
        throw new System.Exception("Card not found, this shouldn't happen");
    }
}
