using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackmarket : MonoBehaviour
{
    // Dictionary of lists
    public SerializableDictionary<BlackmarketData.Type, Transform> listings;

    // Generated flag
    private bool listingsGenerated = false;
    


    // On open, update listings
    public void UpdateListings()
    {

    }
}
