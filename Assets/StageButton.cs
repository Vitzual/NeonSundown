using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    // Interface elements
    public new TextMeshProUGUI name;
    public TextMeshProUGUI rate, stats;
    public Image icon, border;

    // Update is called once per frame
    public void Set(StageData.Enemy enemy)
    {
        // Get the variant color
        Color color = VariantPalette.GetPrimaryColor(enemy.variant);
        
        // Set all enemy information
        name.text = enemy.data.name;
        rate.text = enemy.amount + "x / " + enemy.cooldown + "s";
        stats.text = "<b>HEALTH:</b> " + enemy.data.variants[enemy.variant].health +
                     "| <b>DAMAGE:</b> " + enemy.data.variants[enemy.variant].damage +
                     "| <b>SPEED:</b> " + enemy.data.variants[enemy.variant].speed;
        icon.sprite = enemy.data.icon;
        icon.color = color;
        border.color = color;
    }
}
