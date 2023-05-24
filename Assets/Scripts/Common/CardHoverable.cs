using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardHoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMPro.TMP_Text TextDisplay;
    [SerializeField] RectTransform TextWindow;
    [SerializeField] float TimeToWait = 0.05f;
    private Gameplay_Card card;

    void Start()
    {
        card = CardConnector.GetGameplayCard(GetComponent<GatchaCard>().GetCardID());
        HideMessage();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        HideMessage();
    }

    private void ShowMessage()
    {
        //if (TextDisplay != null) TextDisplay.text = TextToShow;
        TextDisplay.text = $"{card.GetCardName()}\n{card.GetCardDescription()}";
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
