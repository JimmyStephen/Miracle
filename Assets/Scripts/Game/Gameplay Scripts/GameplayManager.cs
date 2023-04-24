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
    [SerializeField] int PlayerOneStartingHealth = 20;
    [SerializeField] int PlayerTwoStartingHealth = 20;

    //Display
    [SerializeField] TMPro.TMP_Text playerOneHealthDisplay;
    [SerializeField] TMPro.TMP_Text playerTwoHealthDisplay;
    [SerializeField] GameObject playerOneChosenDisplay;
    [SerializeField] GameObject playerTwoChosenDisplay;
    [SerializeField] GameObject playerOneHandDisplay;
    [SerializeField] TMPro.TMP_Text TopText;
    [SerializeField] TMPro.TMP_Text BotText;
    List<GameObject> currentHandGameObjects;
    private GameObject playerOneDisplay;
    private GameObject playerTwoDisplay;
    //End Display

    //current card
    Card currentPlayerOneSelected = null;

    //starting decks
    public Deck playerOneDeck;
    public Deck playerTwoDeck;
    public Player player { get; private set;}
    public EnemyAI opponent { get; private set; }

    //Event, Duration
    [HideInInspector] public List<EventDictionary> OngoingEvents;

    // Start is called before the first frame update
    void Start()
    {
        //Give cards ID's
        int id = 0;
        foreach(var v in AllCardsList)
        {
            v.GetComponent<Card>().CardID = id++;
        }

        //Initalize Lists
        currentHandGameObjects = new();
        OngoingEvents = new();

        //initalize decks
        playerOneDeck.Init();
        playerTwoDeck.Init();
        player = new Player(playerOneDeck, PlayerOneStartingHealth, this);
        opponent = new EnemyAI(playerTwoDeck, PlayerTwoStartingHealth, this);
        //set inital state
        currentGameState = CurrentMode.WAITING;
        CheckStartOfGameEffects();
        player.TriggerStored(PlayerOption.PLAYER_ONE);
        opponent.TriggerStored(PlayerOption.PLAYER_TWO);
        //Set inital display
        playerOneHealthDisplay.text = "";
        playerTwoHealthDisplay.text = "";
        TopText.text = "";
        BotText.text = "";
    }

    //Methods
    public void SetSelectedCard(int CardNumber)
    {
        if (currentGameState != CurrentMode.SELECTING) return;
        currentPlayerOneSelected = AllCardsList[CardNumber].GetComponent<Card>();
        if (playerOneDisplay != null) Destroy(playerOneDisplay);
        playerOneDisplay = Instantiate(AllCardsList[currentPlayerOneSelected.CardID], playerOneChosenDisplay.transform);
    }
    public void Continue()
    {
        string setButtonText = "";
        switch (currentGameState)
        {
            case CurrentMode.WAITING:
                //set starting display
                UpdateDisplay();
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
                {
                    setButtonText = "Lock In";
                }
                break;
            case CurrentMode.ACTIVATING:
                PlayCards();
                currentGameState = CurrentMode.ENDING;
                setButtonText = "Next Round";
                break;
            case CurrentMode.ENDING:
                Destroy(playerOneDisplay);
                Destroy(playerTwoDisplay);
                if (CheckWinner())
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
    private void PlayCards()
    {
        string TopTextReturn;
        string BotTextReturn;

        //display and get cards
        playerTwoDisplay = Instantiate(AllCardsList[opponent.Play().CardID], playerTwoChosenDisplay.transform);
        Card playerCard = playerOneDisplay.GetComponent<Card>();
        Card opponentCard = playerTwoDisplay.GetComponent<Card>();

        //Remove the cards from hand
        currentPlayerOneSelected = null;
        player.RemoveCard(playerCard);

        //Trigger the cards
        if (Random.Range(0.0f, 1.0f) > .5f)
        {
            TopTextReturn = playerCard.PlayCard(player, opponent, this, true, Trigger.ON_PLAY);
            BotTextReturn = opponentCard.PlayCard(player, opponent, this, false, Trigger.ON_PLAY);
        }
        else
        {
            BotTextReturn = opponentCard.PlayCard(player, opponent, this, false, Trigger.ON_PLAY);
            TopTextReturn = playerCard.PlayCard(player, opponent, this, true, Trigger.ON_PLAY);
        }

        //Trigger end of turn effects
        CheckInHandEffects();

        //draw a new card for the player
        player.Draw();
        opponent.Draw();

        //Debug
        //GameplayDebug.OutputEvents(_OngoingEvents);

        //Trigger all stored variables
        player.TriggerStored(PlayerOption.PLAYER_ONE);
        opponent.TriggerStored(PlayerOption.PLAYER_TWO);
        UpdateEvents();
        //Set the displays
        UpdateDisplay(TopTextReturn, BotTextReturn);
    }

    //Called if the game is over
    private void GameOver()
    {
        ClearHand();
        string WinnerDisplay = GetWinner();
        if (WinnerDisplay == "You Win!")
        {
            int reward = Random.Range(victoryRewardMin, victoryRewardMax + 1);
            UpdateDisplay(WinnerDisplay, "Gain " + reward + " Money");
            Inventory.Instance.UpdateFunds(reward);
        }
        else
            UpdateDisplay(WinnerDisplay);
    }

    //Check if the game should be over (Move to GameplayValidator)
    private bool CheckWinner()
    {
        return (player.CurrentHealth <= 0 || opponent.CurrentHealth <= 0 || CheckUNO());
    }
    private bool CheckUNO()
    {
        if (CheckForEvent(_Event.UN_OH, PlayerOption.BOTH))
            return (player.Hand.Count == 0 || opponent.Hand.Count == 0);
        return false;
    }
    private string GetWinner()
    {
        if ((player.CurrentHealth <= 0 && opponent.CurrentHealth <= 0) || (CheckUNO() && player.Hand.Count == 0 && opponent.Hand.Count == 0))
            return "Draw";
        else if (player.CurrentHealth <= 0 || (CheckUNO() && opponent.Hand.Count == 0))
            return "AI Wins";
        else
            return "You Win!";
    }

    //Display Helper Methods (Mode to GameplayUI)
    private void UpdateDisplay(string TopText = "", string BotText = "")
    {
        playerOneHealthDisplay.text = player.GetHealthDisplay();
        playerTwoHealthDisplay.text = opponent.GetHealthDisplay();
        this.TopText.text = TopText;
        this.BotText.text = BotText;
        UpdatePlayerHandDisplay();
    }
    private void ClearHand()
    {
        currentHandGameObjects.ForEach(go => { Destroy(go); });
        currentHandGameObjects.Clear();
    }
    private void UpdatePlayerHandDisplay()
    {
        //Delete the entire hand
        ClearHand();
        //Redraw the hand
        player.Hand.ForEach(card => { currentHandGameObjects.Add(Instantiate(AllCardsList[card.CardID], playerOneHandDisplay.transform)); });
    }

    //Event Helper Methods (Move to GameplayEventManager)
    /// <summary>
    /// Checks if the event exists in the current game
    /// </summary>
    /// <param name="SearchEvent">What event to search for</param>
    /// <param name="EffectedPlayer">Who is effected by the event</param>
    /// <returns></returns>
    public bool CheckForEvent(Enums._Event SearchEvent, Enums.PlayerOption EffectedPlayer)
    {
        foreach (var _event in OngoingEvents)
        {
            if (_event.EventType == SearchEvent && _event.EventTarget == EffectedPlayer)
            {
                return true;
            }
        }
        return false;
    }
    private void UpdateEvents()
    {
        List<EventDictionary> temp = new();
        OngoingEvents.ForEach(evnt => {
            if (evnt.EventDuration - 1 >= 0)
                temp.Add(new(evnt.EventType, evnt.EventTarget, evnt.EventDuration-1));
            else
                Debug.Log($"{evnt.EventType} Removed");
        });
        OngoingEvents = temp;
    }
    private void CheckStartOfGameEffects()
    {
        //Check each hand and trigger any end of turn effects
        playerOneDeck.GetStartingDeck().ForEach(card => { card.PlayCard(player, opponent, this, true, Trigger.START_OF_GAME); });
        playerTwoDeck.GetStartingDeck().ForEach(card => { card.PlayCard(player, opponent, this, false, Trigger.START_OF_GAME); });
    }
    private void CheckInHandEffects()
    {
        //Check each hand and trigger any end of turn effects
        player.Hand.ForEach(card => { card.PlayCard(player, opponent, this, true, Trigger.IN_HAND); });
        opponent.Hand.ForEach(card => { card.PlayCard(player, opponent, this, false, Trigger.IN_HAND); });
    }
}
