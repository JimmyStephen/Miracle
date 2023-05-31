using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryComunicator : MonoBehaviour
{
    [SerializeField] GameObject InventoryDisplay;
    [SerializeField] GameObject InventorySlotBackground;
    [SerializeField] GameObject SelectedLocation;
    [SerializeField] TMPro.TMP_Text pageNumDisplay;
    [SerializeField] TMP_Text money;
    private List<GameObject> currentDisplayedInventory;

    private int currentPageNum = 0;

    private Inventory inventory;
    private void Start()
    {
        money.text = Inventory.Instance.GetFunds().ToString();
        //Get Inventory
        inventory = Inventory.Instance;
        currentDisplayedInventory = new List<GameObject>();
        GetPageOne();
    }

    //Methods
    public void NextPage()
    {
        currentPageNum++;
        GameObject[] temp = inventory.GetPage(currentPageNum);
        if (temp == null || temp.Length == 0)
        {
            //Debug.Log("Temp is null or empty");
            currentPageNum--;
            return;
        }
        ClearInventoryDisplay();
        DisplayInventory(temp);
    }
    public void PrevPage()
    {
        currentPageNum--;
        if(currentPageNum < 0) currentPageNum = 0;
        GameObject[] temp = inventory.GetPage(currentPageNum);
        if (temp == null)
        {
            return;
        }
        ClearInventoryDisplay();
        DisplayInventory(temp);
    }
    //public void Order(int orderOption)
    //{
    //    switch (orderOption)
    //    {
    //        case 0:
    //            inventory.SortByAlphabetical();
    //            break;
    //        case 1:
    //            inventory.SortByReceived();
    //            break;
    //        case 2:
    //            inventory.SortByRarity();
    //            break;
    //        case 3:
    //            inventory.ReverseSort();
    //            break;
    //        default:
    //            Debug.Log("How??");
    //            break;
    //    }
    //    GetPageOne();
    //}

    GameObject currentSelected;
    public void ToggleFocused(int index)
    {
        GameObject toSpawn = inventory.GetReward(index);
        if (toSpawn == null)
        {
            Debug.Log("Error, to spawn {" + index + "} == null");
            return;
        }

        Destroy(currentSelected);
        currentSelected = Instantiate(toSpawn, SelectedLocation.transform);
    }

    //private
    private void ClearInventoryDisplay()
    {
        foreach(var v in currentDisplayedInventory)
        {
            Destroy(v);
        }
        currentDisplayedInventory.Clear();
    }
    private void DisplayInventory(GameObject[] toDisplay)
    {
        foreach (var v in toDisplay)
        {
            GameObject temp = Instantiate(InventorySlotBackground, InventoryDisplay.transform);
            GameObject temp2 = Instantiate(v, temp.transform);
            temp2.GetComponent<GatchaCard>().Init();
            currentDisplayedInventory.Add(temp);
        }
        SetPageNumDisplay();
    }
    private void GetPageOne()
    {
        currentPageNum = 0;
        GameObject[] temp = inventory.GetPage(currentPageNum);
        if (temp == null) return;
        ClearInventoryDisplay();
        DisplayInventory(temp);
    }
    private void SetPageNumDisplay()
    {
        pageNumDisplay.text = "Page " + (currentPageNum + 1) + "/" + inventory.GetMaxPages();
    }
}
