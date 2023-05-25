using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardHoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Gameplay_Card card;
    private CardHoverManager HoverManager;

    void Start()
    {
        HoverManager = FindObjectOfType<CardHoverManager>();
        if (HoverManager == null)
        {
            Destroy(this);
            return;
        }
        card = CardConnector.GetGameplayCard(GetComponent<GatchaCard>().GetCardID());

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverManager.ShowMessage(card, GetComponent<RectTransform>());
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        HoverManager.HideMessage();
    }
}
