using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeData
{
    public TimeData(float time, string ship, SerializableDictionary<string, int> cards)
    {
        this.time = time;
        this.ship = ship;
        this.cards = cards;
    }

    [SerializeField] public float time;
    [SerializeField] public string ship;
    [SerializeField] public SerializableDictionary<string, int> cards;
}
