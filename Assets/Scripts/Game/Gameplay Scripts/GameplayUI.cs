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
    List<GameObject> currentHandGameObjects;

    [SerializeField] TMPro.TMP_Text TopText;
    [SerializeField] TMPro.TMP_Text BotText;

    /// <summary>
    /// Update the display for the UI, health, and text
    /// </summary>
    private void UpdateDisplay(Player player, EnemyAI opponent, string TopText = "", string BotText = "")
    {
        playerOneHealthDisplay.text = player.GetHealthDisplay();
        playerTwoHealthDisplay.text = opponent.GetHealthDisplay();
        this.TopText.text = TopText;
        this.BotText.text = BotText;
        UpdatePlayerHandDisplay(player);
    }
    /// <summary>
    /// Remove the current hand from the display
    /// </summary>
    private void ClearHand()
    {
        currentHandGameObjects.ForEach(go => { Destroy(go); });
        currentHandGameObjects.Clear();
    }
    /// <summary>
    /// Draw the players hand on the screen
    /// </summary>
    private void UpdatePlayerHandDisplay(Player player)
    {
        //Delete the entire hand
        ClearHand();
        //Redraw the hand
        player.Hand.ForEach(card => { currentHandGameObjects.Add(Instantiate(GameplayManager.Instance.AllCardsList[card.CardID], playerOneHandDisplay.transform)); });
    }
}
