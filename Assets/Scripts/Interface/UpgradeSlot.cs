using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour
{
    // All upgrade slot variables
    private UpgradeData upgrade;
    public new TextMeshProUGUI name;
    public TextMeshProUGUI positiveDesc, negativeDesc, quality;
    public Image background, border, indent;
    public Color badColor, goodColor, greatColor, uniqueColor;
    [HideInInspector] public CanvasGroup canvasGroup;
    private int qualityIndex;

    public void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Set(UpgradeData upgrade, int qualityIndex)
    {
        // Set upgrade variable
        this.upgrade = upgrade;
        this.qualityIndex = qualityIndex;

        // Set upgrade components
        name.text = upgrade.name.ToUpper();

        // Set positive text
        float formatted = upgrade.qualities[qualityIndex].positiveEffect;
        if (upgrade.positiveMultiplier)
        {
            if (!upgrade.positiveReduction) formatted = (formatted * 100) - 100;
            else formatted = 100 - (formatted * 100);
        }
        positiveDesc.text = upgrade.positiveDesc.Replace("<value>", Formatter.Round(formatted, 1));

        // Set negative text
        formatted = upgrade.qualities[qualityIndex].negativeEffect;
        if (upgrade.negativeMultiplier)
        {
            if (!upgrade.negativeReduction) formatted = (formatted * 100) - 100;
            else formatted = 100 - (formatted * 100);
        }
        negativeDesc.text = upgrade.negativeDesc.Replace("<value>", Formatter.Round(formatted, 1));

        // Set border color
        switch (upgrade.qualities[qualityIndex].quality)
        {
            case Quality.Bad:
                border.color = badColor;
                quality.text = "BAD QUALITY";
                break;
            case Quality.Good:
                border.color = goodColor;
                quality.text = "GOOD QUALITY";
                break;
            case Quality.Great:
                border.color = greatColor;
                quality.text = "GREAT QUALITY";
                break;
            default:
                border.color = uniqueColor;
                quality.text = "UNIQUE QUALITY";
                break;
        }

        // Set other colors
        indent.color = border.color;
        background.color = new Color(border.color.r, border.color.g, border.color.b, 0.2f);
    }

    public void ApplyUpgrade()
    {
        Dealer.active.ApplyUpgrade(upgrade, qualityIndex);
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
