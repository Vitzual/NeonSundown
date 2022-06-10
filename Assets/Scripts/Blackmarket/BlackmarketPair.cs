using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackmarketPair : MonoBehaviour
{
    // Top and bottom items
    public BlackmarketItem topItem, bottomItem;

    // Setup a new pair
    public void Setup(BlackmarketData data, bool top)
    {
        if (top)
        {
            topItem.Set(data);
            topItem.gameObject.SetActive(true);
        }
        else
        {
            bottomItem.Set(data);
            bottomItem.gameObject.SetActive(true);
        }
    }
}
