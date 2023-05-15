using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckSelector : MonoBehaviour
{
    [SerializeField, Tooltip("The Prefab to be displayed")] GameObject Display;
    [SerializeField, Tooltip("Where the prefab will be displayed")] GameObject DisplayParent;
    [SerializeField, Tooltip("Where the cards in the deck are displayed")] GameObject DisplayDeck;
    private List<GameObject> DisplayedCards = new();
    private string CurrentSelectedName = Inventory.Instance.GetSelectedDeck().DeckName;

    void Start()
    {
        //Display all the decks on the display list
        var decks = Inventory.Instance.GetCustomDecks();
        foreach(var deck in decks)
        {
            var temp = Instantiate(Display, DisplayParent.transform);
            temp.GetComponent<DeckDisplay>().Init(deck, this);
        }
    }

    public void SaveSelectedDeck()
    {
        Inventory.Instance.SetSelectedDeck(CurrentSelectedName);
    }

    public void DisplayCards(Deck DeckToDisplay, string DeckName)
    {
        CurrentSelectedName = DeckName;
        ClearDisplay();
        var Cards = GameManager.Instance.GetGameplayCards_GameObjects();
        DeckToDisplay.GetStartingDeck().ForEach(card =>
        {
            DisplayedCards.Add(Instantiate(Cards[card.CardID], DisplayDeck.transform));
        });
    }
    private void ClearDisplay()
    {
        DisplayedCards.ForEach(obj =>
        {
            Destroy(obj);
        });
        DisplayedCards.Clear();
    }
}
