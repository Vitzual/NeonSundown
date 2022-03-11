using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class Dealer : MonoBehaviour
{
    // Active instance
    public static Dealer active;
    public static bool isOpen;

    // List of card slots
    [BoxGroup("Card Options")]
    public List<Card> cardSlots;
    [BoxGroup("Card Options")]
    private List<CardData> dealList;
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
    public TextMeshProUGUI title;
    [BoxGroup("Interface Options")]
    public float titleFadeInSpeed = 0.01f;
    [BoxGroup("Interface Options")]
    public Image background;
    [BoxGroup("Interface Options")]
    public float bgFadeInSpeed = 0.01f;
    [BoxGroup("Interface Options"), Range(0, 1)]
    public float bgAlphaTarget = 0.5f;

    // Debug switch
    [BoxGroup("Debug Flags")]
    public bool debugSwitch = false;

    // Internal flags
    private bool dealCards = false;
    private bool cardsDealt = false;
    private bool canvasSet = false;
    private float cardCooldown = 0.5f;
    private int cardNumber = 0;

    // Private components 
    private Transform rotator;
    private CanvasGroup canvasGroup;

    // On start get private components 
    public void Start()
    {
        active = this;
        rotator = transform;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // While open, rotate
    public void Update()
    {
        // If debug switch set to true, deal cards
        if (debugSwitch)
        {
            OpenDealer();
            debugSwitch = false;
        }

        // If dealing, fade in background and deal cards
        if (dealCards)
        {
            if (background.color.a < 0.5f)
            {
                if (Settings.skipCardAnim) background.color = new Color(0, 0, 0, 0.5f);
                else if (Settings.fastCardAnim) background.color = new Color(0, 0, 0, background.color.a + 
                    (bgFadeInSpeed * Time.deltaTime * fastDealSpeed));
                else background.color = new Color(0, 0, 0, background.color.a + (bgFadeInSpeed * Time.deltaTime));
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
            if (Settings.musicPitching && music.pitch > pitchDown)
            {
                if (Settings.skipCardAnim) music.pitch = pitchDown;
                else if (Settings.fastCardAnim) music.pitch -= pitchSpeed * Time.deltaTime * fastDealSpeed;
                else music.pitch -= pitchSpeed * Time.deltaTime;
            }

            if (!canvasSet && cardsDealt)
            {
                if (title.alpha < 1f)
                {
                    if (Settings.skipCardAnim) title.alpha = 1f;
                    else if (Settings.fastCardAnim) title.alpha += titleFadeInSpeed * Time.deltaTime * fastDealSpeed;
                    else title.alpha += titleFadeInSpeed * Time.deltaTime;
                }
                else canvasSet = true;
            }
        }

        // Pitch back up music after dealign
        else if (music.pitch < 1.0f)
        {
            if (Settings.skipCardAnim) music.pitch = 1f;
            else if (Settings.fastCardAnim) music.pitch += pitchSpeed * Time.deltaTime * fastDealSpeed;
            else music.pitch += pitchSpeed * Time.deltaTime;

            if (music.pitch >= 1f)
                music.pitch = 1f;
        }
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
        CardData card = dealList[Random.Range(0, dealList.Count)];

        // Set the card
        cardSlots[number].Set(card);
        dealList.Remove(card);

        // Reset card cooldown
        if (!Settings.skipCardAnim)
        {
            if (Settings.fastCardAnim) cardCooldown = cardDealSpeed / 4f;
            else cardCooldown = cardDealSpeed;
        }

        // Check if that was the last card
        if (number + 1 == cardsToPick)
        {
            dealCards = false;
            cardsDealt = true;
        }
        else cardNumber += 1;
    }
    
    // Pick the card and add to palyer
    public void PickCard(CardData card, bool redraw)
    {
        if (!cardsDealt) return;
        Deck.active.AddCard(card);

        // Check if card is secondary
        if (redraw)
        {
            // Reset cards
            foreach (Card cardSlot in cardSlots)
                cardSlot.ResetCard();
            OpenDealer();
        }
        else CloseDealer();
    }

    // Open dealer
    public void OpenDealer()
    {
        // Reset all cards
        foreach (Card card in cardSlots)
            card.canvasGroup.alpha = 0f;

        // Get copy of the scriptables list
        dealList = new List<CardData>(Scriptables.cards);

        // Check cards and remove outdated ones
        Dictionary<CardData, int> deckCards = Deck.active.GetCards();
        for (int i = 0; i < dealList.Count; i++)
        {
            // Get card
            CardData card = dealList[i];

            // Remove card if not unlocked
            if (!card.canDrop || Gamemode.arena.blacklistCards.Contains(card) ||
                (!card.isUnlocked && !SaveSystem.IsCardUnlocked(card.InternalID)) ||
                (deckCards.ContainsKey(card) && deckCards[card] >= card.maximumAmount))
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

        // Set open flag
        isOpen = true;
    }

    // Open dealer
    public void CloseDealer()
    {
        // Reset cards
        foreach(Card card in cardSlots)
            card.ResetCard();

        // Set the canvas component
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Set open flag
        isOpen = false;
    }
}
