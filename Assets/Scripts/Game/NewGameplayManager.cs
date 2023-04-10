using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameplayManager : MonoBehaviour
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
        //Debug.Log("Gameplay Started");
        //initalize decks
        playerOneDeck.Init();
        playerTwoDeck.Init();

        //Check for start of game effects before triggering anything
        StartOfGameEffects();

        player = new Player(playerOneDeck, PlayerOneStartingHealth, null);
        opponent = new EnemyAI(playerTwoDeck, PlayerTwoStartingHealth, null);

        //set inital state
        currentGameState = CurrentMode.WAITING;

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
                foreach (Card card in player.GetHand())
                {
                    //add to the display
                    currentHandGameObjects.Add(Instantiate(AllCardsList[card.index], playerOneHandDisplay.transform));
                }

                playerOneHealthDisplay.text = player.GetHealth().ToString();
                playerTwoHealthDisplay.text = opponent.GetHealth().ToString();

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
        string TopTextReturn = "";
        string BotTextReturn = "";

        //display and get cards
        playerTwoDisplay = Instantiate(AllCardsList[opponent.Play().index], playerTwoChosenDisplay.transform);

        Card playerCard = playerOneDisplay.GetComponent<Card>();
        Card opponentCard = playerTwoDisplay.GetComponent<Card>();

        //playerCard._Init();
        //opponentCard._Init();

        Debug.Log("Change to be \"this\" instead of \"null\"");
        playerCard.PlayCard(player, opponent, null, true);
        opponentCard.PlayCard(player, opponent, null, false);

        //set the selected to null
        currentPlayerOneSelected = null;
        //add the opponents card to the used cards
        playerOneDeck.AddToNewDeck(playerCard);
        playerTwoDeck.AddToNewDeck(opponentCard);

        //Trigger end of turn effects
        EndOfTurnEffects();

        //Set the displays
        TopText.text = TopTextReturn;
        BotText.text = BotTextReturn;
        playerOneHealthDisplay.text = player.GetHealth().ToString();
        playerTwoHealthDisplay.text = opponent.GetHealth().ToString();
    }



    //Triggers
    public void StartOfGameEffects()
    {
        //Check if there are any cards in either deck that have start of game effects
        Debug.Log("Change null to this");
        foreach(var card in player.GetHand())
        {
            card.StartOfGame(player, opponent, null, true);
        }
        foreach (var card in opponent.GetHand())
        {
            card.StartOfGame(player, opponent, null, false);
        }
    }
    public void EndOfTurnEffects()
    {
        //Check each hand and trigger any end of turn effects
        Debug.Log("Change null to this");
        foreach (var card in player.GetHand())
        {
            card.CardInHand(player, opponent, null, true);
        }
        foreach (var card in opponent.GetHand())
        {
            card.CardInHand(player, opponent, null, false);
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
