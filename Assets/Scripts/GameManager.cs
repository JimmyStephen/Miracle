using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameObject[] GameplayCardsList;
    [SerializeField] GameObject[] GatchaCardsList;

    public RuntimeDeck CustomDeck { get; set; }

    private void Start()
    {
        CardConnector.InitCardID();
    }

    public void ChangeScene(int newScene)
    {
        SceneLoader.Instance.LoadScene(newScene);
    }

    public GameObject[] GetGameplayCards_GameObjects()
    {
        return GameplayCardsList;
    }
    public Gameplay_Card[] GetGameplayCards_Cards()
    {
        List<Gameplay_Card> cards = new();
        foreach (var v in GameplayCardsList)
        {
            cards.Add(v.GetComponent<Gameplay_Card>());
        }
        return cards.ToArray();
    }

    public GameObject[] GetGatchaCards_GameObjects()
    {
        return GatchaCardsList;
    }
    public GatchaCard[] GetGatchaCards_Cards()
    {
        List<GatchaCard> cards = new();
        foreach (var v in GatchaCardsList)
        {
            cards.Add(v.GetComponent<GatchaCard>());
        }
        return cards.ToArray();
    }


    public void SortCardsByID()
    {
        SortGameplayCardsByID();
        SortGatchaCardsByID();
    }
    public void SortGameplayCardsByID()
    {
        GameObject[] TempArr = new GameObject[GameplayCardsList.Length];
        for (int i = 0; i < GameplayCardsList.Length; i++)
        {
            TempArr[i] = CardConnector.GetGameplayCardObj(i + 1);
        }
        GameplayCardsList = TempArr;
    }
    public void SortGatchaCardsByID()
    {
        GameObject[] TempArr = new GameObject[GatchaCardsList.Length];
        for(int i = 0; i < GatchaCardsList.Length; i++)
        {
            TempArr[i] = CardConnector.GetGatchaCardObj(i + 1);
        }
        GatchaCardsList = TempArr;
    }
}

