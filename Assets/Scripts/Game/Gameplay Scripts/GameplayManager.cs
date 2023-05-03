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
    [SerializeField] public GameObject[] AllCardsList;

    //Health
    [SerializeField] int PlayerStartingHealth = 20;

    [SerializeField] GameplayUI UI;
    Card currentPlayerOneSelected = null;

    //starting decks
    public Deck playerOneDeck;
    public Deck playerTwoDeck;
    public Player player { get; private set;}
    public EnemyAI opponent { get; private set; }

    //Event, Duration
    [HideInInspector] public List<EventDictionary> OngoingEvents;

    void Start()
    {
        /*Optional*/GameplayDebug.GiveCardsID(AllCardsList);
        //Initalize
        OngoingEvents = new();
        playerOneDeck.Init();
        playerTwoDeck.Init();
        player = new Player(playerOneDeck, PlayerStartingHealth, this);
        opponent = new EnemyAI(playerTwoDeck, PlayerStartingHealth, this, player);
        //set inital state
        currentGameState = CurrentMode.WAITING;
        GameplayEventManager.CheckStartOfGameEffects(this);
        UI.Init(AllCardsList);
    }

    //Methods
    public void Continue()
    {
        string setButtonText = "";
        switch (currentGameState)
        {
            case CurrentMode.WAITING:
                //set starting display
                UI.UpdateDisplay(player, opponent, AllCardsList);
                currentGameState = CurrentMode.SELECTING;
                setButtonText = "Lock In";
                break;
            case CurrentMode.SELECTING:
                if (currentPlayerOneSelected != null)
                {
                    currentGameState = CurrentMode.ACTIVATING;
                    Continue();
                    setButtonText = "Next Round";
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
        Card opponentCard = opponent.Play();
        UI.DrawSelectedCard(opponentCard.CardID, PlayerOption.PLAYER_TWO);
        //Trigger the cards
        (string TopTextReturn, string BotTextReturn) = Random.Range(0.0f, 1.0f) > .5f ?
            (currentPlayerOneSelected.PlayCard(player, opponent, this, true, Trigger.ON_PLAY), opponentCard.PlayCard(player, opponent, this, false, Trigger.ON_PLAY)):
            (opponentCard.PlayCard(player, opponent, this, false, Trigger.ON_PLAY), currentPlayerOneSelected.PlayCard(player, opponent, this, true, Trigger.ON_PLAY));

        EndOfTurn();
        UI.UpdateDisplay(player, opponent, AllCardsList, TopTextReturn, BotTextReturn);
    }
    private void EndOfTurn()
    {
        player.RemoveCard(currentPlayerOneSelected);
        GameplayEventManager.CheckInHandEffects(this);
        player.Draw();
        opponent.Draw();
        player.TriggerStored(PlayerOption.PLAYER_ONE);
        opponent.TriggerStored(PlayerOption.PLAYER_TWO);
        GameplayEventManager.UpdateEvents(OngoingEvents);
        currentPlayerOneSelected = null;
    }
    private void GameOver()
    {
        string WinnerDisplay = GameplayValidator.GetWinner(OngoingEvents, player, opponent);
        if (WinnerDisplay == "You Win!")
        {
            int reward = Random.Range(victoryRewardMin, victoryRewardMax + 1);
            UI.UpdateDisplay(player, opponent, AllCardsList, WinnerDisplay, "Gain " + reward + " Money");
            Inventory.Instance.UpdateFunds(reward);
        }
        else
            UI.UpdateDisplay(player, opponent, AllCardsList, WinnerDisplay);
        UI.ClearHand();
    }
}
