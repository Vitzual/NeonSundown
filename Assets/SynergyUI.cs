using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SynergyUI : MonoBehaviour
{
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

    // On start get screen
    public void Start()
    {
        screen = GetComponent<CanvasGroup>();
    }

    // Synergize them cards
    public void Synergize(SynergyData synergy)
    {
        debugFlag = false;
        synergizing = true;
        LeanTween.alphaCanvas(screen, 1f, 1f);
        cardOne.Synergize(synergy.cardOne, fadeInSpeed, moveSpeedDelay);
        cardTwo.Synergize(synergy.cardTwo, fadeInSpeed, moveSpeedDelay);
        synergyCard.Set(synergy.outputCard, false, true);
        synergyCard.SetSynergy(synergy);
        synergyCard.canvasGroup.alpha = 0f;
        LeanTween.alphaCanvas(synergyCard.canvasGroup, 1f, 0.25f).setDelay(fadeInSpeed + moveSpeedDelay + 1f);
        synergyCard.GetComponent<RectTransform>().localScale = new Vector3(12f, 12f, 12f);
        LeanTween.scale(synergyCard.gameObject, new Vector3(14f, 14f, 1f), 0.5f)
            .setEase(LeanTweenType.easeInExpo).setDelay(fadeInSpeed + moveSpeedDelay + 1f);
        cooldown = fadeInSpeed + moveSpeedDelay + 1f;
        Dealer.isOpen = true;
    }

    // For debugging
    public void Update()
    {
        if (debugFlag) Synergize(debugTest);

        if (synergizing)
        {
            if (cooldown > 0f)
                cooldown -= Time.deltaTime;
            else
            {
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
    }
}
