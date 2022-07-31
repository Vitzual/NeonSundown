using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackmarket : MonoBehaviour
{
    // Blackmarket item
    public struct Item
    {
        public Item(BlackmarketData data, int index)
        {
            this.data = data;
            this.index = index;
        }

        public BlackmarketData data;
        public int index;
    }

    // Dictionary of lists
    public SerializableDictionary<BlackmarketData.Type, Transform> listings;
    private Dictionary<BlackmarketData.Type, BlackmarketPair> genPair;
    private Dictionary<BlackmarketData.Type, int> typeIndex;

    // Blackmarket pair prefab
    public BlackmarketPair blackmarketPair;

    // On start generate listing 
    public void Start()
    {
        // Setup new dictionary
        genPair = new Dictionary<BlackmarketData.Type, BlackmarketPair>();
        typeIndex = new Dictionary<BlackmarketData.Type, int>();

        // Update black market listings
        UpdateListings();
    }

    // On open, update listings
    public void UpdateListings()
    {
        // List of organized items
        BlackmarketData[] blackmarketItems = new BlackmarketData[Scriptables.blackmarketItems.Count];

        // Iterate through all types and organize
        int previousAmount = 0;
        foreach (int i in Enum.GetValues(typeof(BlackmarketData.Type)))
        {
            int totalAmount = 0;
            foreach (BlackmarketData data in Scriptables.blackmarketItems)
            {
                if ((int)data.type == i)
                {
                    blackmarketItems[previousAmount + data.order] = data;
                    totalAmount += 1;
                }
            }
            previousAmount += totalAmount;
        }

        for (int i = 0; i < blackmarketItems.Length; i++)
            Debug.Log(i + ") " + blackmarketItems[i]);

        // Load all blackmarket items 
        foreach (BlackmarketData blackmarketItem in blackmarketItems)
        {
            // Check if null
            if (blackmarketItem == null) continue;

            // Check if there's an available pair
            if (genPair.ContainsKey(blackmarketItem.type))
            {
                // Set bottom item if gen pair not null
                if (genPair[blackmarketItem.type] != null)
                {
                    genPair[blackmarketItem.type].Setup(blackmarketItem, false);
                    genPair[blackmarketItem.type] = null;
                }
                else
                {
                    BlackmarketPair newPair = Instantiate(blackmarketPair, listings[blackmarketItem.type]);
                    genPair[blackmarketItem.type] = newPair;
                    newPair.Setup(blackmarketItem, true);
                }
            }
            else
            {
                BlackmarketPair newPair = Instantiate(blackmarketPair, listings[blackmarketItem.type]);
                genPair.Add(blackmarketItem.type, newPair);
                newPair.Setup(blackmarketItem, true);
            }
        }
    }
}
