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
    public TextMeshProUGUI positiveDesc, negativeDesc, quality;
    public Image background, border, indent;
    public Color badColor, goodColor, greatColor, uniqueColor;
    [HideInInspector] public CanvasGroup canvasGroup;

    public void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Set(CardData.Upgrade upgrade)
    {
        // Set upgrade variable
        this.upgrade = upgrade;
      
        // Set upgrade components
        name.text = upgrade.name.ToUpper();

        // Set positive text
        float formatted = upgrade.positiveEffect;
        if (upgrade.positiveMultiplier)
        {
            if (upgrade.positiveReduction) formatted = (formatted * 100) - 100;
            else formatted = 100 - (formatted * 100);
        }
        positiveDesc.text = upgrade.positiveDesc.Replace("<value>", Formatter.Round(formatted, 1));

        // Set negative text
        formatted = upgrade.negativeEffect;
        if (upgrade.negativeMultiplier)
        {
            if (upgrade.negativeReduction) formatted = (formatted * 100) - 100;
            else formatted = 100 - (formatted * 100);
        }
        negativeDesc.text = upgrade.positiveDesc.Replace("<value>", Formatter.Round(formatted, 1));

        // Set quality text
        quality.text = upgrade.quality.ToString().ToUpper() + " QUALITY";

        // Set border color
        switch (upgrade.quality)
        {
            case Quality.Bad:
                border.color = badColor;
                break;
            case Quality.Good:
                border.color = goodColor;
                break;
            case Quality.Great:
                border.color = greatColor;
                break;
            default:
                border.color = uniqueColor;
                break;
        }

        // Set other colors
        indent.color = border.color;
        background.color = new Color(border.color.r, border.color.g, border.color.b, 0.2f);
    }

    public void ApplyUpgrade()
    {
        Dealer.active.ApplyUpgrade(upgrade);
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
