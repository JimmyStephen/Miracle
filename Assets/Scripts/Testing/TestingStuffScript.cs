using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingStuffScript : MonoBehaviour
{
    public void CreateCustomDeck()
    {
        Inventory.Instance.AddFunds(Random.Range(0, 100));
        Inventory.Instance.AddPity(Random.Range(0, 100));
        CustomDeck cd = new();
        InitCustomDeck(cd);
        Inventory.Instance.AddCustomDeck(cd);
    }
    private void InitCustomDeck(CustomDeck cd)
    {
        cd.DeckName = RandomName();
        cd.Cards = RandomCards();
    }
    private string RandomName()
    {
        string[] s1 = { "apple", "banana", "cherry", "date", "elderberry" };
        string[] s2 = { "football", "basketball", "volleyball", "hockey", "soccer" };
        string[] s3 = { "elephant", "giraffe", "rhinoceros", "hippopotamus", "crocodile" };
        string[] s4 = { "computer", "laptop", "tablet", "smartphone", "smartwatch" };

        string s1Select = s1[Random.Range(0, s1.Length)];
        string s2Select = s2[Random.Range(0, s2.Length)];
        string s3Select = s3[Random.Range(0, s3.Length)];
        string s4Select = s4[Random.Range(0, s4.Length)];

        return $"{s1Select}-{s2Select}-{s3Select}-{s4Select}";
    }
    private List<int> RandomCards()
    {
        List<int> tempList = new();
        for (int i = 0; i < 10; i++)
        {
            tempList.Add(Random.Range(1,11));
        }
        return tempList;
    }

    public void ReadInCustomDeck()
    {
        CustomDeck[] CDs = Inventory.Instance.GetCustomDecks();
        string testName = CDs[0].DeckName;
        foreach(var cd in CDs)
        {
            Debug.Log($"{cd.DeckName} : {cd.Cards}");
        }
        CustomDeck SingleCD = Inventory.Instance.GetCustomDeck(testName);
        Debug.Log($"Single: {SingleCD.DeckName} : {SingleCD.Cards}");
    }
}
