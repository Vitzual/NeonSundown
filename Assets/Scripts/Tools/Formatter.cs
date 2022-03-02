using System;
using UnityEngine;

public class Formatter : MonoBehaviour
{
    public static string Time(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        return string.Format("{0:D1}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }

    public static string Round(float amount)
    {
        return String.Format("{0:0.0}", amount);
    }
}
