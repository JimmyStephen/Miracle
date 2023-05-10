using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deckbuilder : MonoBehaviour
{
    List<int> CardID = new();

    /// <summary>
    /// Attempts to add a card to the current list
    /// </summary>
    /// <param name="ToAdd">The card to add to the list</param>
    public void AddToList(int ToAdd)
    {
        if (CanAddToList(ToAdd))
        {
            CardID.Add(ToAdd);
            OutputText.SetText($"Deck Output\n{ToAdd} Added");
        }
        else
        {
            OutputText.SetText($"Deck Output\n{ToAdd} Not Added");
        }
    }
    /// <summary>
    /// Makes sure the input can be added to the list
    /// </summary>
    /// <param name="ToAdd">The number being attempted to add</param>
    /// <returns>If it can be added</returns>
    private bool CanAddToList(int ToAdd)
    {
        try
        {
            CardConnector.GetGameplayCard(ToAdd);
            int InDeck = 0;
            CardID.ForEach(id =>
            {
                if (id == ToAdd)
                {
                    InDeck++;
                }
            });
            return InDeck < 2 && CardID.Count < 15;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Makes sure the deck can be built
    /// </summary>
    /// <returns>If the deck is a valid deck</returns>
    public bool CanBuild()
    {
        return CardID.Count >= 5 && CardID.Count < 16;
    }
    /// <summary>
    /// Build a deck, as long as it is valid
    /// </summary>
    /// <returns>A valid deck</returns>
    public void BuildDeck()
    {
        if (!CanBuild())
            throw new System.Exception("Deck cannot be built");

        List<Gameplay_Card> InitialDeck = new();
        CardID.ForEach(id =>
        {
            InitialDeck.Add(CardConnector.GetGameplayCard(id));
        });
        OutputDeck(InitialDeck);
        RuntimeDeck RD = new RuntimeDeck(InitialDeck.ToArray());
        RD.Init();
        GameManager.Instance.CustomDeck = RD;
    }


    //Debug and Testing
    [SerializeField] TMPro.TMP_Text OutputText;
    private void OutputDeck(List<Gameplay_Card> Cards)
    {
        string output = "Deck Output\n";
        Cards.ForEach(card =>
        {
            output += $"{card.GetCardName()} : {card.GetCardID()}\n";
        });
        OutputText.SetText(output);
    }

    public void OutputGMDeck()
    {
        string output = "Deck Output\n";
        GameManager.Instance.CustomDeck.GetStartingDeck().ForEach(card =>
        {
            output += $"{card.GetCardName()} : {card.GetCardID()}\n";
        });
        OutputText.SetText(output);
    }

    public void DrawAndDisplayStartingHand()
    {
        string output = "Deck Output\n";
        GameManager.Instance.CustomDeck.DrawStartingHand().ForEach(card =>
        {
            output += $"{card.GetCardName()} : {card.GetCardID()}\n";
        });
        OutputText.SetText(output);
    }
}
