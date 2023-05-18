using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text playerOneHealthDisplay;
    [SerializeField] TMPro.TMP_Text playerTwoHealthDisplay;
    [SerializeField] GameObject playerOneChosenDisplay;
    [SerializeField] GameObject playerTwoChosenDisplay;
    [SerializeField] GameObject playerOneHandDisplay;
    [SerializeField] GameObject playerTwoHandDisplay;
    [SerializeField] TMPro.TMP_Text TopText;
    [SerializeField] TMPro.TMP_Text BotText;

    [SerializeField] GameObject CardBackObj;

    private GameObject playerOneDisplay;
    private GameObject playerTwoDisplay;
    private List<GameObject> currentPlayerHandGameObjects;
    private List<GameObject> currentOpponentHandGameObjects;

    public void Init()
    {
        currentPlayerHandGameObjects = new();
        currentOpponentHandGameObjects = new();
        ClearDisplay();
    }

    /// <summary>
    /// Update the display for the UI, health, and text
    /// </summary>
    public void UpdateDisplay(Player player, EnemyAI opponent, string TopText = "", string BotText = "")
    {
        playerOneHealthDisplay.text = player.GetHealthDisplay();
        playerTwoHealthDisplay.text = opponent.GetHealthDisplay();
        this.TopText.text = TopText;
        this.BotText.text = BotText;
        UpdateBothHandDisplays(player, opponent);
    }
    
    //Draw
    /// <summary>
    /// Update the hand display for both players
    /// </summary>
    public void UpdateBothHandDisplays(Player player, EnemyAI opponent)
    {
        UpdatePlayerHandDisplay(player);
        UpdateOpponentHandDisplay(opponent);
    }
    /// <summary>
    /// Draw the players hand on the screen
    /// </summary>
    public void UpdatePlayerHandDisplay(Player player)
    {
        //Delete the entire hand
        ClearHand();
        //Redraw the hand
        player.Hand.ForEach(card => 
        {
            //Debug.Log($"Card In Hand: {card.GetCardName()} : ID: {card.GetCardID()}");
            //GameObject CardToDraw = CardConnector.GetGameplayCard(card);
            //Debug.Log($"Card In Hand: {CardToDraw.GetComponent<Gameplay_Card>().GetCardName()} : ID: {CardToDraw.GetComponent<Gameplay_Card>().GetCardID()}");
            currentPlayerHandGameObjects.Add(Instantiate(CardConnector.GetGameplayCardObj(card), playerOneHandDisplay.transform)); 
        });
    }
    /// <summary>
    /// Draw cards on the opponents side of the screen
    /// </summary>
    /// <param name="opponent"></param>
    public void UpdateOpponentHandDisplay(EnemyAI opponent)
    {
        ClearAIHand();
        opponent.Hand.ForEach((card) =>
        {
            currentOpponentHandGameObjects.Add(Instantiate(CardBackObj, playerTwoHandDisplay.transform));
        });
    }
    
    /// <summary>
    /// Draws a card on the screen based on the selected ID
    /// </summary>
    /// <param name="cardID">ID of the card to draw</param>
    /// <param name="player">What player is having a card drawn</param>
    public Gameplay_Card DrawSelectedCard(int CardID, Enums.PlayerOption player)
    {
        EraseCard(player);
        if (player == Enums.PlayerOption.PLAYER_ONE)
        {
            playerOneDisplay = Instantiate(CardConnector.GetGameplayCardObj(CardID), playerOneChosenDisplay.transform);
            return playerOneDisplay.GetComponent<Gameplay_Card>();
        }
        else
        {
            playerTwoDisplay = Instantiate(CardConnector.GetGameplayCardObj(CardID), playerTwoChosenDisplay.transform);
            return playerTwoDisplay.GetComponent<Gameplay_Card>();
        }
    }



    //Clear
    /// <summary>
    /// Clears the entire display setting everything to be blank
    /// </summary>
    public void ClearDisplay()
    {
        ClearHands();
        EraseCard(Enums.PlayerOption.BOTH);
        playerOneHealthDisplay.text = "";
        playerTwoHealthDisplay.text = "";
        TopText.text = "";
        BotText.text = "";
    }
    
    /// <summary>
    /// Clear both hands from the display
    /// </summary>
    public void ClearHands()
    {
        ClearHand();
        ClearAIHand();
    }    
    /// <summary>
    /// Remove the current hand from the display
    /// </summary>
    public void ClearHand()
    {
        currentPlayerHandGameObjects.ForEach(go => { Destroy(go); });
        currentPlayerHandGameObjects.Clear();
    }
    /// <summary>
    /// Remove the current opponents hand from the display
    /// </summary>
    public void ClearAIHand()
    {
        currentOpponentHandGameObjects.ForEach(go => { Destroy(go); });
        currentOpponentHandGameObjects.Clear();
    }

    /// <summary>
    /// Erases the card display
    /// </summary>
    /// <param name="target">What player has the display erased</param>
    public void EraseCard(Enums.PlayerOption target)
    {
        switch (target)
        {
            case Enums.PlayerOption.PLAYER_ONE:
                if (playerOneDisplay != null) Destroy(playerOneDisplay);
                break;
            case Enums.PlayerOption.PLAYER_TWO:
                if (playerTwoDisplay != null) Destroy(playerTwoDisplay);
                break;
            case Enums.PlayerOption.BOTH:
                if (playerOneDisplay != null) Destroy(playerOneDisplay);
                if (playerTwoDisplay != null) Destroy(playerTwoDisplay);
                break;
            default:
                throw new System.Exception("This point should not be reached");
        }
    }
}
