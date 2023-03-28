using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    enum CardType
    {
        DAMAGE,
        SHIELD,
        HEAL,
        RANDOM,
        EFFECT
    }

    [Header("How to Display")]
    [SerializeField] TMPro.TMP_Text NameTextbox;
    [SerializeField] TMPro.TMP_Text DescTextbox;
/*    [Header("Display")]
    [SerializeField] Image background;
    [SerializeField] Color DeselectedColor;
    [SerializeField] Color selectedColor;*/
    [Header("Card Effects")]
    [SerializeField] string CardName;
    [SerializeField] string CardDescription;
    [SerializeField] CardType cardType;
    [SerializeField] int minNumber = 0;
    [SerializeField, Tooltip("If lower than min, always takes min")] int maxNumber = -1;
    public int index;
    private int effectValue;
    private CardType currentCardType;
    //bool selected = false;
    

    private void Start()
    {
        NameTextbox.text = CardName;
        DescTextbox.text = CardDescription;
        effectValue = -1;

        //if random, choose what it will be
        if (cardType == CardType.RANDOM)
        {
            int temp = Random.Range(1, 4);
            currentCardType = temp switch
            {
                1 => CardType.SHIELD,
                2 => CardType.HEAL,
                _ => CardType.DAMAGE,
            };
        }
        else
        {
            currentCardType = cardType;
        }
    }

    public void Init()
    {
        //WHY!!!????
        Start();
    }

    /// <summary>
    /// What the card will do when it gets used
    /// </summary>
    /// <returns>The amount of Damage or Shielding done</returns>
    public int GetValue()
    {
        if(effectValue != -1) return effectValue;

        if (minNumber > maxNumber)
        {
            effectValue = minNumber;
        }
        else
        {
            effectValue = Random.Range(minNumber, maxNumber);
        }
        return effectValue;
    }

    /// <summary>
    /// Returns the priority of the card, lower number means it goes sooner
    /// </summary>
    /// <returns>The priority value of the card based on its type</returns>
    public int GetPriority()
    {
        //Shield goes first
        //Then Heal
        //Then Damage
        return currentCardType switch
        {
            CardType.SHIELD => 1,
            CardType.HEAL => 2,
            CardType.DAMAGE => 3,
            CardType.EFFECT => 4,
            _ => 0,
        };
    }

    public string GetCardEffect()
    {
        return currentCardType switch
        {
            CardType.SHIELD => "blocked " + effectValue + " damage",
            CardType.HEAL => "healed " + effectValue + " damage",
            CardType.DAMAGE => "did  " + effectValue + " damage",
            _ => "did: " + CardDescription + " effect",
        };
    }

    public void Select()
    {
        FindObjectOfType<GameplayManager>().SetSelectedCard(index);
    }

    /// <summary>
    /// Test Method
    /// </summary>
    /// <returns>Name of the card</returns>
    public string GetName()
    {
        return CardName;
    }
    public string GetCardType()
    {
        return currentCardType.ToString();
    }
}
