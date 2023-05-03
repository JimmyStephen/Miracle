using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CardConnector
{
    public static void InitCardID()
    {
        var GameplayCards = GameManager.Instance.GetGameplayCards_Cards();
        var GatchaCards   = GameManager.Instance.GetGatchaCards_Cards();

        for(int i = 1; i < GameplayCards.Length + 1; i++)
        {
            GameplayCards[i].CardID = i;
            GatchaCards[i].CardID = i;
        }
    }

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
