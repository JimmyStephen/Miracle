using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class Card : MonoBehaviour
{
    [Header("How to Display")]
    [SerializeField] TMPro.TMP_Text NameTextbox;
    [SerializeField] Image ImageBox;
    [Header("What to Display")]
    [SerializeField] string CardName;
    [SerializeField, Tooltip("Image should have the description")] Sprite Image;
    [SerializeField, Tooltip("Output discription when the card is played")] string CardOutputDescription = "Default Card Effect";
    [Header("What the card does")]
    [SerializeField, Tooltip("List of all the effects the card can trigger")] Card_Effect[] Effects;
    [SerializeField, Tooltip("List of all the events the card can trigger")] Card_Event[] Events;
    public int CardID;

    private void Start()
    {
        _Init();
    }
    /// <summary>
    /// Call when the card is created on the scene, will set all the values as needed
    /// </summary>
    public void _Init()
    {
        if(NameTextbox != null)
            NameTextbox.text = CardName;
        if(ImageBox != null)
            ImageBox.sprite = Image;

        foreach(var effect in Effects)
            effect._Init(CardOutputDescription);
        foreach (var _event in Events)
            _event._Init(CardOutputDescription);
    }

    /// <summary>
    /// Called when a card is locked in, triggers events or effects that trigger at this point
    /// </summary>
    /// <returns>The effect of the card to display on the screen</returns>
    public string PlayCard(Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer, Trigger onTrigger)
    {
        string S_Effect = "No Valid Effect";
        foreach (var effect in Effects)
        {
            if (effect.effectTrigger == onTrigger)
            {
                S_Effect = effect.TriggerEffect(player, AI, GM, PlayedByPlayer);
            }
        }

        foreach (var _event in Events)
        {
            if (_event._eventTrigger == onTrigger)
            {
                S_Effect = _event.TriggerEvent(player, AI, GM, PlayedByPlayer);
            }
        }

        return S_Effect;
    }

    /// <summary>
    /// Lets the gameplay manager know when the card is selected
    /// </summary>
    public void Select()
    {
        FindObjectOfType<GameplayManager>().SetSelectedCard(CardID);
    }
}
