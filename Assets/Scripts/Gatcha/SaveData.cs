using System;
using System.Collections.Generic;

//Change to Save Data Later****
[Serializable]
public class SaveData
{
    public List<int> rewards;
    public int funds;

    public SaveData(List<int> rewards, int funds)
    {
        this.rewards = rewards;
        this.funds = funds;
    }
}
