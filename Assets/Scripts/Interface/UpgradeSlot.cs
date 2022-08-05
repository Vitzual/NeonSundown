using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour
{
    // All upgrade slot variables
    private CardData.Upgrade upgrade;
    public new TextMeshProUGUI name;
    public TextMeshProUGUI desc, quality;
    public Image background, border, indent;
    public Color badColor, goodColor, greatColor;
    [HideInInspector] public CanvasGroup canvasGroup;
    private float effect;

    public void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Set(CardData.Upgrade upgrade)
    {
        // Set upgrade variable
        this.upgrade = upgrade;

        // Get random value from upgrade
        effect = Random.Range(upgrade.minValue, upgrade.maxValue);

        // Format effect amount
        string formatted = Formatter.Round(effect, 1);
        if (upgrade.multiply) 
        { 
            if (upgrade.lowerIsBetter) formatted = Formatter.Round(100 - (effect * 100)) + "%";
            else formatted = Formatter.Round((effect * 100) - 100) + "%";
        }

        // Set upgrade components
        name.text = upgrade.name.ToUpper();
        desc.text = upgrade.desc.Replace("<value>", formatted);

        // Set quality amount
        float thresholdAmount = (upgrade.maxValue - upgrade.minValue) / 3;
        float low = thresholdAmount, mid = low + thresholdAmount;
        if (upgrade.lowerIsBetter)
        {
            if (effect <= low)
            {
                quality.text = "GREAT QUALITY";
                border.color = greatColor;
            }
            else if (effect <= mid)
            {
                quality.text = "GOOD QUALITY";
                border.color = goodColor;
            }
            else
            {
                quality.text = "BAD QUALITY";
                border.color = badColor;
            }
        }
        else
        {
            if (effect > mid)
            {
                quality.text = "GREAT QUALITY";
                border.color = greatColor;
            }
            else if (effect > low)
            {
                quality.text = "GOOD QUALITY";
                border.color = goodColor;
            }
            else
            {
                quality.text = "BAD QUALITY";
                border.color = badColor;
            }
        }

        // Set other colors
        indent.color = border.color;
        background.color = new Color(border.color.r, border.color.g, border.color.b, 0.2f);
    }

    public void ApplyUpgrade()
    {
        Dealer.active.ApplyUpgrade(upgrade, effect);
    }

    public void Toggle(bool toggle)
    {
        if (toggle)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
