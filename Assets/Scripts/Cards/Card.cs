using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    // Get card data
    private CardData cardData;
    private CardData redrawCard;
    public int cardNumber;

    // Card position
    public Vector3 cardPosition;
    public Vector3 synergyPosition;
    public float verticalAdjustment = 100f;
    public float animationSpeed = 0.25f;
    public float fadeInSpeed = 0.25f;

    // Card elements
    public Image model;
    public Image image;
    public TextMeshProUGUI title, desc, level, type, effectOne, effectTwo, amount;

    // Canvas group
    public CanvasGroup canvasGroup;
    private AudioSource audioSource;
    private bool redraw = false;
    public bool redrawing = false;
    private SynergyData synergy;

    // Get the canvas group
    public void Start() 
    { 
        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();
    }

    // Check for redraw
    public void Update()
    {
        if (redrawing && !LeanTween.isTweening())
            Set(redrawCard, true);
    }

    // Set card function
    public void Set(CardData card, bool redrawing = false, bool synergyCard = false)
    {
        // Set card data
        cardData = card;
        redraw = false;

        // Set card information
        model.color = card.color;
        image.color = card.color;
        title.text = card.name.ToUpper();
        title.color = card.color;
        level.color = card.color;
        type.color = card.color;
        amount.color = card.color;

        bool useBase = true;

        // Show amount
        if (amount.text != null)
        {
            int cardAmount = Deck.active.GetCardAmount(card);
            if (cardAmount >= 0) amount.text = cardAmount + "/" + card.maximumAmount;
            else amount.text = "";
        }

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
                image.sprite = weaponData.sprite;
                if (weapon.level == weaponData.baseLevels.Count)
                {
                    desc.text = "Ready for synergy";
                    level.text = "LEVEL MAX";
                }
                else
                {
                    desc.text = weaponData.baseLevels[weapon.level].description;
                    level.text = "LEVEL " + weapon.level + 1;
                }
            }

            // Set effects
            effectOne.gameObject.SetActive(false);
            effectTwo.gameObject.SetActive(false);
        }

        else if (card is StatData)
        {
            // Set type
            StatData statData = (StatData)card;
            type.text = "Stat";

            // Calculate effect
            StatValue statType = statData.value;
            effectOne.gameObject.SetActive(true);
            effectOne.text = statType.type.ToString() + ": " + Formatter.Round(
                Deck.GetStat(statType.type)) + GetDifference(statType);
            effectTwo.gameObject.SetActive(false);
        }

        else if (card is PrimaryData)
        {
            // Set type
            PrimaryData primary = (PrimaryData)card;
            type.text = "Primary";

            // Calculate effect
            StatValue statType = primary.stats[0];
            effectOne.gameObject.SetActive(true);
            effectOne.text = statType.type.ToString() + ": " + Formatter.Round(
                Deck.GetStat(statType.type)) + GetDifference(statType);
            
            // Check if second effect available
            if (primary.stats.Count >= 2)
            {
                // Show effect
                statType = primary.stats[1];
                effectTwo.gameObject.SetActive(true);
                effectTwo.text = statType.type.ToString() + ": " + Formatter.Round(
                    Deck.GetStat(statType.type)) + GetDifference(statType);
            }
            else effectTwo.gameObject.SetActive(false);
        }

        else if (card is SecondaryData)
        {
            // Set type
            SecondaryData secondary = (SecondaryData)card;
            type.text = "Secondary";

            // Calculate effect
            if (Deck.secondary != secondary)
            {
                if (Deck.secondary == null)
                {
                    // Set effects to none
                    effectOne.gameObject.SetActive(false);
                    effectTwo.gameObject.SetActive(false);
                }
                else
                {
                    redraw = true;
                    effectOne.gameObject.SetActive(true);
                    effectOne.text = "<color=orange>Will replace " + Deck.secondary.name + "!";
                    effectTwo.gameObject.SetActive(true);
                    effectTwo.text = "<color=white>Free draw if picked!";
                }
            }
            else
            {
                // Dont use base
                useBase = false;

                // Get the current secondary instance
                Secondary secondaryInstance = Deck.active.GetSecondaryInstance();

                // Set effects to none
                if (secondaryInstance.level < Deck.secondary.levels.Count)
                {
                    // Set stat value
                    StatValue statType = Deck.secondary.levels[secondaryInstance.level].stat;
                    effectOne.gameObject.SetActive(true);
                    float value = secondaryInstance.GetStat(statType.type);

                    // Get new value
                    float newValue;
                    if (statType.multiply) newValue = (value * statType.modifier) - value;
                    else newValue = statType.modifier;

                    // Format new value
                    string display = statType.type.ToString() + ": " + Formatter.Round(value);
                    
                    // Get coloring
                    if (statType.positive) display += " <color=green>(";
                    else display += " <color=red>(";
                    if (newValue >= 0) display += "+";

                    // Append new value
                    display += Formatter.Round(newValue) + ")";
                    effectOne.text = display;

                    // Set effect two to none
                    effectTwo.gameObject.SetActive(false);

                    // Set description of card level
                    image.sprite = card.sprite;
                    desc.text = secondaryInstance.GetUpgradeString();
                    level.text = "LEVEL " + secondaryInstance.level;
                }
                else
                {
                    // Set description of card level
                    image.sprite = card.sprite;
                    desc.text = "Card is maxed!";
                    level.text = "LEVEL MAX";
                }
            }
        }

        else if (card is ChromaData)
        {
            // Set type
            type.text = "Chroma";
            effectOne.gameObject.SetActive(false);
            effectTwo.gameObject.SetActive(false);
        }
        
        if (useBase)
        {
            image.sprite = card.sprite;
            desc.text = card.description;
            int cardAmount = Deck.active.GetCardAmount(cardData);
            if (cardAmount > 0)
            {
                level.text = cardAmount + " IN DECK";
            }
            else
            {
                level.text = "NEW CARD";
            }
        }

        // Animate the card
        if (!synergyCard)
        {
            if (!redrawing)
            {
                ResetCard();

                // Set card anim speed
                if (Settings.skipCardAnim)
                {
                    transform.localPosition = cardPosition;
                    canvasGroup.alpha = 1f;
                }
                else if (Settings.fastCardAnim)
                {
                    LeanTween.moveLocal(gameObject, cardPosition, animationSpeed / 2f);
                    LeanTween.alphaCanvas(canvasGroup, 1f, fadeInSpeed / 2f);
                }
                else
                {
                    LeanTween.moveLocal(gameObject, cardPosition, animationSpeed);
                    LeanTween.alphaCanvas(canvasGroup, 1f, fadeInSpeed);
                }

                // Play card sound
                if (!Settings.skipCardAnim)
                {
                    audioSource.volume = Settings.sound;
                    audioSource.Play();
                }
            }
            else
            {
                this.redrawing = false;
                LeanTween.rotateLocal(gameObject, Vector3.zero, animationSpeed / 3f);
            }
        }

        else level.text = "SYNERGY CARD";
    }

    // Card clicked
    public void OnClick(bool synergy)
    {
        if (synergy) Dealer.active.PickSynergyCard(this.synergy);
        else Dealer.active.PickCard(cardData, redraw, cardNumber);
    }

    // Reset card
    public void ResetCard()
    {
        transform.localPosition = new Vector3(cardPosition.x, cardPosition.y - verticalAdjustment, 0);
        canvasGroup.alpha = 0f;
    }

    // Redraw card
    public void RedrawCard(CardData newCard)
    {
        redrawCard = newCard;
        LeanTween.rotateLocal(gameObject, new Vector3(0, 90f, 0), animationSpeed / 3f);
        audioSource.volume = Settings.sound;
        audioSource.Play();
        redrawing = true;
    }

    // Calculate stat
    public string GetDifference(StatValue stat)
    {
        // New and current amounts
        float defaultAmount = Deck.GetDefaultStat(stat.type);
        float newAmount;
        float total;

        // Calculate the stat
        if (stat.type == Stat.Health)
        {
            total = stat.modifier;
        }
        else
        {
            if (stat.multiply) newAmount = (defaultAmount + Deck.GetAdditions(stat.type)) * (Deck.GetMultiplier(stat.type) * stat.modifier);
            else newAmount = (defaultAmount + Deck.GetAdditions(stat.type) + stat.modifier) * Deck.GetMultiplier(stat.type);
            total = newAmount - Deck.GetStat(stat.type);
        }

        // Calculate color
        string color = "<color=green> (";
        if (!stat.positive) color = "<color=red> (";
        if (total >= 0) color += "+";

        // Return formatted string
        return color + Formatter.Round(total) + ")";
    }

    // Play synergy animation
    public void Synergize(CardData card, float speed, float moveDelay)
    {
        // Set canvas to 0
        canvasGroup.alpha = 0f;

        // Set card data
        cardData = card;
        redraw = false;

        // Set card information
        model.color = card.color;
        image.color = card.color;
        title.text = card.name.ToUpper();
        title.color = card.color;
        level.color = card.color;
        type.color = card.color;

        // Set description
        image.sprite = card.sprite;
        desc.text = "Card is maxed";
        level.text = "LEVEL MAX";

        // Smooth move in
        transform.localPosition = cardPosition;
        LeanTween.alphaCanvas(canvasGroup, 1f, speed).setDelay(1f);
        LeanTween.moveLocal(gameObject, synergyPosition, 0.3f).
            setEase(LeanTweenType.easeInExpo).setDelay(1f + moveDelay);
    }

    // Set synergy
    public void SetSynergy(SynergyData synergy) { this.synergy = synergy; }
}
