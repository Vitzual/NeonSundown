using UnityEngine;

[System.Serializable]
public class StatInfo
{
    // Stat info
    public Stat stat;
    public string name, prefix;
    public int decimalPlaces;
    public Sprite sprite;
    public bool hideInStats = false;

    // Stat values (calculated at runtime)
    private float average = 0, total = 0, lowest = Mathf.Infinity, highest = Mathf.NegativeInfinity;
    private int totalShipsCalculated = 0;

    // Add a value
    public void AddValue(float value)
    {
        // Increase total ships calculated
        totalShipsCalculated += 1;

        // Set lowest and highest
        if (value < lowest) lowest = value;
        else if (value > highest) highest = value;

        // Update average
        total += value;
        average = total / totalShipsCalculated;
    }

    // Returns the average
    public float GetAverage() { return average; }
    public float GetLowest() { return lowest; }
    public float GetHighest() { return highest; }
}
