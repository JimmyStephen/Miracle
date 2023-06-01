using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deckbuilder : MonoBehaviour
{
    List<int> CardID = new();
    List<GameObject> DisplayObjects = new();
    [SerializeField] TMPro.TMP_InputField NameInput;
    [SerializeField, Tooltip("The Prefab to be displayed")] GameObject Display;
    [SerializeField, Tooltip("Where the prefab will be displayed")] GameObject DisplayParent;

    public void FromBaseDeck(CustomDeck InitialDeck)
    {
        ClearDisplay();
        //If InitalDeck is null, make a new deck instead
        if (InitialDeck == null) return;
        //Draw the new data
        NameInput.text = InitialDeck.DeckName;
        InitialDeck.Cards.ForEach(id => AddToList(id));
    }
    public void ClearDisplay()
    {
        NameInput.text = "";
        CardID.Clear();
        DisplayObjects.ForEach(obj => Destroy(obj));
        DisplayObjects.Clear();
    }

    /// <summary>
    /// Attempts to add a card to the current list
    /// </summary>
    /// <param name="ToAdd">The card to add to the list</param>
    public void AddToList(int ToAdd)
    {
        if (CanAddToList(ToAdd))
        {
            CardID.Add(ToAdd);
            var temp = Instantiate(Display, DisplayParent.transform);
            DisplayObjects.Add(temp);
            temp.GetComponent<InDeckDisplay>().Init(GetCardName(ToAdd), GetNumInDeck(ToAdd), ToAdd, this);
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
            int InDeck = GetNumInDeck(ToAdd);
            return InDeck < 2 && CardID.Count < 15;
        }
        catch
        {
            return false;
        }
    }
    private int GetNumInDeck(int ID)
    {
        int InDeck = 0;
        CardID.ForEach(id =>
        {
            if (id == ID)
            {
                InDeck++;
            }
        });
        return InDeck;
    }
    private string GetCardName(int ID)
    {
        return CardConnector.GetGameplayCard(ID).GetCardName();
    }

    public void RemoveFromList(int toRemove)
    {
        CardID.Remove(toRemove);
    }

    /// <summary>
    /// Makes sure the deck can be built
    /// </summary>
    /// <returns>If the deck is a valid deck</returns>
    public bool CanBuild()
    {
        return CardID.Count >= 10 && CardID.Count < 16;
    }
    /// <summary>
    /// Build a deck, as long as it is valid
    /// </summary>
    /// <returns>A valid deck</returns>
    public void BuildDeck()
    {
        CustomDeck customDeck = new CustomDeck();
        if (NameInput.text.Trim() == "")
        {
            return;
        }
        customDeck.DeckName = NameInput.text;
        customDeck.Cards = CardID;
        customDeck.IsValid = CanBuild();
        DeckSaver.SaveDeck(customDeck);
    }

    //Build Deck from saved CustomDeck
    public void BuildDeck(CustomDeck SavedDeck)
    {
        List<Gameplay_Card> InitialDeck = new();
        SavedDeck.Cards.ForEach(id =>
        {
            InitialDeck.Add(CardConnector.GetGameplayCard(id));
        });
        RuntimeDeck RD = new RuntimeDeck(InitialDeck.ToArray());
        RD.Init();
        GameManager.Instance.CustomDeck = RD;
    }
}
