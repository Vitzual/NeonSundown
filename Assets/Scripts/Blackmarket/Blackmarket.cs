using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackmarket : MonoBehaviour
{
    // Dictionary of lists
    public SerializableDictionary<BlackmarketData.Type, Transform> listings;
    private Dictionary<BlackmarketData.Type, BlackmarketPair> genPair;

    // Blackmarket pair prefab
    public BlackmarketPair blackmarketPair;

    // On start generate listing 
    public void Start()
    {

    }

    // On open, update listings
    public void UpdateListings()
    {
        // Load all blackmarket items 
        foreach (BlackmarketData blackmarketItem in Scriptables.blackmarketItems)
        {
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

            }
        }
    }
}
