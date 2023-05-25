using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CardHoverManager : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text TextDisplay;
    [SerializeField] RectTransform TextWindow;
    [SerializeField] float TimeToWait = 0.05f;

    private void Start()
    {
        HideMessage();
    }

    public void ShowMessage(Gameplay_Card card, RectTransform transform)
    {
        TextDisplay.text = $"{card.GetCardName()}\n{card.GetCardDescription()}";    
        Vector3 position = transform.position;
        position.y += (transform.rect.height * .5f) + (TextDisplay.preferredHeight * .5f);
        TextWindow.position = position;
        TextWindow.sizeDelta = new() { x = 400, y = TextDisplay.preferredHeight };
        TextWindow.gameObject.SetActive(true);
    }
    public void HideMessage()
    {
        if (TextDisplay != null) TextDisplay.text = "";
        if (TextWindow != null) TextWindow.gameObject.SetActive(false);
    }
}
