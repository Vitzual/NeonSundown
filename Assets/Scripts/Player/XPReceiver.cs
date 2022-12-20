using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XPReceiver : MonoBehaviour
{
    // XP Variables
    public List<float> levels;
    private int level = 0;
    private float xp = 0;
    private float rankup = 50;
    private float xpMultiplier = 1;
    public float rankupMultiplier;
    public ProgressBar xpBar;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;
    public bool xpHealing = false;

    public void Awake()
    {
        // Reset XP and level
        level = 0;
        xp = 0;

        // Set starting rankup cost
        if (levels.Count > 0)
            rankup = levels[0];
        else rankup = 25;

        // Update UI element
        levelText.text = "LEVEL " + level;
        xpText.text = Mathf.Round(xp) + " / " + Mathf.Round(rankup) + "xp";
        xpBar.currentPercent = (float)xp / rankup * 100;
        xpBar.UpdateUI();
    }

    public void Start()
    {
        // Set target
        Events.active.XPReceiverStart(this);
    }

    public void AddXP(float amount)
    {
        // Add the XP amount
        float addAmount = amount * xpMultiplier;
        Levels.AddXP(addAmount);
        xp += addAmount;

        // Check if healing enabled
        if (xpHealing) Ship.Heal(addAmount / 10f);

        // Check if XP over rankup
        if (xp >= rankup)
        {
            // Increase level
            level += 1;
            xp -= rankup;
            if (levels.Count <= level)
            {
                rankup = (int)(rankup * rankupMultiplier);
                if (rankup > 80000) rankup = 80000;
            }
            else rankup = levels[level];

            // Set text
            levelText.text = "LEVEL " + level;
            Dealer.active.OpenDealer();

            Debug.Log("Levelled up to " + level);
        }

        // Set XP bar
        xpText.text = Mathf.Round(xp) + " / " + Mathf.Round(rankup) + "xp";
        xpBar.currentPercent = (float)xp / rankup * 100;
        xpBar.UpdateUI();
    }

    public void SetXPMultiplier(float amount)
    {
        xpMultiplier = amount; 
    }
    
    public float GetXPMultiplier() 
    {
        return xpMultiplier; 
    }
}
