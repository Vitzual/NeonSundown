using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public class Dealer : MonoBehaviour
{
    // Active instance
    public static Dealer active;
    public static bool isOpen;

    // Gameobject for controls
    public GameObject controlsOne;
    public GameObject controlsTwo;
    public GameObject controlsThree;

    // List of card slots
    [BoxGroup("Card Options")]
    public List<Card> cardSlots;
    [BoxGroup("Card Options")]
    public List<UpgradeSlot> upgrades;
    [BoxGroup("Card Options")]
    private List<CardData> dealList;
    [BoxGroup("Card Options")]
    public Vector3 upgradeNormalSize, upgradeTargetSize;
    [BoxGroup("Card Options")]
    public int cardsToPick = 3;
    [BoxGroup("Card Options")]
    public float cardDealSpeed = 0.5f;
    [BoxGroup("Card Options")]
    public float fastDealSpeed = 4f;

    // Pitch in variables
    [BoxGroup("Music Options")]
    public AudioSource music;
    [BoxGroup("Music Options")]
    public float pitchSpeed = 0.1f;
    [BoxGroup("Music Options")]
    public float pitchDown = 0.8f;

    // Interface variables
    [BoxGroup("Interface Options")]
    public CanvasGroup title;
    [BoxGroup("Interface Options")]
    public TextMeshProUGUI redraws;
    [BoxGroup("Interface Options")]
    public TextMeshProUGUI burns;
    [BoxGroup("Interface Options")]
    public CanvasGroup dealOptions, upgradeOptions;
    [BoxGroup("Interface Options")]
    public float titleFadeInSpeed = 0.01f;
    [BoxGroup("Interface Options")]
    public Image background;
    [BoxGroup("Interface Options")]
    public float bgFadeInSpeed = 0.01f;
    [BoxGroup("Interface Options"), Range(0, 1)]
    public float bgAlphaTarget = 0.5f;
    [BoxGroup("Interface Options")]
    public EventSystem eventSystem;
    [BoxGroup("Interface Options")]
    public GraphicRaycaster eventRaycaster;
    [BoxGroup("Interface Options")]
    public AudioClip upgradeSound;

    // Debug switch
    [BoxGroup("Debug Flags")]
    public bool debugSwitch = false;

    // Internal flags
    private bool dealCards = false;
    private bool cardsDealt = false;
    private bool canvasSet = false;
    private float cardCooldown = 0.5f;
    private int cardNumber = 0;
    private int redrawsLeft = 0;
    private int burnsLeft = 1;

    // Private components 
    private CanvasGroup canvasGroup;
    private List<CardData> pickedList;
    private List<CardData> pickedCards;
    private Card upgradingCard;
    private bool isUpgrading = false;

    // On start get private components 
    public void Start()
    {
        active = this;
        canvasGroup = GetComponent<CanvasGroup>();
        pickedList = new List<CardData>();
        pickedCards = new List<CardData>();
    }

    // While open, rotate
    public void Update()
    {
        // Check if input from re-draw
        if (isOpen && cardsDealt && Input.GetKeyDown(Keybinds.secondary)) RedrawCard();
        else if (isOpen && cardsDealt && Input.GetKeyDown(Keybinds.burn)) BurnCard();

        // If debug switch set to true, deal cards
        if (debugSwitch)
        {
            OpenDealer();
            debugSwitch = false;
        }

        // Check background
        if (background.color.a < 0.49f)
            background.color = new Color(0, 0, 0, 0.5f);

        // If dealing, fade in background and deal cards
        if (dealCards)
        {
            if (background.color.a < 0.5f)
            {
                background.color = new Color(0, 0, 0, background.color.a + 
                    (bgFadeInSpeed * Time.deltaTime * fastDealSpeed));
            }
            else
            {
                if (cardCooldown <= 0f) DealCard(cardNumber);
                else cardCooldown -= Time.deltaTime;
            }
        }

        // Pitch down music
        if (isOpen)
        {
            // Check the connected controller
            if (Controller.isControllerConnected && cardsDealt) CheckController();

            if (Settings.musicPitching && music.pitch > pitchDown)
                music.pitch -= pitchSpeed * Time.deltaTime * fastDealSpeed;

            if (!canvasSet && cardsDealt)
            {
                if (title.alpha < 1f)
                {
                    title.alpha += titleFadeInSpeed * Time.deltaTime * fastDealSpeed;
                    dealOptions.alpha += titleFadeInSpeed * Time.deltaTime * fastDealSpeed;
                }
                else canvasSet = true;
            }
        }

        // Pitch back up music after dealign
        else if (music.pitch < 1.0f)
        {
            music.pitch += pitchSpeed * Time.deltaTime * fastDealSpeed;

            if (music.pitch >= 1f)
                music.pitch = 1f;
        }
    }

    // Check controller
    public void CheckController()
    {
        // Check for redraws
        if (Input.GetAxis("D-Pad X") < 0) Redraw(cardSlots[0]);
        else if (Input.GetAxis("D-Pad X") > 0) Redraw(cardSlots[2]);
        else if (Input.GetAxis("D-Pad Y") < 0) Redraw(cardSlots[1]);
        else if (Input.GetAxis("D-Pad Y") > 0) CloseDealer();

        // Check for pick cards
        if (Input.GetKeyDown(KeyCode.JoystickButton0)) cardSlots[1].OnClick(false);
        else if (Input.GetKeyDown(KeyCode.JoystickButton1)) cardSlots[2].OnClick(false);
        else if (Input.GetKeyDown(KeyCode.JoystickButton2)) cardSlots[0].OnClick(false);
    }

    // Deal cards
    public void DealCard(int number)
    {
        // Check to make sure enough cards are in the list
        if (dealList.Count == 0 || cardSlots.Count <= number)
        {
            Debug.Log("Mismatch deck slots");
            dealCards = false;
            cardsDealt = true;
            return;
        }

        // Take the card and remove it from the list
        CardData card = PickNewCard();
        cardSlots[number].Set(card);

        // Reset card cooldown
        cardCooldown = 0.15f;

        // Check if that was the last card
        if (number + 1 == cardsToPick)
        {
            dealCards = false;
            cardsDealt = true;
        }
        else cardNumber += 1;
    }
    
    // Pick the card and add to palyer
    public void PickCard(CardData card, bool redraw, int cardNumber)
    {
        // Check if card is upgradeable
        if (card.isUpgradeable && card.upgrades.Count > 0 && Deck.active.HasCard(card))
        {
            upgradingCard = cardSlots[cardNumber];
            upgradingCard.MoveToUpgradePosition();
            ToggleUpgrades(true, card);
            for (int i = 0; i < cardSlots.Count; i++)
            {
                cardSlots[i].canvasGroup.interactable = false;
                cardSlots[i].canvasGroup.blocksRaycasts = false;
                if (i != cardNumber) LeanTween.alphaCanvas(cardSlots[i].canvasGroup, 0f, 0.15f);
            }

            // Adjust alpha canvas
            dealOptions.interactable = false;
            dealOptions.blocksRaycasts = false;
            upgradeOptions.interactable = true;
            upgradeOptions.blocksRaycasts = true;
            LeanTween.alphaCanvas(dealOptions, 0f, 0.15f);
            LeanTween.alphaCanvas(upgradeOptions, 1f, 0.15f).setDelay(0.15f);
        }
        else
        {
            // Add the thing boss man
            RuntimeStats.cardsChosen += 1;
            
            // Add this to pick list
            if (Deck.active.GetCardAmount(card) + 1 == card.maximumAmount)
            {
                if (pickedCards.Contains(card))
                {
                    Debug.Log(card.name + " is now maxed, removing from drop pool");
                    pickedCards.Remove(card);
                }
            }

            if (!pickedCards.Contains(card))
                pickedCards.Add(card);

            // Check if cards dealt
            if (!cardsDealt || cardSlots[cardNumber].redrawing) return;
            Deck.active.AddCard(card);

            // Check if card is secondary
            if (redraw) cardSlots[cardNumber].RedrawCard(PickNewCard());
            else CloseDealer();
        }
    }

    // Applies an upgrade
    public void ApplyUpgrade(UpgradeData upgrade, int quality)
    {
        // Add the thing boss man
        RuntimeStats.cardsChosen += 1;

        // Add this to pick list
        CardData card = upgradingCard.GetCard();
        if (Deck.active.GetCardAmount(card) + 1 == card.maximumAmount)
        {
            if (pickedCards.Contains(card))
            {
                Debug.Log(card.name + " is now maxed, removing from drop pool");
                pickedCards.Remove(card);
            }
        }
        Deck.active.UpgradeCard(card, upgrade, quality);

        // Adjust alpha canvas
        dealOptions.alpha = 1f;
        dealOptions.interactable = true;
        dealOptions.blocksRaycasts = true;
        upgradeOptions.alpha = 0f;
        upgradeOptions.interactable = false;
        upgradeOptions.blocksRaycasts = false;

        // Add card to dealer
        Deck.active.AddCard(card);
        CloseDealer();
    }

    public void RerollUpgrades()
    {
        ToggleUpgrades(true, upgradingCard.GetCard());
    }

    public void ToggleUpgrades(bool toggle, CardData card = null)
    {
        isUpgrading = toggle;

        if (toggle && card != null)
        {
            // Set delay
            float delay = 0.15f;

            // Create new upgrade list
            List<UpgradeData> cardUpgrades = new List<UpgradeData>(card.upgrades);

            foreach (UpgradeSlot slot in upgrades)
            {
                slot.canvasGroup.alpha = 0f;
                slot.canvasGroup.interactable = true;
                slot.canvasGroup.blocksRaycasts = true;
                UpgradeData newUpgrade = cardUpgrades[Random.Range(0, cardUpgrades.Count)];
                UpgradeData upgrade = newUpgrade;
                slot.Set(upgrade, Random.Range(0, upgrade.qualities.Count));
                slot.transform.localScale = upgradeNormalSize;
                LeanTween.scale(slot.gameObject, upgradeTargetSize, 0.15f).setEase(LeanTweenType.easeInExpo).setDelay(delay);
                LeanTween.alphaCanvas(slot.canvasGroup, 1f, 0.15f).setDelay(delay);
                LeanTween.delayedSound(upgradeSound, Deck.active.transform.position, Settings.sound).setDelay(delay);
                cardUpgrades.Remove(newUpgrade);
                delay += 0.15f;
            }
        }
        else
        {
            // Reset upgrades
            foreach (UpgradeSlot upgrade in upgrades)
            {
                upgrade.canvasGroup.alpha = 0f;
                upgrade.canvasGroup.interactable = false;
                upgrade.canvasGroup.blocksRaycasts = false;
                upgrade.transform.localScale = upgradeNormalSize;
            }

            // Reset card slots
            for (int i = 0; i < cardSlots.Count; i++)
            {
                cardSlots[i].canvasGroup.interactable = true;
                cardSlots[i].canvasGroup.blocksRaycasts = true;
            }
        }
    }

    public void CancelUpgrade()
    {
        if (!isUpgrading) return;
        isUpgrading = false;

        foreach (UpgradeSlot slot in upgrades)
        {
            LeanTween.moveLocal(slot.gameObject, new Vector3(-475, slot.transform.position.y, 0f),
                0.25f).setEase(LeanTweenType.easeInExpo);
            LeanTween.alpha(slot.gameObject, 1f, 0.25f);
        }
        if (upgradingCard != null)
            upgradingCard.MoveToNormalPosition();
    }

    // Pick synergy card
    public void PickSynergyCard(SynergyData data)
    {
        SynergyUI.Close();
        Events.active.AddSynergy(data);
        isOpen = false;
    }

    // Burns a specific card
    public void BurnCard()
    {
        // Raycast for card on interface layer
        PointerEventData m_PointerEventData = new PointerEventData(eventSystem);
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        eventRaycaster.Raycast(m_PointerEventData, results);

        // Debug the result
        foreach (RaycastResult result in results)
        {
            Card card = result.gameObject.transform.parent.GetComponent<Card>();
            if (card != null)
            {
                Burn(card);
                break;
            }
        }
    }

    // Re draws a specific card
    public void RedrawCard()
    {
        // Raycast for card on interface layer
        PointerEventData m_PointerEventData = new PointerEventData(eventSystem);
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        eventRaycaster.Raycast(m_PointerEventData, results);

        // Debug the result
        foreach (RaycastResult result in results)
        {
            Card card = result.gameObject.transform.parent.GetComponent<Card>();
            if (card != null)
            {
                Redraw(card);
                break;
            }
        }
    }

    // Burns a card
    public void Burn(Card card)
    {
        if (card != null && !card.redrawing)
        {
            // Check to make sure enough cards are in the list
            if (dealList.Count == 0) burns.text = "No Cards Left!";
            else if (burnsLeft <= 0) burns.text = "0 Remaining";
            else
            {
                // Lower burn amount
                burnsLeft -= 1;

                // Remove card from picked list
                if (pickedList.Contains(card.GetCard()))
                    pickedList.Remove(card.GetCard());

                // Remove card from draw
                Gamemode.blacklistCards.Add(card.GetCard());
                card.RedrawCard(PickNewCard());
                burns.text = burnsLeft + " Remaining";
                return;
            }
        }
    }

    // Redraws a card
    public void Redraw(Card card)
    {
        if (card != null && !card.redrawing)
        {
            // Check to make sure enough cards are in the list
            if (dealList.Count == 0) redraws.text = "No Cards Left!";
            else if (redrawsLeft <= 0) redraws.text = "0 Remaining";
            else
            {
                redrawsLeft -= 1;
                card.RedrawCard(PickNewCard());
                redraws.text = redrawsLeft + " Remaining";
                return;
            }
        }
    }

    // Pick card
    public CardData PickNewCard()
    {
        // Take the card and remove it from the list
        CardData newCard = null;

        if (pickedList.Count > 0 && Random.Range(0, 1f) > 0.8f)
        {
            CardData card = pickedList[Random.Range(0, pickedList.Count)];
            Dictionary<CardData, int> deckCards = Deck.active.GetCards();
            if (!deckCards.ContainsKey(card) || deckCards[card] < card.maximumAmount)
            {
                newCard = card;
                if (dealList.Contains(newCard))
                    dealList.Remove(newCard);
                pickedList.Remove(newCard);
            }
        }
        if (newCard == null)
        {
            newCard = dealList[Random.Range(0, dealList.Count)];
            if (pickedList.Contains(newCard))
                pickedList.Remove(newCard);
            dealList.Remove(newCard);
        }
        return newCard;
    }

    // Open dealer
    public void OpenDealer()
    {
        // Close upgrades
        ToggleUpgrades(false);
        upgradeOptions.alpha = 0f;
        upgradeOptions.interactable = false;
        upgradeOptions.blocksRaycasts = false;

        // Set controller controls
        controlsOne.SetActive(Controller.isControllerConnected);
        controlsTwo.SetActive(Controller.isControllerConnected);
        controlsThree.SetActive(Controller.isControllerConnected);

        // Reset all cards
        foreach (Card card in cardSlots)
            card.canvasGroup.alpha = 0f;

        // Get copy of the scriptables list
        dealList = new List<CardData>(Scriptables.cards);
        pickedList = new List<CardData>(pickedCards);

        // Check cards and remove outdated ones
        Dictionary<CardData, int> deckCards = Deck.active.GetCards();
        for (int i = 0; i < dealList.Count; i++)
        {
            // Get card
            CardData card = dealList[i];

            // Remove card if not unlocked
            if (!card.canDrop || (!card.isUnlocked && !SaveSystem.IsCardUnlocked(card.InternalID)) ||
                (deckCards.ContainsKey(card) && deckCards[card] >= card.maximumAmount) ||
                Gamemode.blacklistCards.Contains(card))
            {
                dealList.Remove(card);
                i--;
            }

            // If card unlock, use chance
            else if (Random.Range(0f, 1f) > card.dropChance)
            {
                dealList.Remove(card);
                i--;
            }
        }

        // Deal the cards
        dealCards = true;
        cardsDealt = false;
        canvasSet = false;
        cardCooldown = cardDealSpeed;
        cardNumber = 0;

        // Set the canvas component
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        background.color = new Color(0, 0, 0, 0);
        title.alpha = 0;
        dealOptions.alpha = 0;

        // Get redraws from save file
        redrawsLeft = SaveSystem.GetRedraws();
        redraws.text = redrawsLeft + " Remaining";

        // Set open flag
        isOpen = true;
    }

    // Open dealer
    public void CloseDealer()
    {
        // Reset cards
        foreach(Card card in cardSlots)
            card.ResetCard();

        // Close upgrades
        ToggleUpgrades(false);

        // Set the canvas component
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Set open flag
        isOpen = false;
        cardsDealt = false;
    }
}
