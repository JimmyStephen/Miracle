using UnityEngine;

public class GatchaCard : Card
{
    [SerializeField] string ConnectionName = "Unknown";

    public string GetConnectionName() { return ConnectionName; }

    private void Start()
    {
        Init();
    }

    public void Select()
    {
        //Debug.Log("Select | " + GetCardName());
        GameObject DeckBuilderObject = GameObject.Find("DeckBuilder");
        if(DeckBuilderObject != null && DeckBuilderObject.TryGetComponent<Deckbuilder>(out Deckbuilder DeckBuilderComponent))
            DeckBuilderComponent.AddToList(CardID);
    }
}
