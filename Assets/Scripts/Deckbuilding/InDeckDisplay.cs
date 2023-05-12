using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InDeckDisplay : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text Display;
    private int num;
    private int id;
    private string StoredName;
    private Deckbuilder deckbuilder;

    public void Init(string CardName, int NumInDeck, int cardID, Deckbuilder db)
    {
        var temp = GameObject.Find("Display_" + CardName);
        name = "Display_" + CardName;
        deckbuilder = db;
        id = cardID;

        if (temp)
        {
            temp.GetComponent<InDeckDisplay>().UpdateDisplay(CardName, NumInDeck);
            Destroy(this.gameObject);
        }
        else
            UpdateDisplay(CardName, NumInDeck);
    }

    private void UpdateDisplay(string CardName, int NumInDeck)
    {
        num = NumInDeck;
        StoredName = CardName;
        Display.SetText($"{num}x : {StoredName}");
    }

    public void Select()
    {
        num--;
        deckbuilder.RemoveFromList(id);
        if(num == 0)
            Destroy(this.gameObject);
        else
            UpdateDisplay(StoredName, num);
    }
}
