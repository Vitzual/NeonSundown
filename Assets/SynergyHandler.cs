using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyHandler : MonoBehaviour
{
    // List of max cards
    public static List<CardData> maxedCards;
    private static Queue<SynergyData> availableSynergies;
    private static List<SynergyData> synergiesMade;

    // On start subscribe to crystal break
    public void Start()
    {
        maxedCards = new List<CardData>();
        Events.active.onBloodCrystalBroken += Create;
        availableSynergies = new Queue<SynergyData>();
        synergiesMade = new List<SynergyData>();
    }
    
    // Add a card to the maxed list
    public static void Add(CardData data)
    {
        // Add card
        if (!maxedCards.Contains(data)) maxedCards.Add(data);
        Debug.Log(data.name + " is maxed and ready for synergies!");

        // Check if synergy available
        foreach (SynergyData synergy in Scriptables.synergies)
        {
            if (SaveSystem.IsSynergyUnlocked(synergy.InternalID) &&
                !synergiesMade.Contains(synergy) &&
                maxedCards.Contains(synergy.cardOne) &&
                maxedCards.Contains(synergy.cardTwo))
            {
                availableSynergies.Enqueue(synergy);
                synergiesMade.Add(synergy);
                Debug.Log(synergy.name + " synergy is available!");
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