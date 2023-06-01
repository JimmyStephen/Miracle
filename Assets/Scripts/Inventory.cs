using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class Inventory : Singleton<Inventory>
{
    //[SerializeField] GameObject[] FullRewardList;
    [SerializeField] int numPerPage;
    private List<GameObject> InventoryList;
    private string SelectedDeck;
    private int Funds;
    //private readonly string fileName = "Inventory_Test_Three";
    private readonly string fileName = "Inventory";
    private List<int> rewardsListInt;
    private List<CustomDeck> CustomDeckLists;
    public int pity = 0;
    public List<bool> cheatUse = new List<bool>() { false, false };

    private GameObject[] GatchaCards;

    void Start()
    {
        StartCoroutine(WaitForGameManager());
        //GatchaCards = GameManager.Instance.GetGatchaCards_GameObjects();
        //ReadInInventory();
        //SortByRarity();
    }

    //Add/Update/Remove Methods
    /// <summary>
    /// Updates the funds
    /// </summary>
    /// <param name="ChangeValue">How much to add or take away (- for taking away)</param>
    public void AddFunds(int ChangeValue)
    {
        Funds += ChangeValue;
        SaveData();
    }
    /// <summary>
    /// Update the Pity
    /// </summary>
    /// <param name="newPity">The new Pity Value</param>
    public void AddPity(int newPity)
    {
        pity += newPity;
        if(pity <= 0) pity = 0;
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
        foreach (var adding in toAdd)
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
    public void AddCustomDeck(CustomDeck toAdd)
    {
        CustomDeckLists.Add(toAdd);
        SaveData();
    }
    public void SetSelectedDeck(string DeckName)
    {
        SelectedDeck = DeckName;
        SaveData();
    }
    public void RemoveCustomDeck(string toRemove)
    {
        CustomDeck DeckToRemove = null;
        foreach (var v in CustomDeckLists)
        {
            if (v.DeckName == toRemove)
            {
                DeckToRemove = v;
                break;
            }
        }
        CustomDeckLists.Remove(DeckToRemove);
    }

    //Get Methods
    /// <summary>
    /// Get the current Pity value
    /// </summary>
    /// <returns>The current pity value</returns>
    public int GetPity() {
        return pity;
    }
    public CustomDeck GetSelectedDeck()
    {
        return GetCustomDeck(SelectedDeck);
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
    public CustomDeck[] GetCustomDecks()
    {
        return CustomDeckLists.ToArray();
    }
    public CustomDeck GetCustomDeck(string DeckName)
    {
        try
        {
            return CustomDeckLists.First(cd => cd.DeckName.Equals(DeckName));
        }
        catch
        {
            return null;
        }
    }
    public GameObject GetReward(int index)
    {
        if (index < 0 || index >= GatchaCards.Count()) return null;
        return GatchaCards[index];
    }

    //Private
    /// <summary>
    /// Saves the current Inventory to JSON
    /// </summary>
    private void SaveData()
    {
        List<SaveData> dataList = new();
        dataList.Add(new SaveData(rewardsListInt, Funds, pity, CustomDeckLists, SelectedDeck));
        FileHandler.SaveToJSON<SaveData>(dataList, fileName);
        ReadInInventory();
    }
    private void ReadInInventory()
    {
        List<SaveData> dataList = FileHandler.ReadFromJSON<SaveData>(fileName);
        SaveData data = dataList[0];
        ReadInSaveData(data);
    }
    private void ReadInSaveData(SaveData data)
    {
        Funds = data.funds;
        rewardsListInt = data.rewards;
        CustomDeckLists = data.CDecks;
        SelectedDeck = data.CurrentDeckName;
        pity = data.pity;
        InventoryList = new List<GameObject>();
        foreach (var v in rewardsListInt)
            InventoryList.Add(CardConnector.GetGatchaCardObj(v));
    }
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

    private IEnumerator WaitForGameManager()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        GatchaCards = GameManager.Instance.GetGatchaCards_GameObjects();
        ReadInInventory();
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
            return null;
        }
        if (startIndex < 0) startIndex = 0;
        return GetInventory(startIndex, endIndex);
    }
    public int GetMaxPages()
    {
        float maxPages = InventoryList.Count() / (float)numPerPage;
        int ret = (int)Mathf.Ceil(maxPages);
        if (ret <= 0) ret = 1;
        return ret;
    }
}
