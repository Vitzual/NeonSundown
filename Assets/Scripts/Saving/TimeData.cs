using System.Collections.Generic;

[System.Serializable]
public class TimeData
{
    public TimeData(float time, string ship, SerializableDictionary<string, int> cards)
    {
        this.time = time;
        this.ship = ship;
        this.cards = cards;
    }

    public float time;
    public string ship;
    public SerializableDictionary<string, int> cards;
}
