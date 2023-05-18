using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;

//Notes
    //Edit ShowMessage and Uncomment TextToShow if moved to another project
public class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMPro.TMP_Text TextDisplay;
    [SerializeField] RectTransform TextWindow;
    //[SerializeField] string TextToShow;
    [SerializeField, Tooltip("If the display is for the player of opponent")] bool PlayerDisplay = true;
    [SerializeField, Tooltip("If the display is for deck size or hand size")] bool DeckSizeDisplay = true;
    [SerializeField] float TimeToWait = 0.25f;

    void Start()
    {
        HideMessage();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        HideMessage();
    }

    private void ShowMessage()
    {
        //if (TextDisplay != null) TextDisplay.text = TextToShow;
        //remove
        if (PlayerDisplay)
            if (TextDisplay != null) 
                TextDisplay.text = DeckSizeDisplay ? FindObjectOfType<GameplayManager>().player.Deck.CurrentDeck.Count.ToString() : FindObjectOfType<GameplayManager>().player.Hand.Count.ToString();
        else
            if (TextDisplay != null) 
                TextDisplay.text = DeckSizeDisplay ? FindObjectOfType<GameplayManager>().opponent.Deck.CurrentDeck.Count.ToString() : FindObjectOfType<GameplayManager>().opponent.Hand.Count.ToString();
        //end remove
        if (TextWindow != null) TextWindow.gameObject.SetActive(true);
    }
    private void HideMessage()
    {
        if (TextDisplay != null) TextDisplay.text = "";
        if (TextWindow != null) TextWindow.gameObject.SetActive(false);
    }
    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(TimeToWait);
        ShowMessage();
    }
}
