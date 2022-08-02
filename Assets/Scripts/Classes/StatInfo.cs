using UnityEngine;

public class StatInfo
{
    // Stat info
    public Stat stat;
    public string name;
    public Sprite sprite;

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
