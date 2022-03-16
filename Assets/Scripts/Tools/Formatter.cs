using System;
using UnityEngine;

public class Formatter : MonoBehaviour
{
    public static string Time(float time)
    {
        // Create new time span
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);

        // Check total time
        if (time < 3600) return string.Format("{0:D1}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        else return string.Format("{0:D1}:{1:D2}:{2:D3}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    public static string Round(float amount, int decimals = 2)
    {
        return Math.Round(amount, decimals).ToString();
    }
}
