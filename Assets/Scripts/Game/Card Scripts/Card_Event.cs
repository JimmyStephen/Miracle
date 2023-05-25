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
    public Trigger EffectTrigger {get; private set;}
    public Target CardTarget { get; private set;}
    public string EffectType { get; private set; }
    private string OnPlayText;

    /// <summary>
    /// Initalizes the Effect so that it can be called later without issue
    /// </summary>
    public void _Init(string Text)
    {
        _event = baseCardEvent;
        EffectTrigger = baseEventTrigger;
        CardTarget = baseCardEventTarget;
        EffectType = baseCardEvent.ToString();
        OnPlayText = Text;
    }

    /// <summary>
    /// If the event is already ongoing, remove it and replace it with the newer one otherwise add the event to the list of ongoing events
    /// </summary>
    public string TriggerEffect(Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer)
    {
        EventDictionary EvntDict = new(_event, GetEventTarget(PlayedByPlayer), eventDuration);
        //Debug.Log(Enums.GetEnumAsString(EvntDict.EventType.ToString()) + " Adding");
        int contains = CheckContains(GM.OngoingEvents, EvntDict);
        if (contains != -100)
        {
            //Debug.Log(Enums.GetEnumAsString(EvntDict.EventType.ToString()) + " is already in the list");
            if(contains < eventDuration)
            {
                GM.OngoingEvents.Remove(GetEvent(GM.OngoingEvents, EvntDict));
                GM.OngoingEvents.Add(EvntDict);
            }
        }
        else
            GM.OngoingEvents.Add(EvntDict);
        //Debug.Log(Enums.GetEnumAsString(EvntDict.EventType.ToString()) + " Added");
        return OnPlayText;
    }

    private int CheckContains(List<EventDictionary> events, EventDictionary toFind)
    {
        foreach (var evnt in events)
            if (evnt.EventType == toFind.EventType && evnt.EventTarget == toFind.EventTarget) { return evnt.EventDuration; }
        return -100;
    }

    private EventDictionary GetEvent(List<EventDictionary> events, EventDictionary toFind)
    {
        foreach (var evnt in events)
            if (evnt.EventType == toFind.EventType && evnt.EventTarget == toFind.EventTarget) { return evnt; }
        return events[events.Count+1];
    }

    private PlayerOption GetEventTarget(bool playedByPlayer)
    {
        if (playedByPlayer)
        {
            if (CardTarget == Target.SELF_HEALTH)
                return PlayerOption.PLAYER_ONE;
            if (CardTarget == Target.OPPONENT_HEALTH)
                return PlayerOption.PLAYER_TWO;
            return PlayerOption.BOTH;
        }
        if (CardTarget == Target.SELF_HEALTH)
            return PlayerOption.PLAYER_TWO;
        if (CardTarget == Target.OPPONENT_HEALTH)
            return PlayerOption.PLAYER_ONE;
        return PlayerOption.BOTH;
    }
}
