using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RuntimeCards : MonoBehaviour
{
    public Dictionary<CardData, RuntimeCard> cards = 
        new Dictionary<CardData, RuntimeCard>();
    public RuntimeCard runtimeCard;
    public Transform runtimeList;
    public float spacing;
    public int length;

    public void Start()
    {
        Events.active.onAddCard += UpdateStats;
    }

    public void UpdateStats(CardData card)
    {
        if (cards.ContainsKey(card)) cards[card].UpdateStat();
        else
        {
            RuntimeCard newCard = Instantiate(runtimeCard, runtimeList);
            newCard.Setup(card, new Vector2((cards.Count % length) * spacing,
                (cards.Count / length) * spacing));
            cards.Add(card, newCard);
        }
    }
}
