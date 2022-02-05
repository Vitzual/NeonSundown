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
    public AudioClip cardSound;

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
                background.color = new Color(0, 0, 0, background.color.a + bgFadeInSpeed);
            else
            {
                if (cardCooldown <= 0f) DealCard(cardNumber);
                else cardCooldown -= Time.deltaTime;
            }
        }

        // Pitch down music
        if (isOpen)
        {
            if (music.pitch > pitchDown)
                music.pitch -= pitchSpeed;

            if (!canvasSet && cardsDealt)
            {
                if (title.alpha < 1f)
                    title.alpha += titleFadeInSpeed;
                else canvasSet = true;
            }
        }

        // Pitch back up music after dealign
        else if (music.pitch < 1.0f)
        {
            music.pitch += pitchSpeed;
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
        if (card.canDrop)
        {
            cardSlots[number].Set(card);
            dealList.Remove(card);

            // Play card sound
            AudioPlayer.Play(cardSound);

            // Reset card cooldown
            cardCooldown = cardDealSpeed;

            // Check if that was the last card
            if (number + 1 == cardsToPick)
            {
                dealCards = false;
                cardsDealt = true;
            }
            else cardNumber += 1;
        }
        else
        {
            Debug.Log(card.name + " cant drop!");
            dealList.Remove(card);
        }
    }

    // Pick the card and add to palyer
    public void PickCard(CardData card, int number)
    {
        CloseDealer();
        Deck.active.AddCard(card);
    }

    // Open dealer
    public void OpenDealer()
    {
        // Get copy of the scriptables list
        dealList = new List<CardData>(Scriptables.cards);

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
