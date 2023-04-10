using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//make singleton
public class GameplayManager : MonoBehaviour
{
    //Waiting: Inital state
    //Selecting: Player is selecting the card
    //Activating: The cards are activating
    //Ending: Check if winner and pause before going to the next state
    //Game Over: When the game ends
    enum CurrentMode
    {
        WAITING,
        SELECTING,
        ACTIVATING,
        ENDING,
        GAMEOVER
    }
    //current state
    CurrentMode currentGameState;
    [SerializeField] int victoryRewardMin = 100;
    [SerializeField] int victoryRewardMax = 1000;
    [SerializeField] TMPro.TMP_Text buttonText;
    [SerializeField] GameObject[] AllCardsList;

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
        //Initalize Lists
        currentHandGameObjects = new List<GameObject>();

        //initalize decks
        playerOneDeck.Init();
        playerTwoDeck.Init();


        player = new Player(playerOneDeck, PlayerOneStartingHealth, this);
        opponent = new EnemyAI(playerTwoDeck, PlayerTwoStartingHealth, this);

        //set inital state
        currentGameState = CurrentMode.WAITING;
        StartOfGameEffects();
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
        playerOneDisplay = Instantiate(AllCardsList[currentPlayerOneSelected.index], playerOneChosenDisplay.transform);
    }
    public void Continue()
    {
        string setButtonText = "";
        switch (currentGameState)
        {
            case CurrentMode.WAITING:
                //set starting display
                //int count = 1;
                UpdatePlayerHandDisplay();
                playerOneHealthDisplay.text = player.GetHealthDisplay();
                playerTwoHealthDisplay.text = opponent.GetHealthDisplay();

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
                    string winner = GetWinner();
                    GameOver();
                    //Debug.Log(winner);
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


    //private
    private void PlayCards()
    {
        //display and get cards
        playerTwoDisplay = Instantiate(AllCardsList[opponent.Play().index], playerTwoChosenDisplay.transform);

        Card playerCard = playerOneDisplay.GetComponent<Card>();
        Card opponentCard = playerTwoDisplay.GetComponent<Card>();

        string TopTextReturn = playerCard.PlayCard(player, opponent, this, true);
        string BotTextReturn = opponentCard.PlayCard(player, opponent, this, false);

        //set the selected to null
        currentPlayerOneSelected = null;

        //remove card from players hand
        player.RemoveCard(playerCard);

        //Trigger end of turn effects
        EndOfTurnEffects();

        //draw a new card for the player
        player.Draw();

        //Trigger all stored variables
        player.TriggerStored();
        opponent.TriggerStored();

        //Set the displays
        TopText.text = TopTextReturn;
        BotText.text = BotTextReturn;
        playerOneHealthDisplay.text = player.GetHealthDisplay();
        playerTwoHealthDisplay.text = opponent.GetHealthDisplay();
        UpdatePlayerHandDisplay();
    }

    private void UpdatePlayerHandDisplay()
    {
        //Delete the entire hand
        foreach(var v in currentHandGameObjects)
        {
            Destroy(v);
        }
        currentHandGameObjects.Clear();

        //Redraw Hand
        foreach (Card card in player.GetHand())
        {
           currentHandGameObjects.Add(Instantiate(AllCardsList[card.index], playerOneHandDisplay.transform));
        }
    }

    //Triggers
    public void StartOfGameEffects()
    {
        //Check if there are any cards in either deck that have start of game effects
        foreach (var card in player.GetHand())
        {
            card.StartOfGame(player, opponent, this, true);
        }
        foreach (var card in opponent.GetHand())
        {
            card.StartOfGame(player, opponent, this, false);
        }
    }
    public void EndOfTurnEffects()
    {
        //Check each hand and trigger any end of turn effects
        foreach (var card in player.GetHand())
        {
            card.CardInHand(player, opponent, this, true);
        }
        foreach (var card in opponent.GetHand())
        {
            card.CardInHand(player, opponent, this, false);
        }
    }

    //Check the current effects
    public bool EnabledDraw()
    {
        foreach (var _event in OngoingEvents)
        {
            if (_event.Key == Enums._Event.LIMITED_DECK)
                return false;
        }
        return true;
    }
    public bool EnabledHeal()
    {
        foreach (var _event in OngoingEvents)
        {
            if (_event.Key == Enums._Event.NO_HEALS)
                return false;
        }
        return true;
    }
    public bool EnabledShield()
    {
        foreach (var _event in OngoingEvents)
        {
            if (_event.Key == Enums._Event.NO_SHIELDS)
                return false;
        }
        return true;
    }

    //Check if the game should be over
    private bool CheckWinner()
    {
        return (player.GetHealth() <= 0 || opponent.GetHealth() <= 0);
    }
    private string GetWinner()
    {
        if (player.GetHealth() <= 0 && opponent.GetHealth() <= 0)
        {
            return "Draw";
        }
        else if (player.GetHealth() <= 0)
        {
            return "AI Wins";
        }
        else
        {
            return "You Win!";
        }
    }
    private void GameOver()
    {
        //Delete all the cards in hand
        foreach (var v in currentHandGameObjects)
        {
            Destroy(v);
        }

        string winner = GetWinner();
        if (winner == "You Win!")
        {
            int reward = Random.Range(victoryRewardMin, victoryRewardMax + 1);
            BotText.text = "Gain " + reward + " Money";
            Inventory.Instance.UpdateFunds(reward);
        }
        else
        {
            BotText.text = "";
        }
        TopText.text = winner;
    }
}
