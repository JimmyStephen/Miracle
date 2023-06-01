using System;
using UnityEngine;
using static Enums;

[Serializable]
public struct Data
{
    public TwoValue DamageValue;
    public TwoValue HealValue;
    public TwoValue ShieldValue;
}
[Serializable]
public struct DrawDiscard
{
    [Range(0, 25)] public int ToDraw;
    [Range(0, 25)] public int ToDiscard;
}
[Serializable]
public struct TwoValue
{
    [Range(0, 25)] public int min;
    [Range(0, 25)] public int max;
    private int? value;
    public int GetValue()
    {
        if (value == null)
        {
            if (min >= max)
                value = min;
            else
                value = UnityEngine.Random.Range(min, max+1);
        }
        return (int)value;
    }
}

public struct EventDictionary
{
    public _Event EventType;
    public PlayerOption EventTarget;
    public int EventDuration;

    public EventDictionary(_Event Type, PlayerOption _target, int Duration)
    {
        EventType = Type; EventTarget = _target; EventDuration = Duration;
    }

    public void UpdateDuration()
    {
        EventDuration--;
    }
}
