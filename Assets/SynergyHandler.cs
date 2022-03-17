using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyHandler : MonoBehaviour
{
    // List of max cards
    public static List<CardData> maxedCards;
    public static Queue<SynergyData> availableSynergies;
    private static List<SynergyData> synergiesMade;

    // Create new list instances
    public void Awake()
    {
        maxedCards = new List<CardData>();
        availableSynergies = new Queue<SynergyData>();
        synergiesMade = new List<SynergyData>();
    }

    // On start subscribe to crystal break
    public void Start()
    {
        Events.active.onBloodCrystalBroken += Create;
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
                Events.active.SynergyAvailable(synergy);
            }
        }
    }
    
    // Create a synergy
    public static void Create()
    {
        if (availableSynergies.Count > 0)
        {
            // Add the synergy and unlock
            SynergyData synergy = availableSynergies.Dequeue();
            SynergyUI.active.Synergize(synergy);
            if (synergy.achievement != null)
                SaveSystem.UnlockAchievement(synergy.achievement);
        }
    }
}
