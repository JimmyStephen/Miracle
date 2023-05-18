using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] GameObject DeckSelectorDisplay;
    [SerializeField] GameObject DeckBuilderDisplay;
    [HideInInspector] public InventoryMode CurrentMode = InventoryMode.VIEWER;
    Deckbuilder deckbuilder;
    DeckSelector deckselector;

    private void Start()
    {
        deckbuilder = DeckBuilderDisplay.GetComponent<Deckbuilder>();
        deckselector = DeckSelectorDisplay.GetComponent<DeckSelector>();
        DeckSelectorDisplay.SetActive(true);
        DeckBuilderDisplay.SetActive(false);
        CurrentMode = InventoryMode.VIEWER;
    }

    //IGNORE
    public void UIToggle(int NewMode)
    {
        switch (NewMode)
        {
            case 1:
                Toggle(InventoryMode.VIEWER);
                break;
            case 2:
                Toggle(InventoryMode.DECK_BUILDER);
                break;
            case 3:
                Toggle(InventoryMode.DECK_EDITOR);
                break;
            case 4:
                Toggle(InventoryMode.DECK_SELECTOR);
                break;
            default:
                Debug.Log("Default");
                break;
        }
    }

    public void Toggle(InventoryMode NewMode)
    {
        deckbuilder.ClearDisplay();

        switch (NewMode)
        {
            case InventoryMode.VIEWER:
                DeckSelectorDisplay.SetActive(true);
                DeckBuilderDisplay.SetActive(false);
                break;
            case InventoryMode.DECK_BUILDER:
                DeckSelectorDisplay.SetActive(false);
                DeckBuilderDisplay.SetActive(true);
                deckbuilder.ClearDisplay();
                break;
            case InventoryMode.DECK_EDITOR:
                DeckSelectorDisplay.SetActive(false);
                DeckBuilderDisplay.SetActive(true);
                deckbuilder.FromBaseDeck(deckselector.GetSelectedDeck());
                break;
            case InventoryMode.DECK_SELECTOR:
                Debug.Log("Deck Selector Enabled!");
                DeckSelectorDisplay.SetActive(true);
                DeckBuilderDisplay.SetActive(false);
                break;
            default:
                throw new Exception("How did you get here?");
        }
        CurrentMode = NewMode;
    }
}

[Serializable]
public enum InventoryMode
{
    VIEWER,
    DECK_BUILDER,
    DECK_EDITOR,
    DECK_SELECTOR
}
