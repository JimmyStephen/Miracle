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

    /// <summary>
    /// Gets the funds from the inventory
    /// </summary>
    /// <returns>How many funds the player has</returns>
    public int GetFunds()
    {
        return Funds;
    }
    /// <summary>
    /// Updates the funds
    /// </summary>
    /// <param name="ChangeValue">How much to add or take away (- for taking away)</param>
    public void UpdateFunds(int ChangeValue)
    {
        //Update
        Funds += ChangeValue;
        //add to data
        List<SaveData> dataList = new();
        dataList.Add(new SaveData(rewardsListInt, Funds));
        FileHandler.SaveToJSON<SaveData>(dataList, fileName);
    }
    /// <summary>
    /// Gets items from the inventory from a range, validates to make sure the range is valid and returns as many as possible
    /// </summary>
    /// <param name="startIndex">Where to start in the array</param>
    /// <param name="endIndex">Where to end in the array</param>
    /// <returns>List of all the objects found in the requested range</returns>
    public GameObject[] GetInventory(int startIndex, int endIndex)
    {
        if(startIndex < 0) startIndex = 0;
        int count = endIndex- startIndex;
        if (count > InventoryList.Count() - startIndex) count = InventoryList.Count() - startIndex;
        return InventoryList.GetRange(startIndex, count).ToArray();
    }
    public GameObject GetReward(int index)
    {
        if(index < 0 || index >= FullRewardList.Count()) return null;
        return FullRewardList[index];
    }

    /// <summary>
    /// Adds one item to the inventory
    /// </summary>
    /// <param name="toAdd">The item to add</param>
    /// <returns>How many of the items already exist in the inventory</returns>
    public int AddToInventory(GameObject toAdd)
    {
        //check if exists
        if (CheckIfContains(toAdd.GetComponent<Reward>()))
        {
            return 1;
        }
        //Add
        InventoryList.Add(toAdd);
        rewardsListInt.Add(toAdd.GetComponent<Reward>().GetIndex());
        //add to data
        List<SaveData> dataList = new();
        dataList.Add(new SaveData(rewardsListInt, Funds));
        FileHandler.SaveToJSON<SaveData>(dataList, fileName);
        return 0;
    }
    /// <summary>
    /// Adds multiple items to the inventory
    /// </summary>
    /// <param name="toAdd">The array of items to add</param>
    /// <returns>How many of the items already exist in the inventory</returns>
    public int AddToInventory(GameObject[] toAdd)
    {
        int repeated = 0;
        //add in mass
        foreach(var adding in toAdd)
        {
            //check if it already contains
            if (CheckIfContains(adding.GetComponent<Reward>()))
            {
                repeated++;
            }
            else
            {
                //Add
                InventoryList.Add(adding);
                rewardsListInt.Add(adding.GetComponent<Reward>().GetIndex());
                //add to data
                List<SaveData> dataList = new();
                dataList.Add(new SaveData(rewardsListInt, Funds));
                FileHandler.SaveToJSON<SaveData>(dataList, fileName);
            }
        }
        return repeated;
    }

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
        if(startIndex < 0) startIndex = 0;
        //Debug.Log("Start Index: " + startIndex + " End Index: " + endIndex + " Num in Array: " + InventoryList.Count());
        return GetInventory(startIndex, endIndex);
    }

    public int GetMaxPages()
    {
        float maxPages = InventoryList.Count() / (float)numPerPage;
        int ret = (int)Mathf.Ceil(maxPages);
        if(ret <= 0) ret = 1;
        return ret;
    }

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

    //Private
    private bool CheckIfContains(Reward checking)
    {
        foreach(var v in InventoryList)
        {
            Reward containedReward = v.GetComponent<Reward>();
            if (checking.GetName().Equals(containedReward.GetName()) && checking.GetRarity().Equals(containedReward.GetRarity()))
            {
                return true;
            }
        }
        return false;
    }

  public int GetPity() {
    return pity;
  }

  public void AddPity(int newPity) {
    pity += newPity;
  }
}
