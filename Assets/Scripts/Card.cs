using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class Card : MonoBehaviour
{
    [Header("Where to Display")]
    [SerializeField] protected TMPro.TMP_Text NameTextbox;
    [SerializeField] protected Image ImageBox;
    [Header("What to Display")]
    [SerializeField] protected string CardName;
    [SerializeField, Tooltip("Image should have the description")] protected Sprite Image;
    [SerializeField] protected CardType Type = CardType.UNKNOWN;
    [SerializeField] protected Rarity CardRarity = Rarity.NONE;
    public int CardID;

    public void Init() {
      if (NameTextbox != null)
        NameTextbox.text = CardName;
      if (ImageBox != null)
        ImageBox.sprite = Image;
    }
    
    public string GetCardName()   { return CardName; }
    public CardType GetCardType() { return Type; }
    public Rarity GetCardRarity() { return CardRarity; }
    public int GetCardID() {  return CardID; }

    public Sprite GetSprite() { return Image; }
}