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

    // Card elements
    public Image model;
    public Image image;
    public TextMeshProUGUI title;
    public TextMeshProUGUI desc;
    public TextMeshProUGUI level;

    // Canvas group
    private CanvasGroup canvasGroup;

    // Get the canvas group
    public void Start() { canvasGroup = GetComponent<CanvasGroup>(); }

    // Set card function
    public void Set(CardData card)
    {
        // Set card data
        cardData = card;

        // Set card information
        model.color = card.color;
        image.sprite = card.sprite;
        image.color = card.color;
        title.text = card.name.ToUpper();
        title.color = card.color;

        // Check if card is a weapon or not
        if (card is WeaponData)
        {
            WeaponData weaponData = (WeaponData)card;
            Weapon weapon = Deck.active.GetWeaponInstance(weaponData);
            if (weapon != null)
            {
                if (weapon.prestige)
                {
                    desc.text = weaponData.prestigeLevels[weapon.level].description;
                    if (weapon.level == weaponData.prestigeLevels.Count)
                        level.text = "PRESTIGE MAX";
                    else level.text = "PRESTIGE " + weapon.level;
                }
                else
                {
                    desc.text = weaponData.baseLevels[weapon.level].description;
                    if (weapon.level == weaponData.baseLevels.Count)
                        level.text = "LEVEL MAX";
                    else level.text = "LEVEL " + weapon.level;
                }
            }
        }
        else 
        {
            desc.text = card.description;
            int cardAmount = Deck.active.GetCardAmount(cardData);
            if (cardAmount > 0) level.text = cardAmount + " IN DECK";
            else level.text = "NEW CARD";
        }

        // Animate the card
        transform.localPosition = new Vector3(cardPosition.x, cardPosition.y - 25f, 0);
        canvasGroup.alpha = 0f;
        LeanTween.moveLocal(gameObject, cardPosition, 0.25f);
        LeanTween.alphaCanvas(canvasGroup, 1f, 0.25f);
    }

    // Card clicked
    public void OnClick()
    {
        Dealer.active.PickCard(cardData, cardNumber);
    }
}
