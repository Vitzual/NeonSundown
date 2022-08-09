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
    public Vector3 upgradePosition, upgradeSize, originalSize;
    public float verticalAdjustment = 100f;
    public float animationSpeed = 0.25f;
    public float fadeInSpeed = 0.25f;

    // Card elements
    public Image border, background;
    public Image image;
    public TextMeshProUGUI title, desc, level, type, effectOne, effectTwo, amount;

    // Canvas group
    public CanvasGroup canvasGroup;
    private AudioSource audioSource;
    private bool redraw = false;
    private float cooldown = 0f;
    public bool redrawing = false;
    private SynergyData synergy;
    private OnHoverAdjustScale hover;

    // Get the canvas group
    public void Start() 
    { 
        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();
        hover = GetComponent<OnHoverAdjustScale>();
    }

    // Check for redraw
    public void Update()
    {
        if (redrawing)
        {
            if (!LeanTween.isTweening() || cooldown <= 0)
                Set(redrawCard, true);
            cooldown -= Time.deltaTime;
        }
    }

    // Set card function
    public void Set(CardData card, bool redrawing = false, bool synergyCard = false, int synergyTier = 1)
    {
        // Set card data
        cardData = card;
        redraw = false;

        // Set card information
        border.color = card.color;
        background.color = new Color(card.color.r * 0.25f, card.color.g * 0.25f, card.color.b * 0.25f, 1f);
        image.color = card.color;
        title.text = card.name.ToUpper();
        title.color = card.color;
        level.color = card.color;
        type.color = card.color;

        // Set card description
        if (card.enableDescriptionOverrides)
        {
            int amount = Deck.active.GetCardAmount(card);
            if (card.levelDescriptionOverrides.ContainsKey(amount))
                desc.text = card.levelDescriptionOverrides[amount].ToUpper();
            else desc.text = card.description.ToUpper();
        }
        else desc.text = card.description.ToUpper();

        bool useBase = true;

        // Show amount
        if (amount != null)
        {
            amount.color = card.color;
            int cardAmount = Deck.active.GetCardAmount(card);
            if (cardAmount >= 0) amount.text = cardAmount + "/" + card.maximumAmount;
            else amount.text = "";
        }

        // Check if card is a weapon or not
        if (card is WeaponData)
        {
            // Set type
            type.text = "WEAPON";
            type.color = card.color;

            // Enable new upgrade
            if (Deck.active.HasCard(card))
            {
                useBase = false;
                image.sprite = card.sprite;
                level.text = "UPGRADE AVAILABLE!";
            }
        }

        else if (card is HelperData)
        {
            // Set type
            type.text = "HELPER";
            type.color = card.color;

            // Enable new upgrade
            if (Deck.active.HasCard(card))
            {
                useBase = false;
                image.sprite = card.sprite;
                level.text = "UPGRADE AVAILABLE!";
            }
        }

        else if (card is StatData)
        {
            // Set type
            StatData stat = (StatData)card;
            type.text = "STAT";

            /* Calculate effect
            StatValue statType = stat.stats[0];
            effectOne.gameObject.SetActive(true);
            effectOne.text = "<b>" + statType.type.ToString() + ":</b> " + Formatter.Round(
                Deck.GetStat(statType.type)) + GetDifference(statType);
            
            // Check if second effect available
            if (stat.stats.Count >= 2)
            {
                // Show effect
                statType = stat.stats[1];
                effectTwo.gameObject.SetActive(true);
                effectTwo.text = "<b>" + statType.type.ToString() + ":</b> " + Formatter.Round(
                    Deck.GetStat(statType.type)) + GetDifference(statType);
            }
            else effectTwo.gameObject.SetActive(false);*/
        }

        else if (card is SecondaryData)
        {
            // Set type
            type.text = "SECONDARY";
            type.color = card.color;

            // Enable new upgrade
            if (Deck.active.HasCard(card))
            {
                useBase = false;
                image.sprite = card.sprite;
                level.text = "UPGRADE AVAILABLE!";
            }
        }

        else if (card is ChromaData)
        {
            // Set type
            type.text = "CHROMA";
        }
        
        if (useBase)
        {
            image.sprite = card.sprite;
            int cardAmount = Deck.active.GetCardAmount(cardData);
            if (cardAmount > 0) level.text = cardAmount + " IN DECK";
            else level.text = "NEW CARD";
        }

        // Animate the card
        if (!synergyCard)
        {
            if (!redrawing)
            {
                ResetCard();

                LeanTween.moveLocal(gameObject, cardPosition, animationSpeed / 2f);
                LeanTween.alphaCanvas(canvasGroup, 1f, fadeInSpeed / 2f);
                audioSource.volume = Settings.sound;
                audioSource.Play();
            }
            else
            {
                this.redrawing = false;
                LeanTween.rotateLocal(gameObject, Vector3.zero, animationSpeed / 3f);
            }
        }

        else level.text = "TIER " + synergyTier + " SYNERGY";
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
        transform.localScale = originalSize;
        transform.localPosition = new Vector3(cardPosition.x, cardPosition.y - verticalAdjustment, 0);
        canvasGroup.alpha = 0f;
    }

    // Redraw card
    public void RedrawCard(CardData newCard)
    {
        if (!redrawing)
        {
            redrawCard = newCard;
            LeanTween.rotateLocal(gameObject, new Vector3(0, 90f, 0), animationSpeed / 3f);
            cooldown = animationSpeed / 3f;
            audioSource.volume = Settings.sound;
            audioSource.Play();
            redrawing = true;
        }
    }
    
    public void MoveToUpgradePosition()
    {
        originalSize = transform.localScale;
        LeanTween.moveLocal(gameObject, upgradePosition, 0.25f).setEase(LeanTweenType.easeInExpo);
        LeanTween.scale(gameObject, upgradeSize, 0.25f).setEase(LeanTweenType.easeInExpo);
    }

    public void MoveToNormalPosition()
    {
        LeanTween.moveLocal(gameObject, cardPosition, 0.25f).setEase(LeanTweenType.easeInExpo).setDelay(0.25f);
        LeanTween.scale(gameObject, originalSize, 0.25f).setEase(LeanTweenType.easeInExpo).setDelay(0.25f);
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
        // Check if card is null
        if (card == null) return;

        // Set canvas to 0
        canvasGroup.alpha = 0f;

        // Set card data
        cardData = card;
        redraw = false;

        // Set card information
        border.color = card.color;
        background.color = new Color(card.color.r * 0.25f, card.color.g * 0.25f, card.color.b * 0.25f, 1f);
        image.color = card.color;
        title.text = card.name.ToUpper();
        title.color = card.color;
        level.color = card.color;
        type.color = card.color;
        amount.color = card.color;
        amount.text = card.maximumAmount + "/" + card.maximumAmount;

        // Set description
        image.sprite = card.sprite;
        desc.text = "CARD IS MAXED";
        level.text = "LEVEL MAX";

        // Smooth move in
        transform.localPosition = cardPosition;
        LeanTween.alphaCanvas(canvasGroup, 1f, speed).setDelay(1f);
        LeanTween.moveLocal(gameObject, synergyPosition, 0.3f).
            setEase(LeanTweenType.easeInExpo).setDelay(1f + moveDelay);
    }

    // Set synergy
    public void SetSynergy(SynergyData synergy) { this.synergy = synergy; }
    public CardData GetCard() { return cardData; }
}
