using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    // Get card data
    [SerializeField]
    private CardData cardData;
    public int cardNumber;

    // Card position
    public Vector3 cardPosition;
    public float verticalAdjustment = 100f;
    public float animationSpeed = 0.25f;
    public float fadeInSpeed = 0.25f;

    // Card elements
    public Image model;
    public Image image;
    public TextMeshProUGUI title;
    public TextMeshProUGUI desc;
    public TextMeshProUGUI level;
    public TextMeshProUGUI type;
    public TextMeshProUGUI effectOne, effectTwo;

    // Canvas group
    public CanvasGroup canvasGroup;

    // Get the canvas group
    public void Start() { canvasGroup = GetComponent<CanvasGroup>(); }

    // Set card function
    public void Set(CardData card)
    {
        // Set card data
        cardData = card;

        // Set card information
        model.color = card.color;
        image.color = card.color;
        title.text = card.name.ToUpper();
        title.color = card.color;
        level.color = card.color;
        type.color = card.color;

        bool useBase = true;

        // Check if card is a weapon or not
        if (card is WeaponData)
        {
            // Set type
            type.text = "Weapon";
            type.color = card.color;

            WeaponData weaponData = (WeaponData)card;
            Weapon weapon = Deck.active.GetWeaponInstance(weaponData);
            if (weapon != null)
            {
                useBase = false;

                if (weapon.prestige)
                {
                    image.sprite = weaponData.prestigeSprite;

                    if (weapon.level == weaponData.prestigeLevels.Count)
                    {
                        desc.text = "Card is maxed";
                        level.text = "PRESTIGE MAX";
                    }
                    else
                    {
                        desc.text = weaponData.prestigeLevels[weapon.level].description;
                        level.text = "PRESTIGE " + weapon.level;
                    }
                }
                else
                {
                    image.sprite = weaponData.sprite;

                    if (weapon.level == weaponData.baseLevels.Count)
                    {
                        desc.text = "Sacrifice the card to enter prestige levels";
                        level.text = "LEVEL MAX";
                    }
                    else
                    {
                        desc.text = weaponData.baseLevels[weapon.level].description;
                        level.text = "LEVEL " + weapon.level;
                    }
                }
            }

            // Set effects
            effectOne.gameObject.SetActive(false);
            effectTwo.gameObject.SetActive(false);
        }

        else if (card is StatData)
        {
            // Set type
            StatData stat = (StatData)card;
            type.text = "Stat";

            // Get amounts
            float newAmount;
            float currentAmount = Deck.GetStat(stat.type);

            // Calculate effect
            if (stat.multiply) newAmount = (currentAmount + Deck.GetAdditions(stat.type)) * (Deck.GetMultiplier(stat.type) * stat.modifier);
            else newAmount = (currentAmount + Deck.GetAdditions(stat.type) + stat.modifier) * Deck.GetMultiplier(stat.type);
            float total = newAmount - currentAmount;

            // Calculate color
            string color = "<color=green> (";
            if (total >= 0) color += "+";

            // Show effect
            effectOne.gameObject.SetActive(true);
            effectOne.text = stat.type.ToString() + ": " + Formatter.Round(Deck.GetStat(stat.type)) +
                color + Formatter.Round(total) + ")";
            effectTwo.gameObject.SetActive(false);
        }

        // I WILL REDO THIS
        else if (card is PrimaryData)
        {
            // Set type
            PrimaryData primary = (PrimaryData)card;
            type.text = "Primary";
            float newAmount;
            float currentAmount;

            // Calculate effect
            PrimaryData.StatType stat = primary.stats[0];
            currentAmount = Deck.GetStat(stat.type);
            if (stat.multiply) newAmount = (currentAmount + Deck.GetAdditions(stat.type)) * (Deck.GetMultiplier(stat.type) * stat.modifier);
            else newAmount = (currentAmount + Deck.GetAdditions(stat.type) + stat.modifier) * Deck.GetMultiplier(stat.type);
            float total = newAmount - currentAmount;

            // Calculate color
            string color = "<color=green> (";
            if (!stat.positive) color = "<color=red> (";
            if (total >= 0) color += "+";

            // Show effect
            effectOne.gameObject.SetActive(true);
            effectOne.text = stat.type.ToString() + ": " + Formatter.Round(Deck.GetStat(stat.type)) +
                color + Formatter.Round(total) + ")";

            // Check if second effect available
            if (primary.stats.Count >= 2)
            {
                // Calculate effect
                stat = primary.stats[1];
                currentAmount = Deck.GetStat(stat.type);
                if (stat.multiply) newAmount = (currentAmount + Deck.GetAdditions(stat.type)) * (Deck.GetMultiplier(stat.type) * stat.modifier);
                else newAmount = (currentAmount + Deck.GetAdditions(stat.type) + stat.modifier) * Deck.GetMultiplier(stat.type);
                total = newAmount - currentAmount;

                // Calculate color
                color = "<color=green> (";
                if (!stat.positive) color = "<color=red> (";
                if (total >= 0) color += "+";

                // Show effect
                effectTwo.gameObject.SetActive(true);
                effectTwo.text = stat.type.ToString() + ": " + Formatter.Round(Deck.GetStat(stat.type)) +
                    color + Formatter.Round(total) + ")";
            }
            else effectTwo.gameObject.SetActive(false);
        }
        
        if (useBase)
        {
            image.sprite = card.sprite;
            desc.text = card.description;
            int cardAmount = Deck.active.GetCardAmount(cardData);
            if (cardAmount > 0) level.text = cardAmount + " IN DECK";
            else level.text = "NEW CARD";
        }

        // Animate the card
        ResetCard();
        LeanTween.moveLocal(gameObject, cardPosition, animationSpeed);
        LeanTween.alphaCanvas(canvasGroup, 1f, fadeInSpeed);
    }

    // Card clicked
    public void OnClick()
    {
        Dealer.active.PickCard(cardData, cardNumber);
    }

    // Reset card
    public void ResetCard()
    {
        transform.localPosition = new Vector3(cardPosition.x, cardPosition.y - verticalAdjustment, 0);
        canvasGroup.alpha = 0f;
    }
}
