using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DeckSaver
{
    //Save a deck
        //if it doesnt exist, just save it
        //if it does, delete the previous deck with the name and replace it

    public static void SaveDeck(CustomDeck cd)
    {
        if (CheckIfExists(cd.DeckName))
            RemoveOldDeck(cd.DeckName);
        SaveToData(cd);
    }

    private static bool CheckIfExists(string DeckName)
    {
        var DeckFound = Inventory.Instance.GetCustomDeck(DeckName);
        return DeckFound != null;
    }

    private static void RemoveOldDeck(string DeckName)
    {
        Inventory.Instance.RemoveCustomDeck(DeckName);
    }

    private static void SaveToData(CustomDeck cd)
    {
        Inventory.Instance.AddCustomDeck(cd);
    }
}
