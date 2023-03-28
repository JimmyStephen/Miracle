using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    public enum RarityE
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    [SerializeField] TMPro.TMP_Text nameDisplay;
    [SerializeField] TMPro.TMP_Text rarityDisplay;
    [SerializeField] Image imageDisplay;

    [SerializeField] string Name;
    [SerializeField] RarityE Rarity;
    [SerializeField] AudioSource audioToPlay;
    [SerializeField] Sprite image;

    [SerializeField] int Index = 0;

    private void Start()
    {
        nameDisplay.SetText(Name);
        rarityDisplay.SetText(Rarity.ToString());
        if(image != null) imageDisplay.sprite = image;
    }

    public void OnClickReward()
    {
        //play audio if there is audio
        if (audioToPlay != null)
        {
            if (!audioToPlay.isPlaying) audioToPlay.Play();
            else
            {
                audioToPlay.Stop();
                audioToPlay.Play();
            }
        }

        InventoryComunicator ic = FindObjectOfType<InventoryComunicator>();
        if (ic != null)
        {
            ic.ToggleFocused(Index);
        }
    }
    public string GetName()
    {
        return Name;
    }
    public string GetRarity()
    {
        return Rarity.ToString();
    }
    public RarityE GetRarityE()
    {
        return Rarity;
    }
    public int GetIndex()
    {
        return Index;
    }

    public void SetIndex(int i)
    {
        Index = i;
    }
}
