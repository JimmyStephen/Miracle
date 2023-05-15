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
    //[SerializeField] public GameObject[] AllCardsList;

    //Health
    [SerializeField] int PlayerStartingHealth = 20;

    [SerializeField] GameplayUI UI;
    Gameplay_Card currentPlayerOneSelected = null;

    //starting decks
    public Deck playerOneDeck;
    public Deck playerTwoDeck;
    public Player player { get; private set;}
    public EnemyAI opponent { get; private set; }

    //Event, Duration
    [HideInInspector] public List<EventDictionary> OngoingEvents;

    void Start()
    {
        ///*Optional*/GameplayDebug.GiveCardsID(AllCardsList);
        //Initalize
        OngoingEvents = new();
        playerTwoDeck.Init();
        playerOneDeck.Init();
        Deck customDeck = gameObject.AddComponent<Deck>();
        //customDeck.SetStartingDeck(GameManager.Instance.CustomDeck.GetStartingDeck().ToArray());
        //playerOneDeck = customDeck;
        //playerOneDeck.Init();
        player = new Player(playerOneDeck, PlayerStartingHealth, this);
        opponent = new EnemyAI(playerTwoDeck, PlayerStartingHealth, this, player);
        //set inital state
        currentGameState = CurrentMode.WAITING;
        GameplayEventManager.CheckStartOfGameEffects(this);
        UI.Init();
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
        GameplayEventManager.UpdateEvents(OngoingEvents);
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
        UI.ClearHand();
    }
}
