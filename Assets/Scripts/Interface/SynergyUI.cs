using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SynergyUI : MonoBehaviour
{
    // Get active instance
    public static SynergyUI active;

    // Debug synergy
    public static CanvasGroup screen;
    public Card cardOne, cardTwo, synergyCard;
    public bool debugFlag;
    public SynergyData debugTest;
    public float fadeInSpeed = 1f;
    public float moveSpeedDelay = 2f;
    public Vector3 movePosition;
    private float cooldown = 0f;
    private bool synergizing = false;
    private static bool listenForController = false;

    // On awake get active instance
    public void Awake() { active = this; }
    public void Start() { screen = GetComponent<CanvasGroup>(); }

    // Synergize them cards
    public void Synergize(SynergyData synergy)
    {
        // Add the thing boss man
        RuntimeStats.synergiesCreated += 1;

        debugFlag = false;
        synergizing = true;
        LeanTween.alphaCanvas(screen, 1f, 1f);
        cardOne.Synergize(synergy.cardOne, fadeInSpeed, moveSpeedDelay);
        cardTwo.Synergize(synergy.cardTwo, fadeInSpeed, moveSpeedDelay);
        synergyCard.Set(synergy.outputCard, false, true, synergy.tier);
        synergyCard.SetSynergy(synergy);
        synergyCard.canvasGroup.alpha = 0f;
        LeanTween.alphaCanvas(synergyCard.canvasGroup, 1f, 0.25f).setDelay(fadeInSpeed + moveSpeedDelay);
        synergyCard.GetComponent<RectTransform>().localScale = new Vector3(10f, 10f, 1f);
        LeanTween.scale(synergyCard.gameObject, new Vector3(11f, 11f, 1f), 0.5f)
            .setEase(LeanTweenType.easeInExpo).setDelay(fadeInSpeed + moveSpeedDelay);
        cooldown = fadeInSpeed + moveSpeedDelay;
        Dealer.isOpen = true;
    }

    // For debugging
    public void Update()
    {
        if (debugFlag) Synergize(debugTest);

        if (listenForController && Input.GetKeyDown(KeyCode.JoystickButton0)) 
        {
            synergyCard.OnClick(true); 
        }

        if (synergizing)
        {
            if (cooldown > 0f)
                cooldown -= Time.deltaTime;
            else
            {
                listenForController = true;
                screen.interactable = true;
                screen.blocksRaycasts = true;
                synergizing = false;
                AudioSource source = GetComponent<AudioSource>();
                source.volume = Settings.sound;
                source.Play();
            }
        }
    }

    // Disable
    public static void Close()
    {
        screen.alpha = 0f;
        screen.interactable = false;
        screen.blocksRaycasts = false;
        listenForController = false;
    }
}
