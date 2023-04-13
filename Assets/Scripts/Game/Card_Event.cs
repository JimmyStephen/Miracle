using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

[Serializable]
public class Card_Event
{
    [Header("Event Effects"), Tooltip("Events are duration based and will continue to effect the game until the duration runs out")]
    [SerializeField] Target baseCardEventTarget = Target.NONE;
    [SerializeField] Trigger baseEventTrigger = Trigger.NONE;
    [SerializeField] _Event baseCardEvent = _Event.NONE;
    [SerializeField] int eventDuration = 0;

    //the actual event values, in case they got changed from the base in some way
    public _Event _event { get; private set; }
    public Trigger _eventTrigger {get; private set;}
    public Target _eventTarget {get; private set;}
    private string OnPlayText;

    /// <summary>
    /// Initalizes the Effect so that it can be called later without issue
    /// </summary>
    public void _Init(string Text)
    {
        _event = baseCardEvent;
        _eventTrigger = baseEventTrigger;
        _eventTarget = baseCardEventTarget;
        OnPlayText = Text;
    }

    /// <summary>
    /// Called if the cards effect should trigger
    /// </summary>
    public string TriggerEvent(Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer)
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
        return OnPlayText;
    }
}
