using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
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
    List<GameObject> currentHandGameObjects;

    [SerializeField] TMPro.TMP_Text TopText;
    [SerializeField] TMPro.TMP_Text BotText;

    private GameObject playerOneDisplay;
    private GameObject playerTwoDisplay;
    //End Display

    //current card
    Card currentPlayerOneSelected = null;

    //starting decks
    [SerializeField] Deck playerOneDeck;
    [SerializeField] Deck playerTwoDeck;

    private Player player;
    private EnemyAI opponent;

    //Event, Duration
    [HideInInspector] public Dictionary<Enums._Event, int> OngoingEvents = new Dictionary<Enums._Event, int>();

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
        currentHandGameObjects = new List<GameObject>();
        //initalize decks
        playerOneDeck.Init();
        playerTwoDeck.Init();
        player = new Player(playerOneDeck, PlayerOneStartingHealth, this);
        opponent = new EnemyAI(playerTwoDeck, PlayerTwoStartingHealth, this);
        //set inital state
        currentGameState = CurrentMode.WAITING;
        CheckStartOfGameEffects();
        player.TriggerStored();
        opponent.TriggerStored();
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
    
    //Helper Methods
    public bool CheckForEvent(Enums._Event search)
    {
        foreach (var _event in OngoingEvents)
        {
            if (_event.Key == search)
                return false;
        }
        return true;
    }

    //private
    private void PlayCards()
    {
        string TopTextReturn;
        string BotTextReturn;
        //Debug Hand
        Debug.Log("Pre Play");
        GameplayDebug.OutputHands(player, opponent);

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
        //Debug Hand
        Debug.Log("Post Play");
        GameplayDebug.OutputHands(player, opponent);

        //Trigger end of turn effects
        CheckInHandEffects();

        //draw a new card for the player
        player.Draw();
        opponent.Draw();

        //Debug Hand
        Debug.Log("Pre Draw");
        GameplayDebug.OutputHands(player, opponent);

        //Trigger all stored variables
        player.TriggerStored();
        opponent.TriggerStored();

        //Set the displays
        UpdateDisplay(TopTextReturn, BotTextReturn);
    }

    //Triggers
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

    //Check if the game should be over
    private bool CheckWinner()
    {
        return (player.CurrentHealth <= 0 || opponent.CurrentHealth <= 0);
    }
    private string GetWinner()
    {
        if (player.CurrentHealth <= 0 && opponent.CurrentHealth <= 0)
            return "Draw";
        else if (player.CurrentHealth <= 0)
            return "AI Wins";
        else
            return "You Win!";
    }
    private void GameOver()
    {
        //Delete all the cards in hand
        ClearHand();

        if (GetWinner() == "You Win!")
        {
            int reward = Random.Range(victoryRewardMin, victoryRewardMax + 1);
            UpdateDisplay("You Win!", "Gain " + reward + " Money");
            Inventory.Instance.UpdateFunds(reward);
        }
        else
            UpdateDisplay("AI Wins");
    }

    //Helper Methods
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
}
