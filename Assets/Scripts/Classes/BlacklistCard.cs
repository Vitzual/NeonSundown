using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlacklistCard : MonoBehaviour
{
    // Components
    public new TextMeshProUGUI name;
    public Image icon;
    public Image model;

    public void Set(CardData card)
    {
        // Set components
        name.text = card.name.ToUpper();
        icon.sprite = card.sprite;

        // Set colors
        name.color = card.color;
        icon.color = card.color;
        model.color = card.color;
    }
}
