using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatchaMachine : MonoBehaviour
{

    [Header("Pull Data")]
    [SerializeField] int pullCost = 100;
    [SerializeField] int duplicateRefund = 50;

    [Header("What you can get when you roll a reward")]
    [SerializeField] GameObject[] CommonRewards;    //1
    [SerializeField] GameObject[] UncommonRewards;  //2
    [SerializeField] GameObject[] RareRewards;      //3
    [SerializeField] GameObject[] EpicRewards;      //4
    [SerializeField] GameObject[] LegendaryRewards; //5

    [Header("What you need to roll at or above on the RNG to get this reward, chooses the highest value")]
    //[SerializeField] int CommonRarity = 0;
    [SerializeField] int UncommonRarity = 51;
    [SerializeField] int RareRarity = 81;
    [SerializeField] int EpicRarity = 95;
    [SerializeField] int LegendaryRarity = 100;

    [Header("Display")]
    [SerializeField] GameObject RewardDisplay;

    [Header("Audio")]
    [SerializeField] AudioSource PullAudio;
    [SerializeField] AudioSource LackingFundsAudio;

    private List<GameObject> currentDisplay;
    private void Start()
    {
        currentDisplay = new List<GameObject>();
    }

    public void GetGatcha(int numToGet)
    {
        //validate funds
        if (!ValidateRequiredFunds(numToGet))
        {
            if (LackingFundsAudio != null) LackingFundsAudio.Play();
            return;
        }

        if(PullAudio != null) PullAudio.Play();
        ClearPreviousRewards();

        //what you get at the end
        List<GameObject> rewards = new();
        //while you need to get more pulls
        for(int i = 0; i < numToGet; i++)
        {
            //choose the rarity
            int RNG = Random.Range(1, 101);
            int rarity = 1;
            if (RNG >= UncommonRarity) rarity = 2;
            if (RNG >= RareRarity) rarity = 3;
            if (RNG >= EpicRarity) rarity = 4;
            if (RNG >= LegendaryRarity) rarity = 5;

            //choose the reward
            rewards.Add(rarity switch
            {
                1 => CommonRewards[Random.Range(0, CommonRewards.Length)],
                2 => UncommonRewards[Random.Range(0, UncommonRewards.Length)],
                3 => RareRewards[Random.Range(0, RareRewards.Length)],
                4 => EpicRewards[Random.Range(0, EpicRewards.Length)],
                _ => LegendaryRewards[Random.Range(0, LegendaryRewards.Length)]
            });
        }

        //subtract the funds after the rolls succeed
        Inventory.Instance.UpdateFunds(-numToGet * pullCost);

        //add the new rewards to the inventory
        int refundNum = Inventory.Instance.AddToInventory(rewards.ToArray());

        Inventory.Instance.UpdateFunds(refundNum * duplicateRefund);

        //display the rewards (testing)
        foreach(var v in rewards)
        {
            currentDisplay.Add(Instantiate(v, RewardDisplay.transform));
        }
    }

    private bool ValidateRequiredFunds(int numPulls)
    {
        //get funds from inventory
        int funds = Inventory.Instance.GetFunds();
        //make sure funds are greater than num pulls * pull num
        return (funds >= numPulls * pullCost);
    }

    private void ClearPreviousRewards()
    {
        foreach(var v in currentDisplay)
        {
            Destroy(v);
        }
        currentDisplay.Clear();
    }
}
