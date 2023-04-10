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


    [Header("Card Effects"), Tooltip("Effects are (normally) single use and trigger when activated")]
    [SerializeField] Target baseCardEffectTarget = Target.NONE;
    [SerializeField] Trigger baseEffectTrigger = Trigger.NONE;
    [SerializeField] Effect baseCardEffectType = Effect.NONE;
    [SerializeField] int effectMinValue = 0;
    [SerializeField, Tooltip("If lower than effectMinValue, always takes min")] int eventMaxValue = -1;

    [Header("Event Effects"), Tooltip("Events are duration based and will continue to effect the game until the duration runs out")]
    [SerializeField] Target baseCardEventTarget = Target.NONE;
    [SerializeField] Trigger baseEventTrigger = Trigger.NONE;
    [SerializeField] _Event baseCardEvent = _Event.NONE;
    [SerializeField] int eventDuration = 0;
    public int index;

    //the actual effect values in case they got changed from the base in some way
    private Effect effect;
    private Target effectTarget;
    private Trigger effectTrigger;

    //the actual event values, in case they got changed from the base in some way
    private _Event _event;
    private Trigger _eventTrigger;
    private Target _eventTarget;

    //value to be passed into the effect method
    private int effectValue;

    private void Start()
    {
        //Debug.Log("Calling Init From Start");
        _Init();
    }
    /// <summary>
    /// Call when the card is created on the scene, will set all the values as needed
    /// </summary>
    public void _Init()
    {
        //Debug.Log("Init Called");
        //Init all the values as needed
        //set the effect target
        if (baseCardEffectTarget == Target.RANDOM)
        {
            //choose a random target
            int temp = Random.Range(1, 4);
            effectTarget = temp switch
            {
                1 => Target.SELF,
                2 => Target.OPPONENT,
                _ => Target.BOTH
            };
        }
        else
        {
            effectTarget = baseCardEffectTarget;
        }

        //Set the effect type
        if (baseCardEffectType == Effect.RANDOM)
        {
            //choose a random effect
            int temp = Random.Range(1, 4);
            effect = temp switch
            {
                1 => Effect.SHIELD,
                2 => Effect.HEAL,
                _ => Effect.DAMAGE
            };
        }
        else
        {
            effect = baseCardEffectType;
        }

        //set the effect value
        if (effectMinValue > eventMaxValue)
        {
            effectValue = effectMinValue;
        }
        else
        {
            effectValue = Random.Range(effectMinValue, eventMaxValue);
        }

        //Set up the event values / With the current events they cannot be changed from the base
        _event = baseCardEvent; _eventTrigger = baseEventTrigger; _eventTarget = baseCardEventTarget;
        //set the effect trigger since it cannot change
        effectTrigger = baseEffectTrigger;

        //Debug.Log("Effect:");
        //Debug.Log($"Effect Current: {effect} | Effect Base: {baseCardEffectType}");
        //Debug.Log($"Effect Trigger Current: {effectTrigger} | Effect Trigger Base: {baseEffectTrigger}");
        //Debug.Log($"Effect Target Current: {effectTarget} | Effect Target Base: {baseCardEffectTarget}");
        //Debug.Log("Event");
        //Debug.Log($"Event Current: {effect} | Event Base: {baseCardEffectType}");
        //Debug.Log($"Event Trigger Current: {_eventTrigger} | Event Trigger Base: {baseEventTrigger}");
        //Debug.Log($"Event Target Current: {effectTarget} | Event Target Base: {baseCardEffectTarget}");

        if(NameTextbox != null)
        {
            NameTextbox.text = CardName;
        }
        else
        {
            //Debug.Log("No Name Display");
        }
        if(ImageBox != null)
        {
            ImageBox.sprite = Image;
        }
        else
        {
            //Debug.Log("No Image Display");
        }
        //Debug.Log("Init Finished");
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
    public void PlayCard(Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer)
    {
        if (effectTrigger == Trigger.ON_PLAY)
        {
            TriggerEffect(player, AI, GM, PlayedByPlayer);
        }
        if (_eventTrigger == Trigger.ON_PLAY)
        {
            TriggerEvent(player, AI, GM, PlayedByPlayer);
        }
    }
    /// <summary>
    /// Called at the end of the round, triggers events or effects that trigger at this point
    /// </summary>
    public void CardInHand(Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer)
    {
        if (effectTrigger == Trigger.IN_HAND)
        {
            TriggerEffect(player, AI, GM, PlayedByPlayer);
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
        if (effectTrigger == Trigger.START_OF_GAME)
        {
            TriggerEffect(player, AI, GM, PlayedByPlayer);
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
    private void TriggerEffect(Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer)
    {
        switch (effect)
        {
            case Effect.DAMAGE:
                TriggerDamageEffect(player, AI, GM, PlayedByPlayer);
                break;
            case Effect.SHIELD:
                TriggerShieldEffect(player, AI, GM, PlayedByPlayer);
                break;
            case Effect.HEAL:
                TriggerHealEffect(player, AI, GM, PlayedByPlayer);
                break;
            case Effect.DRAW:
                TriggerDrawEffect(player, AI, GM, PlayedByPlayer);
                break;
            default:
                throw new System.Exception("You should never reach this point");
        }
    }
    /// <summary>
    /// Called if the cards effect should trigger
    /// </summary>
    private void TriggerEvent(Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer)
    {
        //If the event is already ongoing, remove it and replace it with the newer one
        if (GM.OngoingEvents.TryGetValue(_event, out int CurrentEventDuration))
        {
            if (eventDuration > CurrentEventDuration)
            {
                GM.OngoingEvents.Remove(_event);
                GM.OngoingEvents.Add(_event, eventDuration);
                //This might also work, I'll test it later
                //GM.OngoingEvents[_event] = eventDuration;
            }
        }
        else
        {
            GM.OngoingEvents.Add(_event, eventDuration);
        }
    }

    //"Extra" Private Methods
    /// <summary>
    /// Triggers the damage effect
    /// </summary>
    private void TriggerDamageEffect(Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer)
    {
        switch (effectTarget)
        {
            case Target.SELF:
                if(PlayedByPlayer)
                {
                    player.UpdateStored(damageValue: effectValue);
                }
                else
                {
                    AI.UpdateStored(damageValue: effectValue);
                }
                break;
            case Target.OPPONENT:
                if (PlayedByPlayer)
                {
                    AI.UpdateStored(damageValue: effectValue);
                }
                else
                {
                    player.UpdateStored(damageValue: effectValue);
                }
                break;
            case Target.BOTH:
                AI.UpdateStored(damageValue: effectValue);
                player.UpdateStored(damageValue: effectValue);
                break;
            default:
                throw new System.Exception("Effect Target was not Both, Self, or Opponent");
        }
    }
    /// <summary>
    /// Triggers the damage effect
    /// </summary>
    private void TriggerShieldEffect(Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer)
    {
        switch (effectTarget)
        {
            case Target.SELF:
                if (PlayedByPlayer)
                {
                    player.UpdateStored(shieldValue: effectValue);
                }
                else
                {
                    AI.UpdateStored(shieldValue: effectValue);
                }
                break;
            case Target.OPPONENT:
                if (PlayedByPlayer)
                {
                    AI.UpdateStored(shieldValue: effectValue);
                }
                else
                {
                    player.UpdateStored(shieldValue: effectValue);
                }
                break;
            case Target.BOTH:
                AI.UpdateStored(shieldValue: effectValue);
                player.UpdateStored(shieldValue: effectValue);
                break;
            default:
                throw new System.Exception("Effect Target was not Both, Self, or Opponent");
        }
    }
    /// <summary>
    /// Triggers the damage effect
    /// </summary>
    private void TriggerHealEffect(Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer)
    {
        switch (effectTarget)
        {
            case Target.SELF:
                if (PlayedByPlayer)
                {
                    player.UpdateStored(healValue: effectValue);
                }
                else
                {
                    AI.UpdateStored(healValue: effectValue);
                }
                break;
            case Target.OPPONENT:
                if (PlayedByPlayer)
                {
                    AI.UpdateStored(healValue: effectValue);
                }
                else
                {
                    player.UpdateStored(healValue: effectValue);
                }
                break;
            case Target.BOTH:
                AI.UpdateStored(healValue: effectValue);
                player.UpdateStored(healValue: effectValue);
                break;
            default:
                throw new System.Exception("Effect Target was not Both, Self, or Opponent");
        }
    }
    /// <summary>
    /// Triggers the damage effect
    /// </summary>
    private void TriggerDrawEffect(Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer)
    {
        switch (effectTarget)
        {
            case Target.SELF:
                Debug.Log("Draw Effect on Self");
                break;
            case Target.OPPONENT:
                Debug.Log("Draw Effect on Opponent");
                break;
            case Target.BOTH:
                Debug.Log("Draw Effect on Both");
                break;
            default:
                throw new System.Exception("Effect Target was not Both, Self, or Opponent");
        }
    }
}
