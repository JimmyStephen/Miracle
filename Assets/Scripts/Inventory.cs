using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : Singleton<Inventory>
{
    [SerializeField] GameObject[] FullRewardList;
    [SerializeField] int numPerPage;
    private List<GameObject> InventoryList;
    private int Funds;
    private readonly string fileName = "Inventory";
    private List<int> rewardsListInt;

    //save pity to JSON
    public int pity = 0;
    public List<bool> cheatUse = new List<bool>() { false, false };

  // Start is called before the first frame update
  void Start()
    {
        //Set Index (If Needed Uncomment)
        for(int i = 0; i < FullRewardList.Length; i++)
        {
            FullRewardList[i].GetComponent<Reward>().SetIndex(i);
        }

        //read the saved data
        List<SaveData> dataList = FileHandler.ReadFromJSON<SaveData>(fileName);
        if(dataList == null) { Debug.Log("Empty List | File Does Not Exist"); }
        SaveData data = dataList[0];
        InventoryList = new List<GameObject>();

        Funds = data.funds;
        rewardsListInt = data.rewards;
        foreach(var v in rewardsListInt)
        {
            InventoryList.Add(FullRewardList[v]);
            //Debug.Log("Stored Data: " + InventoryList[v].GetComponent<Reward>().GetName() + " Rarity: " + InventoryList[v].GetComponent<Reward>().GetRarity());
        }

        SortByRarity();
    }

    //Set Methods
    /// <summary>
    /// Updates the funds
    /// </summary>
    /// <param name="ChangeValue">How much to add or take away (- for taking away)</param>
    public void UpdateFunds(int ChangeValue)
    {
        Funds += ChangeValue;
        //add to data
        SaveData();
    }
    /// <summary>
    /// Update the Pity
    /// </summary>
    /// <param name="newPity">The new Pity Value</param>
    public void AddPity(int newPity)
    {
        pity += newPity;
        //add to data
        SaveData();
    }

    public void SetCheatUse(bool cheat, int index) {
      cheatUse[index] = cheat;
      SaveData();
    }

    /// <summary>
    /// Adds one item to the inventory
    /// </summary>
    /// <param name="toAdd">The item to add</param>
    /// <returns>How many of the items already exist in the inventory</returns>
    public void AddToInventory(GameObject toAdd)
    {
        if (!CheckIfContains(toAdd.GetComponent<GatchaCard>()))
        {
            InventoryList.Add(toAdd);
            rewardsListInt.Add(toAdd.GetComponent<GatchaCard>().GetCardID());
            SaveData();
        }
    }
    /// <summary>
    /// Adds multiple items to the inventory
    /// </summary>
    /// <param name="toAdd">The array of items to add</param>
    /// <returns>How many of the items already exist in the inventory</returns>
    public void AddToInventory(GameObject[] toAdd)
    {
        //add in mass
        foreach(var adding in toAdd)
        {
            //check if it already contains
            if (!CheckIfContains(adding.GetComponent<GatchaCard>()))
            {
                InventoryList.Add(adding);
                rewardsListInt.Add(adding.GetComponent<GatchaCard>().GetCardID());
            }
        }
        SaveData();
    }


    //Get Methods
    /// <summary>
    /// Get the current Pity value
    /// </summary>
    /// <returns>The current pity value</returns>
    public int GetPity() {
        return pity;
    }

    public List<bool> GetCheatUse() {
      return cheatUse;
    }

    /// <summary>
    /// Gets the funds from the inventory
    /// </summary>
    /// <returns>How many funds the player has</returns>
    public int GetFunds()
    {
        return Funds;
    }
    /// <summary>
    /// Gets items from the inventory from a range, validates to make sure the range is valid and returns as many as possible
    /// </summary>
    /// <param name="startIndex">Where to start in the array</param>
    /// <param name="endIndex">Where to end in the array</param>
    /// <returns>List of all the objects found in the requested range</returns>
    public GameObject[] GetInventory(int startIndex, int endIndex)
    {
        if (startIndex < 0) startIndex = 0;
        int count = endIndex - startIndex;
        if (count > InventoryList.Count() - startIndex) count = InventoryList.Count() - startIndex;
        return InventoryList.GetRange(startIndex, count).ToArray();
    }
    public GameObject[] GetInventory()
    {
        return InventoryList.ToArray();
    }

    public GameObject GetReward(int index)
    {
        if (index < 0 || index >= FullRewardList.Count()) return null;
        return FullRewardList[index];
    }


    /// <summary>
    /// Saves the current Inventory to JSON
    /// </summary>
    private void SaveData()
    {
        List<SaveData> dataList = new();
        dataList.Add(new SaveData(rewardsListInt, Funds, pity));
        FileHandler.SaveToJSON<SaveData>(dataList, fileName);
    }

    //Private
    private bool CheckIfContains(GatchaCard checking)
    {
        foreach (var v in InventoryList)
        {
            GatchaCard containedReward = v.GetComponent<GatchaCard>();
            if (checking.GetCardName().Equals(containedReward.GetCardName()) && checking.GetCardRarity().Equals(containedReward.GetCardRarity()))
            {
                return true;
            }
        }
        return false;
    }

    //--------------------------------------------------------
    //Probably move
    /// <summary>
    /// Goes to the next page of items (use with inventory scene)
    /// </summary>
    /// <returns>The list of objects to display</returns>
    public GameObject[] GetPage(int pageNum)
    {
        int startIndex = pageNum * numPerPage;
        int endIndex = startIndex + numPerPage;
        if (startIndex > InventoryList.Count())
        {
            //            Debug.Log("Start is beyond the limits, returning");
            return null;
        }
        if (startIndex < 0) startIndex = 0;
        //Debug.Log("Start Index: " + startIndex + " End Index: " + endIndex + " Num in Array: " + InventoryList.Count());
        return GetInventory(startIndex, endIndex);
    }
    public int GetMaxPages()
    {
        float maxPages = InventoryList.Count() / (float)numPerPage;
        int ret = (int)Mathf.Ceil(maxPages);
        if (ret <= 0) ret = 1;
        return ret;
    }
    //Sort (Remove/Move)
    public void SortByAlphabetical()
    {
        InventoryList = InventoryList.OrderBy(v => v.GetComponent<Reward>().GetName()).ToList();
    }
    public void SortByRarity()
    {
        InventoryList = InventoryList.OrderBy(v => v.GetComponent<Reward>().GetRarityE()).ToList();
        InventoryList.Reverse();
    }
    public void SortByReceived()
    {
        InventoryList.Clear();
        foreach (var v in rewardsListInt)
        {
            InventoryList.Add(FullRewardList[v]);
        }
    }
    public void ReverseSort()
    {
        InventoryList.Reverse();
    }
}
