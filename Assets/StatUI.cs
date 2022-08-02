using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI value, average, lowest, highest;
    public ProgressBar bar;
    
    public void Set(StatInfo info, float amount, Color color)
    {
        icon.sprite = info.sprite;
        icon.color = color;
        value.text = "<b>" + info.name.ToUpper() + ":</b> " + Formatter.Round(amount, 0) + info.prefix;
        average.text = "<b>AVG:</b> " + Formatter.Round(info.GetAverage(), 0) + info.prefix;
        lowest.text = "<b>LOWEST:</b> " + Formatter.Round(info.GetLowest(), 0) + info.prefix;
        highest.text = "<b>HIGHEST:</b> " + Formatter.Round(info.GetHighest(), 0) + info.prefix;
        bar.minValue = info.GetLowest();
        bar.maxValue = info.GetHighest();
        bar.currentPercent = amount;
        bar.UpdateUI();
    }
}
