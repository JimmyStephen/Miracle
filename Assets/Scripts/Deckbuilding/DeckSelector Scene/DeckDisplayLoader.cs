using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckDisplayLoader : MonoBehaviour
{
    [SerializeField, Tooltip("The Prefab to be displayed")] GameObject Display;
    [SerializeField, Tooltip("Where the prefabs will be displayed")] GameObject DisplayParent;

    List<DeckSelectDisplay> OBJ;
    DeckSelectDisplay CurrentSelected;

    void Start()
    {
        var decks = Inventory.Instance.GetCustomDecks();
        string selectedDeck = Inventory.Instance.GetSelectedDeck().DeckName;
        foreach(var deck in decks)
        {
            GameObject temp = Instantiate(Display, DisplayParent.transform);
            bool currentSelected = (deck.DeckName == selectedDeck);
            temp.GetComponent<DeckSelectDisplay>().Init(deck.DeckName, currentSelected, this);
            if (currentSelected ) CurrentSelected = temp.GetComponent<DeckSelectDisplay>();
        }
    }

    public void ChangeSelected(DeckSelectDisplay newSelected)
    {
        CurrentSelected.Enabled(false);
        CurrentSelected = newSelected;
    }
}
