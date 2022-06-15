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
    public Image icon, border, background;

    // Update is called once per frame
    public void Set(StageData.Enemy enemy)
    {
        // Get the variant color
        Color primaryColor = VariantPalette.GetPrimaryColor(enemy.variant);
        Color secondaryColor = VariantPalette.GetSecondaryColor(enemy.variant);

        // Get seconds thing
        string seconds = " seconds";
        if (enemy.cooldown == 1) seconds = " second";

        // Set all enemy information
        name.text = enemy.data.name.ToUpper();
        rate.text = enemy.amount + "x / " + enemy.cooldown + seconds;
        if (enemy.data.variants[enemy.variant].immune)
        {
            stats.text = "<b>HEALTH:</b> INFINITE" +
             "  |  <b>DAMAGE:</b> INSTA-KILL" +
             "  |  <b>SPEED:</b> " + enemy.data.variants[enemy.variant].speed;
        }
        else
        {
            stats.text = "<b>HEALTH:</b> " + enemy.data.variants[enemy.variant].health +
                         "  |  <b>DAMAGE:</b> " + enemy.data.variants[enemy.variant].damage +
                         "  |  <b>SPEED:</b> " + enemy.data.variants[enemy.variant].speed;
        }
        stats.color = secondaryColor;
        icon.sprite = enemy.data.icon;
        icon.color = primaryColor;
        border.color = primaryColor;
        background.color = new Color(primaryColor.r, primaryColor.g, primaryColor.b, 0.2f);
    }
}
