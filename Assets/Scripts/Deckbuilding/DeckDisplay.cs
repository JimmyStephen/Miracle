using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckDisplay : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text Display;
    private CustomDeck deck;
    private DeckSelector Selector;

    public void Init(CustomDeck Deck, DeckSelector DeckSelector)
    {
        deck = Deck;
        name = "Display_" + Deck.DeckName;
        Selector = DeckSelector;
        Display.SetText(Deck.DeckName);
    }

    public void Select()
    {
        Selector.Select(deck.DeckName);
    }
}
