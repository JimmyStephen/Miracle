using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] GameObject playerOneHandDisplay;
    [SerializeField] GameObject[] initalCards;
    [SerializeField] GameObject playerOneChosenDisplay;
    [SerializeField] GameObject playerTwoChosenDisplay;
    [SerializeField] TMPro.TMP_Text TopText;
    [SerializeField] TMPro.TMP_Text BotText;
    private GameObject playerOneDisplay;
    private GameObject playerTwoDisplay;

    //Health
    [SerializeField] int startingHealth = 20;
    //p1
    [SerializeField] TMPro.TMP_Text playerOneHealthDisplay;
    private int currentPlayerOneHealth;
    //p2
    [SerializeField] TMPro.TMP_Text playerTwoHealthDisplay;

    //current card
    Card currentPlayerOneSelected = null;

    //starting decks
    [SerializeField] Deck playerOneDeck;
    [SerializeField] Deck playerTwoDeck;

    //player one hand
    List<Card> playerOneHand;
    List<GameObject> currentHandGameObjects;
    private EnemyAI opponent;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Gameplay Started");
        //initalize decks
        playerOneDeck.Init();
        playerTwoDeck.Init();
        
        
        opponent = new EnemyAI(playerTwoDeck, startingHealth);
        //initalizePlayers
        currentPlayerOneHealth = startingHealth;
        //initalize AI (if needed)
        //get inital hands
        playerOneHand = playerOneDeck.DrawStartingHand();
        currentHandGameObjects = new List<GameObject>();
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
        currentPlayerOneSelected = initalCards[CardNumber].GetComponent<Card>();
        if (playerOneDisplay != null) Destroy(playerOneDisplay);
        playerOneDisplay = Instantiate(initalCards[currentPlayerOneSelected.index], playerOneChosenDisplay.transform);
    }
    public void Continue()
    {
        string setButtonText = "";
        switch (currentGameState)
        {
            case CurrentMode.WAITING:
                //set starting display
                int count = 1;
                foreach (Card card in playerOneHand)
                {
                    //add to the display
                    currentHandGameObjects.Add(Instantiate(initalCards[card.index], playerOneHandDisplay.transform));
                }
                playerOneHealthDisplay.text = currentPlayerOneHealth.ToString();
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
                else {
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
                //setButtonText = "Main Menu";
                //SceneLoader.Instance.LoadScene(1);
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
        string TopTextReturn;
        string BotTextReturn;

        //display and get cards
        playerTwoDisplay = Instantiate(initalCards[opponent.Play().index], playerTwoChosenDisplay.transform);
        Card playerCard = playerOneDisplay.GetComponent<Card>();
        Card opponentCard = playerTwoDisplay.GetComponent<Card>();
        opponentCard.Init();


        int playerOnePriority = playerCard.GetPriority();
        int playerTwoPriority = opponentCard.GetPriority();

        if (!(playerTwoPriority == 2 && playerOnePriority == 2))
        {
            //1 shield | 2 heal | 3 damage
            int playerOneValue = playerCard.GetValue();
            int playerTwoValue = opponentCard.GetValue();

            //trigger shield
            int playerOneShield = (playerOnePriority == 1) ? playerOneValue : 0;
            int playerTwoShield = (playerTwoPriority == 1) ? playerTwoValue : 0;
            //heal
            int playerOneHeal = (playerOnePriority == 2) ? playerOneValue : 0;
            int playerTwoHeal = (playerTwoPriority == 2) ? playerTwoValue : 0;
            //deal damage
            int playerTwoDamage = (playerOnePriority == 3) ? ((playerOneValue - playerTwoShield > 0) ? playerOneValue - playerTwoShield : 0) : 0;
            int playerOneDamage = (playerTwoPriority == 3) ? ((playerTwoValue - playerOneShield > 0) ? playerTwoValue - playerOneShield : 0) : 0;

            //Set RetVals
            TopTextReturn = "Player One " + playerCard.GetCardEffect();
            BotTextReturn = "Player Two " + opponentCard.GetCardEffect();

            //update health based on outcome (and make sure you dont overheal)
            UpdateHealth(playerOneHeal, playerOneDamage);
            opponent.UpdateHealth(playerTwoHeal, playerTwoDamage);
        }
        else
        {
            TopTextReturn = "DOUBLE HEAL";
            BotTextReturn = "Canceled Out!";
        }

        //If the player draws cards
        if(playerOnePriority == 4)
        {
            TopTextReturn = "Player One New Hand";
            PlayerNewHand();
        }
        else
        {
            //add your card to the used cards
            playerOneDeck.AddToNewDeck(playerCard);

            //remove the card from hand and redraw a new one
            int index = GetIndex(currentPlayerOneSelected);
            playerOneHand.RemoveAt(index);
            GameObject temp = currentHandGameObjects[index];
            Destroy(temp, .01f);
            currentHandGameObjects.RemoveAt(index);

            //update display
            DrawAndUpdatePlayerOneHandDisplay();
        }
        //set the selected to null
        currentPlayerOneSelected = null;

        //add the opponents card to the used cards
        playerTwoDeck.AddToNewDeck(opponentCard);

        //Set the displays
        TopText.text = TopTextReturn;
        BotText.text = BotTextReturn;
        playerOneHealthDisplay.text = currentPlayerOneHealth.ToString();
        playerTwoHealthDisplay.text = opponent.GetHealth().ToString();
    }
    private bool CheckWinner()
    {
        return (currentPlayerOneHealth <= 0 || opponent.GetHealth() <= 0);
    }
    /// <summary>
    /// Finds a card based on its index and returns its index in the hand
    /// </summary>
    /// <param name="toFind">The card to find</param>
    /// <returns>Location in your hand</returns>
    private int GetIndex(Card toFind)
    {
        //string searchName = toFind.GetName();
        int searchIndexNum = toFind.index;
        //Debug.Log("Searching For | Name: " + searchName + " Index: " + searchIndexNum);
        for (int i = 0; i < playerOneHand.Count; i++)
        {
            //string currentName = playerOneHand[i].GetName();
            int currentIndex = playerOneHand[i].index;
            //Debug.Log("Found | Name: " + currentName + " Index: " + currentIndex);
            if (searchIndexNum == currentIndex) return i;
        }

        return -1;
    }
    private string GetWinner()
    {
        if (currentPlayerOneHealth <= 0 && opponent.GetHealth() <= 0)
        {
            return "Draw";
        }
        else if(currentPlayerOneHealth <= 0)
        {
            return "AI Wins";
        }
        else{
            return "You Win!";
        }
    }
    private void GameOver()
    {
        //Delete all the cards in hand
        foreach(var v in currentHandGameObjects)
        {
            Destroy(v);
        }

        string winner = GetWinner();
        if(winner == "You Win!")
        {
            int reward = Random.Range(victoryRewardMin, victoryRewardMax+1);
            BotText.text = "Gain " + reward + " Money";
            Inventory.Instance.UpdateFunds(reward);
        }
        else
        {
            BotText.text = "";
        }
        TopText.text = winner;
    }
    private void DrawAndUpdatePlayerOneHandDisplay()
    {
        Card newCard = playerOneDeck.DrawCard();
        playerOneHand.Add(newCard);
        currentHandGameObjects.Add(Instantiate(initalCards[newCard.index], playerOneHandDisplay.transform));
    }
    private void PlayerNewHand()
    {
        //empty hand
        currentHandGameObjects.ForEach(x => {
            Destroy(x);
            playerOneDeck.AddToNewDeck(x.GetComponent<Card>());
        });
        currentHandGameObjects.Clear();
        playerOneHand.Clear();

        //draw hand
        playerOneHand = playerOneDeck.DrawStartingHand();

        //display hand
        foreach (Card card in playerOneHand)
        {
            //add to the display
            currentHandGameObjects.Add(Instantiate(initalCards[card.index], playerOneHandDisplay.transform));
        }
    }

    /// <summary>
    /// Updates the health of the player
    /// </summary>
    /// <param name="healValue">How much to heal</param>
    /// <param name="damageValue">How much damage to take</param>
    private void UpdateHealth(int healValue, int damageValue)
    {
        currentPlayerOneHealth += healValue;
        if (currentPlayerOneHealth > startingHealth) currentPlayerOneHealth = startingHealth;

        currentPlayerOneHealth -= damageValue;
        if (currentPlayerOneHealth < 0) currentPlayerOneHealth = 0;
    }
}
