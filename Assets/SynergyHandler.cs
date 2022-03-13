using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyHandler : MonoBehaviour
{
    // List of max cards
    public static List<CardData> maxedCards;
    private static Queue<SynergyData> availableSynergies;

    // On start subscribe to crystal break
    public void Start()
    {
        maxedCards = new List<CardData>();
        Events.active.onBloodCrystalBroken += Create;
    }

    // Add a card to the maxed list
    public static void Add(CardData data)
    {
        maxedCards.Add(data);

        // Check if synergy available
        foreach (SynergyData synergy in Scriptables.synergies)
        {
            if (maxedCards.Contains(synergy.cardOne) &&
                maxedCards.Contains(synergy.cardTwo))
            {
                availableSynergies.Enqueue(synergy);
            }
        }
    }

    // Create a synergy
    public static void Create()
    {
        if (availableSynergies.Count > 0)
            SynergyUI.active.Synergize(availableSynergies.Dequeue());
    }
}
