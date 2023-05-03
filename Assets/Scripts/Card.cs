using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class Card : MonoBehaviour
{
    [Header("Where to Display")]
    [SerializeField] TMPro.TMP_Text NameTextbox;
    [SerializeField] Image ImageBox;
    [Header("What to Display")]
    [SerializeField] string CardName;
    [SerializeField, Tooltip("Image should have the description")] Sprite Image;
    [SerializeField] CardType Type = CardType.UNKNOWN;
    [SerializeField] Rarity CardRarity = Rarity.NONE;
    public int CardID;

    public string GetCardName()   { return CardName; }
    public CardType GetCardType() { return Type; }
    public Rarity GetCardRarity() { return CardRarity; }
}