using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardEffects : MonoBehaviour
{
    public List<TextMeshProUGUI> _effects;
    public static List<TextMeshProUGUI> effects;

    public void Awake() { effects = _effects; }
    
    public static void UpdateEffects()
    {
        int totalSkipped = 0;

        foreach (int i in System.Enum.GetValues(typeof(Stat)))
        {
            // Get index based on total skipped
            int index = i - totalSkipped;
            if (index >= effects.Count)
            {
                Debug.Log("Ran out of effect text!");
                return;
            }

            // Get stat
            Stat stat = (Stat)i;
            if (stat == Stat.Evasion || stat == Stat.DashSpeed || stat == Stat.AutoCollect ||
                stat == Stat.View || stat == Stat.Tracking || stat == Stat.Explosive)
            {
                totalSkipped += 1;
                continue;
            }

            // Do the thing my guy
            string name = "<b>" + stat.ToString() + ":</b><color=white> ";
            name += Formatter.Round(Deck.player.GetStat(stat)) + GetDifference(stat);
            effects[index].text = name;
        }
    }

    // Calculate stat
    public static string GetDifference(Stat stat)
    {
        // New and current amounts
        float difference = Deck.GetStat(stat) - Deck.GetDefaultStat(stat);
        if (difference == 0) return "";

        // Calculate color
        string color = " <color=green> (";

        // Check stat
        if (difference < 0 && stat != Stat.Cooldown && stat != Stat.Knockback && stat != Stat.EnemyDmg) color = "<color=red> (";
        else if (difference >= 0 && stat == Stat.Cooldown && stat == Stat.Knockback) color = "<color=green> (";
        if (difference >= 0) color += "+";
        
        // Return formatted string
        return color + Formatter.Round(difference) + ")";
    }
}
