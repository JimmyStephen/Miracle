using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class NewCard : MonoBehaviour
{
    enum CardType
    {
        DAMAGE,
        SHIELD,
        HEAL,
        RANDOM,
        EFFECT
    }
    public enum Target
    {
        SELF,
        OPPONENT,
        RANDOM,
        BOTH,
        NONE
    }
    public enum Event
    {
        NO_HEALS,
        NO_SHIELDS,
        LIMITED_DECK,
        UN_OH,
        NONE
    }
    public enum Effect
    {
        DAMAGE,
        SHIELD,
        HEAL,
        RANDOM,
        DRAW,
        EVENT,
        NONE
    }
    public enum Triggers
    {
        START_OF_GAME,
        IN_HAND,
        ON_PLAY,
        NONE
    }

    [Header("How to Display")]
    [SerializeField] TMPro.TMP_Text NameTextbox;
    [SerializeField] TMPro.TMP_Text DescTextbox;
    [SerializeField] Image ImageBox;


    [Header("Card Effects")]
    [SerializeField] string CardName;
    [SerializeField] string CardDescription;
    [SerializeField] CardType cardType;

    [SerializeField] Target cardEffectTarget = Target.NONE;
    [SerializeField] Target cardEventTarget = Target.NONE;

    [SerializeField] Effect cardEffectType = Effect.NONE;
    [SerializeField] Triggers effectTriggers = Triggers.NONE;
    [SerializeField] int effectMinValue = 0;
    [SerializeField, Tooltip("If lower than effectMinValue, always takes min")] int eventMaxValue = -1;

    [SerializeField] Event cardEvent = Event.NONE;
    [SerializeField] Triggers eventTrigger = Triggers.NONE;
    [SerializeField] int eventDuration = 0;

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

        if (effectMinValue > eventMaxValue)
        {
            effectValue = effectMinValue;
        }
        else
        {
            effectValue = Random.Range(effectMinValue, eventMaxValue);
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





    //New Stuff

    /// <summary>
    /// What happens when this card is played
    /// </summary>
    public void PlayCard(Player player, EnemyAI opponent, GameplayManager GM)
    {
        if(eventTrigger == Triggers.ON_PLAY)
        {
            AddEvent(GM);
        }
        else if(effectTriggers == Triggers.ON_PLAY)
        {
            TriggerEffect(player, opponent, GM, false);
        }
    }

    /// <summary>
    /// If this adds an event at the start of the game
    /// </summary>
    public Event StartOfGameEffect()
    {
        return Event.NONE;
    }

    /// <summary>
    /// What happens every turn while this is in your hand
    /// </summary>
    public void InHandEffect(ref bool hasEffect, ref Target target, ref int selfEffect, ref int opponentEffect)
    {
        if(eventTrigger != Triggers.IN_HAND)
        {
            //event has a trigger
            hasEffect = true;
            target = cardEventTarget;
            if(target == Target.SELF)
            {
                
            }
            else if ( target == Target.OPPONENT)
            {

            }
            else
            {

            }
        }
        else if (effectTriggers != Triggers.IN_HAND)
        {
            //effect has a trigger
            hasEffect = true;
            target = cardEffectTarget;
        }
        else
        {
            hasEffect = false;
        }
    }


    public void TriggerEffect(Player player, EnemyAI AI, GameplayManager GM, bool isPlayer)
    {
        switch (cardEffectType)
        {
            case Effect.DAMAGE:
                break;
            case Effect.SHIELD:
                break;
            case Effect.HEAL:
                break;
            case Effect.RANDOM:
                break;
            case Effect.DRAW:
                break;
            case Effect.EVENT:
                AddEvent(GM);
                break;
            default:
                break;
        }
    }

    private void ApplyEffect(Player player, EnemyAI AI, bool isPlayer)
    {
        switch (cardEffectTarget)
        {
            case Target.SELF:
                if (isPlayer)
                {
                    //effect player
                }
                else
                {
                    //effect AI
                }
                break;
            case Target.OPPONENT:
                if (isPlayer)
                {
                    //effect AI
                }
                else
                {
                    //effect Player
                }
                break;
            case Target.RANDOM:
                //choose a random target
                break;
            case Target.BOTH:
                if (isPlayer)
                {
                    //effect self (player)
                    //effect opponent (AI)
                }
                else
                {
                    //effect self (AI)
                    //effect opponent (player)
                }
                break;
            default:
                break;
        }
    }

    public void AddEvent(GameplayManager GM)
    {
        //GM.OngoingEvents.Add(cardEvent, eventDuration);
    }

}
