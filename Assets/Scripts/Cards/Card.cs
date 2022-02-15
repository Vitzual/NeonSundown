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

        bool useBase = true;

        // Check if card is a weapon or not
        if (card is WeaponData)
        {
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
