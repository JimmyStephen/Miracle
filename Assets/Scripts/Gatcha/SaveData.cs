using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public List<int> rewards;
    public int funds;
    public int pity;
    public string CurrentDeckName;
    public List<CustomDeck> CDecks;

    public SaveData(List<int> rewards, int funds, int pity, List<CustomDeck> cDecks)
    {
        this.rewards = rewards;
        this.funds = funds;
        this.pity = pity;
        CDecks = cDecks;
    }

    public SaveData(List<int> rewards, int funds, int pity, List<CustomDeck> cDecks, string currentDeckName)
    {
        this.rewards = rewards;
        this.funds = funds;
        this.pity = pity;
        CDecks = cDecks;
        CurrentDeckName = currentDeckName;
    }
}
