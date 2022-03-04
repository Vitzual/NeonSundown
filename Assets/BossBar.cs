using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    // Enemy data variable
    protected Enemy enemy;

    // Interface variables
    public ProgressBar bar;
    public new TextMeshProUGUI name;
    public Image icon;
    private CanvasGroup canvasGroup;

    // Internal hidden flag
    private bool isHidden = true;

    // Grab the canvas group on start
    public void Start()
    {
        Events.active.onBossSpawned += Set;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Set the bar
    public void Set(Boss boss, Enemy enemy)
    {
        // Set enemy data
        this.enemy = enemy;

        // Set the new enemy info
        name.text = enemy.name;
        icon.sprite = boss.bossModel;
        icon.color = VariantPalette.GetPrimaryColor(enemy.GetData().variant);

        // Show the bar
        canvasGroup.alpha = 1f;
        isHidden = false;
    }

    // Update is called once per frame
    public void Update()
    {
        if (!isHidden)
        {
            if (enemy != null)
            {
                bar.currentPercent = enemy.GetHealth() / enemy.GetMaxHealth() * 100f;
                bar.UpdateUI();
            }
            else
            {
                canvasGroup.alpha = 0f;
                isHidden = true;
            }
        }
    }
}
