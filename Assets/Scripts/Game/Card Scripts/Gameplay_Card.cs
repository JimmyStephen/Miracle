using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static Enums;

public class Gameplay_Card : Card
{
    [Header("What to Display")]
    [SerializeField, Tooltip("Output discription when the card is played")] string CardOutputDescription = "Default Card Effect";
    [Header("What the card does")]
    [SerializeField, Tooltip("List of all the effects the card can trigger")] List<Card_Effect> Effects;
    [SerializeField, Tooltip("List of all the events the card can trigger")] List<Card_Event> Events;
    [SerializeField, Tooltip("List of all the effects that effect other cards the card can trigger")] List<Card_CardEffect> CardEffects;
    [SerializeField, Tooltip("List of all the swap effects that can trigger")] List<Card_SwapEffect> SwapEffects;

    private void Start()
    {
        _Init();
    }
    /// <summary>
    /// Call when the card is created on the scene, will set all the values as needed
    /// </summary>
    public void _Init()
    {
        if(NameTextbox != null)
            NameTextbox.text = CardName;
        if(ImageBox != null)
            ImageBox.sprite = Image;

        foreach(var effect in Effects)
            effect._Init(CardOutputDescription);
        foreach (var _event in Events)
            _event._Init(CardOutputDescription);
        foreach (var effect in CardEffects)
            effect._Init(CardOutputDescription);
        foreach (var effect in SwapEffects)
            effect._Init(CardOutputDescription);
    }

    /// <summary>
    /// Called when a card is locked in, triggers events or effects that trigger at this point
    /// </summary>
    /// <returns>The effect of the card to display on the screen</returns>
    public string PlayCard(Player player, EnemyAI AI, GameplayManager GM, bool PlayedByPlayer, Trigger onTrigger)
    {
        string S_Effect = CardOutputDescription;
        foreach (var effect in Effects)
        {
            if (effect.EffectTrigger == onTrigger)
            {
                S_Effect = effect.TriggerEffect(player, AI, GM, PlayedByPlayer);
            }
        }

        foreach (var _event in Events)
        {
            if (_event.EffectTrigger == onTrigger)
            {
                S_Effect = _event.TriggerEffect(player, AI, GM, PlayedByPlayer);
            }
        }

        foreach (var effect in CardEffects)
        {
            if (effect.EffectTrigger == onTrigger)
            {
                S_Effect = effect.TriggerEffect(player, AI, GM, PlayedByPlayer);
            }
        }

        foreach (var effect in SwapEffects)
        {
            if (effect.EffectTrigger == onTrigger)
            {
                S_Effect = effect.TriggerEffect(player, AI, GM, PlayedByPlayer);
            }
        }

        return S_Effect;
    }

    /// <summary>
    /// Lets the gameplay manager know when the card is selected
    /// </summary>
    public void Select()
    {
        FindObjectOfType<GameplayManager>().SetSelectedCard(CardID);
    }

    public (string, Target) CardEffectType()
    {
        foreach(var e in Effects) { return (e.EffectType, e.CardTarget); }
        foreach(var e in Events) { return (e.EffectType, e.CardTarget); }
        foreach(var e in CardEffects) { return (e.EffectType, e.CardTarget); }
        foreach(var e in SwapEffects) { return (e.EffectType, e.CardTarget); }
        return ("Unknown", Target.NONE);
    }
    public List<Card_Effect> GetEffects() { return Effects; }
    public List<Card_Event> GetEvents() { return Events; }
    public List<Card_CardEffect> GetCardEffects() { return CardEffects; }
    public List<Card_SwapEffect> GetSwapEffects() { return SwapEffects; }
}
