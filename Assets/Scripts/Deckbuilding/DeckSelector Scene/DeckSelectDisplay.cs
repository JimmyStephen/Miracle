using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckSelectDisplay : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text DeckNameDisplay;
    [SerializeField] GameObject SelectedToggle;
    DeckDisplayLoader loader;

    public void Init(string DeckName, bool enabled, DeckDisplayLoader DDL)
    {
        DeckNameDisplay.text = DeckName;
        loader = DDL;
        Enabled(enabled);
    }

    public void Enabled(bool enabled)
    {
        SelectedToggle.SetActive(enabled);
    }

    public void Select()
    {
        if(!Inventory.Instance.GetCustomDeck(DeckNameDisplay.text).IsValid)
        {
            return;
        }
        Inventory.Instance.SetSelectedDeck(DeckNameDisplay.text);
        loader.ChangeSelected(this);
        Enabled(true);
    }
}
