using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums
{
    public enum Target
    {
        SELF,
        OPPONENT,
        RANDOM,
        BOTH,
        NONE
    }
    public enum _Event
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
        NONE
    }
    public enum Trigger
    {
        START_OF_GAME,
        IN_HAND,
        ON_PLAY,
        NONE
    }  
    public enum CardEffectTarget
    {
        SELF_HAND,
        SELF_DECK,
        OPPONENT_HAND,
        OPPONENT_DECK,
        BOTH_HAND,
        BOTH_DECK,
        NONE
    }
    public enum CurrentMode
    {
        WAITING,
        SELECTING,
        ACTIVATING,
        ENDING,
        GAMEOVER
    }

    public enum SwapTarget
    {
        DECK,
        HAND,
        HEALTH,
        NONE
    }
}
