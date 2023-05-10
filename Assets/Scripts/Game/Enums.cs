using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums
{
    //All Cards
    public enum Target
    {
        SELF_HEALTH,
        OPPONENT_HEALTH,
        RANDOM_HEALTH,
        BOTH_HEALTH,

        SELF_HAND,
        SELF_DECK,
        OPPONENT_HAND,
        OPPONENT_DECK,
        BOTH_HAND,
        BOTH_DECK,

        SWAP_DECK,
        SWAP_HAND,
        SWAP_HEALTH,

        NONE
    }
    public enum Trigger
    {
        START_OF_GAME,
        IN_HAND,
        ON_PLAY,
        NONE
    }  
    
    //Some Cards
    public enum _Event
    {
        NO_HEALS,
        NO_SHIELDS,
        NO_DRAW,
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
    public enum PlayerOption
    {
        PLAYER_ONE,
        PLAYER_TWO,
        BOTH,
        NONE           
    }
    
    //Gameplay
    public enum CurrentMode
    {
        WAITING,
        SELECTING,
        ACTIVATING,
        ENDING,
        GAMEOVER
    }
    
    //Card
    public enum CardType
    {
        GATCHA,
        GAMEPLAY,
        UNKNOWN
    }
    public enum Rarity
    {
        COMMON,
        UNCOMMON,
        RARE,
        LEGENDARY,
        NONE
    }

    //Methods
    public static string GetEnumAsString(string val)
    {
        return val[..1] + val[1..].ToLower();
    }
    
    //Removed
    //public enum CardEffectTarget
    //{
    //    SELF_HAND,
    //    SELF_DECK,
    //    OPPONENT_HAND,
    //    OPPONENT_DECK,
    //    BOTH_HAND,
    //    BOTH_DECK,
    //    NONE
    //}
    //public enum SwapTarget
    //{
    //    DECK,
    //    HAND,
    //    HEALTH,
    //    NONE
    //}
}
