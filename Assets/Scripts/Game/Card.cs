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


/*    [Header("Card Effects"), Tooltip("Effects are (normally) single use and trigger when activated")]
    [SerializeField] Target baseCardEffectTarget = Target.NONE;
    [SerializeField] Trigger baseEffectTrigger = Trigger.NONE;
    [SerializeField] Effect baseCardEffectType = Effect.NONE;
    [SerializeField] int effectMinValue = 0;
    [SerializeField, Tooltip("If lower than effectMinValue, always takes min")] int eventMaxValue = -1;*/

    [SerializeField] Card_Effect[] Effects;

    [Header("Event Effects"), Tooltip("Events are duration based and will continue to effect the game until the duration runs out")]
    [SerializeField] Target baseCardEventTarget = Target.NONE;
    [SerializeField] Trigger baseEventTrigger = Trigger.NONE;
    [SerializeField] _Event baseCardEvent = _Event.NONE;
    [SerializeField] int eventDuration = 0;
    public int index;

    //the actual event values, in case they got changed from the base in some way
    private _Event _event;
    private Trigger _eventTrigger;
    private Target _eventTarget;

    private void Start()
    {
        _Init();
    }
    /// <summary>
    /// Call when the card is created on the scene, will set all the values as needed
    /// </summary>
    public void _Init()
    {
        foreach(var effect in Effects)
        {
            effect._Init();
        }

        if(NameTextbox != null)
            NameTextbox.text = CardName;
        if(ImageBox != null)
            ImageBox.sprite = Image;
    }

    //Public and also used Methods
    public void Select()
    {
        FindObjectOfType<GameplayManager>().SetSelectedCard(index);
    }

    //Methods to call when a card is used
    /// <summary>
    /// Called when a card is locked in, triggers events or effects that trigger at this point
    /// </summary>
    /// <returns>The effect of the card to display on the screen</returns>
    public string PlayCard(Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer)
    {
        string S_Effect = "No On Play Effect";
        foreach (var effect in Effects)
        {
            if (effect.effectTrigger == Trigger.ON_PLAY)
            {
                S_Effect = TriggerEffect(effect, player, AI, GM, PlayedByPlayer);
            }
        }
        if (_eventTrigger == Trigger.ON_PLAY)
        {
            S_Effect = TriggerEvent(player, AI, GM, PlayedByPlayer);
        }
        return S_Effect;
    }
    /// <summary>
    /// Called at the end of the round, triggers events or effects that trigger at this point
    /// </summary>
    public void CardInHand(Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer)
    {
        foreach (var effect in Effects)
        {
            if (effect.effectTrigger == Trigger.IN_HAND)
            {
                TriggerEffect(effect, player, AI, GM, PlayedByPlayer);
            }
        }
        if (_eventTrigger == Trigger.IN_HAND)
        {
            TriggerEvent(player, AI, GM, PlayedByPlayer);
        }
    }
    /// <summary>
    /// If the card has a start of game effect or event, trigger it
    /// </summary>
    public void StartOfGame(Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer)
    {
        foreach (var effect in Effects)
        {
            if (effect.effectTrigger == Trigger.START_OF_GAME)
            {
                TriggerEffect(effect, player, AI, GM, PlayedByPlayer);
            }
        }
        if (_eventTrigger == Trigger.START_OF_GAME)
        {
            TriggerEvent(player, AI, GM, PlayedByPlayer);
        }
    }


    //Private Methods
    /// <summary>
    /// Called if the cards effect should trigger
    /// </summary>
    private string TriggerEffect(Card_Effect effect, Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer)
    {
        string EffectDisplay;
        switch (effect.effect)
        {
            case Effect.DAMAGE:
                EffectDisplay = effect.TriggerEffect(player, AI, GM, PlayedByPlayer);
                break;
            case Effect.SHIELD:
                EffectDisplay = effect.TriggerEffect(player, AI, GM, PlayedByPlayer);
                break;
            case Effect.HEAL:
                EffectDisplay = effect.TriggerEffect(player, AI, GM, PlayedByPlayer);
                break;
            case Effect.DRAW:
                EffectDisplay = effect.TriggerEffect(player, AI, GM, PlayedByPlayer);
                break;
            default:
                throw new System.Exception("You should never reach this point");
        }
        return EffectDisplay;
    }
    /// <summary>
    /// Called if the cards effect should trigger
    /// </summary>
    private string TriggerEvent(Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer)
    {
        //If the event is already ongoing, remove it and replace it with the newer one
        if (GM.OngoingEvents.TryGetValue(_event, out int CurrentEventDuration))
        {
            if (eventDuration > CurrentEventDuration)
            {
                GM.OngoingEvents.Remove(_event);
                GM.OngoingEvents.Add(_event, eventDuration);
            }
        }
        else
        {
            GM.OngoingEvents.Add(_event, eventDuration);
        }
        return "Event Added";
    }

}
