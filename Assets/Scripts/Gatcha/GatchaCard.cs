using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;



public class GatchaCard : Card
{
    [SerializeField] string ConnectionName = "Unknown";

    public string GetConnectionName() { return ConnectionName; }

    private void Start()
    {
        Init();
    }

    public void Select()
    {
        //Debug.Log($"{CardName} Selected");
        GameObject DeckBuilderObject = GameObject.Find("DeckBuilder");
        if(DeckBuilderObject.TryGetComponent<Deckbuilder>(out Deckbuilder DeckBuilderComponent))
        {
            //Debug.Log("Deckbuilder Found!");
            DeckBuilderComponent.AddToList(CardID);
        }
    }
}
