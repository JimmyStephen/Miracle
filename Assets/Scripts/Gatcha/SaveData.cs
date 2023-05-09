using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public List<int> rewards;
    public int funds;
    public int pity;

    public SaveData(List<int> rewards, int funds, int pity)
    {
        this.pity = pity;
    }
}
