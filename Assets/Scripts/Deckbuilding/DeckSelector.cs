using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckSelector : MonoBehaviour
{
    [SerializeField, Tooltip("The Prefab to be displayed")] GameObject Display;
    [SerializeField, Tooltip("Where the prefab will be displayed")] GameObject DisplayParent;
    [SerializeField, Tooltip("Where the cards in the deck are displayed")] GameObject DisplayDeck;
    private List<GameObject> DisplayedDecks = new();
    private string CurrentSelectedName;

    private void OnEnable()
    {
        ClearDisplay();
        var decks = Inventory.Instance.GetCustomDecks();
        foreach (var deck in decks)
        {
            //Debug.Log("Deck: " + deck.DeckName);
            var temp = Instantiate(Display, DisplayParent.transform);
            DisplayedDecks.Add(temp);
            temp.GetComponent<DeckDisplay>().Init(deck, this);
        }
    }

    public void SaveSelectedDeck()
    {
        Debug.Log("Deck Saved: " + CurrentSelectedName);
        Inventory.Instance.SetSelectedDeck(CurrentSelectedName);
    }
    
    public void Select(string DeckName)
    {
        CurrentSelectedName = DeckName;
        InventoryManager IM = FindObjectOfType<InventoryManager>();
        if (IM.CurrentMode == InventoryMode.DECK_SELECTOR)
            SaveSelectedDeck();
        else
            IM.Toggle(InventoryMode.DECK_EDITOR);
    }
    private void ClearDisplay()
    {
        DisplayedDecks.ForEach(obj => Destroy(obj));
        DisplayedDecks.Clear();
    }
    public CustomDeck GetSelectedDeck()
    {
        var temp = Inventory.Instance.GetCustomDeck(CurrentSelectedName);
        return temp;
    }
}
