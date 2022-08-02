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

    public void Set(StatInfo info, float amount)
    {
        icon.sprite = info.sprite;
        value.text = "<b>" + info.name.ToUpper() + ":</b> " + Formatter.Round(amount, 0);
        average.text = "<b>AVERAGE:</b> " + Formatter.Round(info.GetAverage(), 0);
        lowest.text = Formatter.Round(info.GetLowest(), 0);
        highest.text = Formatter.Round(info.GetHighest(), 0);
        bar.minValue = info.GetLowest();
        bar.maxValue = info.GetHighest();
        bar.currentPercent = amount;
        bar.UpdateUI();
    }
}
