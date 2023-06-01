using System.Collections.Generic;
using UnityEngine;
using static Enums;

//make singleton
public class GameplayManager : MonoBehaviour
{
    //current state
    CurrentMode currentGameState;

    [SerializeField] int victoryRewardMin = 100;
    [SerializeField] int victoryRewardMax = 1000;
    [SerializeField] TMPro.TMP_Text buttonText;

    //Health
    [SerializeField] int PlayerStartingHealth = 20;

    [SerializeField] GameplayUI UI;

    //decks & players
    public Deck[] AIDecks;
    public Player player { get; private set;}
    public EnemyAI opponent { get; private set; }
    
    [HideInInspector] public Deck playerOneDeck;
    [HideInInspector] public Deck playerTwoDeck;
    private Gameplay_Card currentPlayerOneSelected = null;

    //Event, Duration
    [HideInInspector] public List<EventDictionary> OngoingEvents;

    private List<int>[] AIDecksInt = new List<int>[]
    {
        new() { 2, 2, 3, 3, 11, 11, 12, 12, 16, 16, 21, 21, 24, 24, 28 },    //Draw
        new() { 1, 1, 7, 7, 13, 10, 10, 14, 24, 24 },        //Aggro
        new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }, //Default
        new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }, //Default
        new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }  //Default
    };
    void Start()
    {
        Debug.Log("Start");
        //Init variables & players
        OngoingEvents = new();
        UI.Init();

        playerOneDeck.SetStartingDeck(CardConnector.GetGameplayCards(Inventory.Instance.GetSelectedDeck().Cards));
        playerOneDeck.Init();

        //playerTwoDeck.SetStartingDeck(CardConnector.GetGameplayCards(new() { 2, 2, 3, 3, 11, 11, 12, 12, 16, 16, 28 }));
        int AIDeckSelected = Random.Range(0, AIDecksInt.Length);
        playerTwoDeck.SetStartingDeck(CardConnector.GetGameplayCards(AIDecksInt[AIDeckSelected]));
        Debug.Log("Deck Selected: " + AIDeckSelected);
        //playerTwoDeck.SetStartingDeck(CardConnector.GetGameplayCards(AIDecksInt[Random.Range(0, AIDecksInt.Length)]));
        playerTwoDeck.Init();

        player = new Player(playerOneDeck, PlayerStartingHealth, this);
        opponent = new EnemyAI(playerTwoDeck, PlayerStartingHealth, this, player);
        
        currentGameState = CurrentMode.WAITING; //set inital game state
        GameplayEventManager.CheckStartOfGameEffects(this);
    }

    //Methods
    public void Continue()
    {
        string setButtonText = "";
        switch (currentGameState)
        {
            case CurrentMode.WAITING:
                //set starting display
                UI.UpdateDisplay(player, opponent);
                currentGameState = CurrentMode.SELECTING;
                setButtonText = "Lock In";
                break;
            case CurrentMode.SELECTING:
                if (currentPlayerOneSelected != null || player.Hand.Count == 0)
                {
                    currentGameState = CurrentMode.ACTIVATING;
                    Continue();
                    setButtonText = "Lock In";
                }
                else
                    setButtonText = "Lock In";
                break;
            case CurrentMode.ACTIVATING:
                PlayCards();
                currentGameState = CurrentMode.ENDING;
                setButtonText = "Next Round";
                break;
            case CurrentMode.ENDING:
                UI.EraseCard(PlayerOption.BOTH);
                if (GameplayValidator.CheckWinner(OngoingEvents, player, opponent))
                {
                    currentGameState = CurrentMode.GAMEOVER;
                    GameOver();
                    setButtonText = "Play Again";
                }
                else
                {
                    currentGameState = CurrentMode.SELECTING;
                    setButtonText = "Lock In";
                }
                break;
            case CurrentMode.GAMEOVER:
                setButtonText = "Start Game";
                Start();
                break;
            default:
                setButtonText = "Error";
                break;
        }
        buttonText.text = setButtonText;
    }

    public void SetSelectedCard(int CardNumber)
    {
        if (currentGameState != CurrentMode.SELECTING) return;
        currentPlayerOneSelected = UI.DrawSelectedCard(CardNumber, PlayerOption.PLAYER_ONE);
    }
    private void PlayCards()
    {
        if(currentPlayerOneSelected == null)
        {
            currentPlayerOneSelected = CardConnector.GetGameplayCard("Hand Empty!"); 
            UI.DrawSelectedCard(currentPlayerOneSelected.CardID, PlayerOption.PLAYER_ONE);
        }
        Gameplay_Card opponentCard = opponent.Play();
        UI.DrawSelectedCard(opponentCard.CardID, PlayerOption.PLAYER_TWO);
        //Trigger the cards
        string TopTextReturn, BotTextReturn;
        //(string TopTextReturn, string BotTextReturn) = Random.Range(0.0f, 1.0f) > .5f ?
        //    (currentPlayerOneSelected.PlayCard(player, opponent, this, true, Trigger.ON_PLAY), opponentCard.PlayCard(player, opponent, this, false, Trigger.ON_PLAY)):
        //    (opponentCard.PlayCard(player, opponent, this, false, Trigger.ON_PLAY), currentPlayerOneSelected.PlayCard(player, opponent, this, true, Trigger.ON_PLAY));
        if(Random.Range(0.0f, 1.0f) > .5f)
            (TopTextReturn, BotTextReturn) = (currentPlayerOneSelected.PlayCard(player, opponent, this, true, Trigger.ON_PLAY), opponentCard.PlayCard(player, opponent, this, false, Trigger.ON_PLAY));
        else
            (BotTextReturn, TopTextReturn) = (opponentCard.PlayCard(player, opponent, this, false, Trigger.ON_PLAY), currentPlayerOneSelected.PlayCard(player, opponent, this, true, Trigger.ON_PLAY));

        EndOfTurn();
        UI.UpdateDisplay(player, opponent, "Player: " + TopTextReturn, "AI: " + BotTextReturn);
    }
    private void EndOfTurn()
    {
        player.RemoveCard(currentPlayerOneSelected);
        GameplayEventManager.CheckInHandEffects(this);
        player.Draw();
        opponent.Draw();
        player.TriggerStored(PlayerOption.PLAYER_ONE);
        opponent.TriggerStored(PlayerOption.PLAYER_TWO);
        GameplayEventManager.UpdateEvents(this);
        currentPlayerOneSelected = null;
    }
    private void GameOver()
    {
        string WinnerDisplay = GameplayValidator.GetWinner(OngoingEvents, player, opponent);
        if (WinnerDisplay == "You Win!")
        {
            int reward = Random.Range(victoryRewardMin, victoryRewardMax + 1);
            UI.UpdateDisplay(player, opponent, WinnerDisplay, "Gain " + reward + " Money");
            Inventory.Instance.AddFunds(reward);
        }
        else
            UI.UpdateDisplay(player, opponent, WinnerDisplay);
        UI.ClearHands();
    }
}
