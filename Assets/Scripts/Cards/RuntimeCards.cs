using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RuntimeCards : MonoBehaviour
{
    private RuntimeCard lastCard;
    private Dictionary<CardData, RuntimeCard> cards = 
        new Dictionary<CardData, RuntimeCard>();
    public RuntimeCard runtimeCard;
    public Transform runtimeList;
    public float spacing;
    public int length;

    public void Start()
    {
        Events.active.onAddCard += UpdateCards;
        Events.active.onShipOnlyCardAdded += UpdateCards;
        Events.active.onAddSynergy += UpdateSynergy;
    }

    public void UpdateCards(CardData card)
    {
        if (cards.ContainsKey(card)) cards[card].UpdateStat();
        else
        {
            RuntimeCard newCard = Instantiate(runtimeCard, Vector2.zero, Quaternion.identity, runtimeList);
            newCard.Setup(card, new Vector2((int)(cards.Count / length) * spacing,
                (int)(cards.Count % length) * spacing));
            cards.Add(card, newCard);

            if (lastCard != null) 
                lastCard.SetNextCard(newCard);
            lastCard = newCard;
        }
    }

    public void UpdateSynergy(SynergyData synergy)
    {
        // Check card count
        if (cards.Count == 0) return;

        // Remove cards depending on tier
        switch (synergy.tier)
        {
            case 1:
                if (synergy.removeCardOne && cards.ContainsKey(synergy.cardOne))
                {
                    cards[synergy.cardOne].DeleteStat();
                    cards.Remove(synergy.cardOne);
                }
                if (synergy.removeCardTwo && cards.ContainsKey(synergy.cardTwo))
                {
                    cards[synergy.cardOne].DeleteStat();
                    cards.Remove(synergy.cardOne);
                }
                UpdateCards(synergy.outputCard);
                break;

            case 2:
                if (synergy.removeCardOne && cards.ContainsKey(synergy.synergyOne.outputCard))
                {
                    cards[synergy.synergyOne.outputCard].DeleteStat();
                    cards.Remove(synergy.synergyOne.outputCard);
                }
                if (synergy.removeCardTwo && cards.ContainsKey(synergy.synergyTwo.outputCard))
                {
                    cards[synergy.synergyOne.outputCard].DeleteStat();
                    cards.Remove(synergy.synergyOne.outputCard);
                }
                UpdateCards(synergy.outputCard);
                break;

            case 3:
                if (synergy.removeCardOne && cards.ContainsKey(synergy.baseSynergy.outputCard))
                {
                    cards[synergy.baseSynergy.outputCard].DeleteStat();
                    cards.Remove(synergy.baseSynergy.outputCard);
                }
                UpdateCards(synergy.outputCard);
                break;
        }
    }
}
