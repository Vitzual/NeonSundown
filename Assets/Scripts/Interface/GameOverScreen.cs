using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    // Canvas group
    public CanvasGroup mainScreen;
    public GameObject gameOverTitle;
    public CanvasGroup gameOverScreen;
    public CanvasGroup runtimeStats;
    public CanvasGroup button;
    public AudioClip gameOverSound;
    public TextMeshProUGUI stats;
    public static bool isActive = false;
    
    // Start is called before the first frame update
    public void Start()
    {
        Events.active.onShipDestroyed += ShowScreen;
        isActive = false;
    }
    
    // Update is called once per frame
    public void ShowScreen()
    {
        // Log ending game
        isActive = true;
        Debug.Log("Ending game!");

        // Set the runtime stats
        stats.text = Formatter.Time(EnemySpawner.GetTime()) + "<br>" +
            Formatter.Round(RuntimeStats.totalXP, 0) + "<br>" +
            RuntimeStats.enemiesDestroyed + "<br>" +
            RuntimeStats.bulletsFired + "<br>" +
            Formatter.Round(RuntimeStats.damageGiven, 0) + "<br>" +
            Formatter.Round(RuntimeStats.damageTaken, 0) + "<br>" +
            RuntimeStats.cardsChosen + "<br>" +
            RuntimeStats.synergiesCreated;

        // Setup animations
        LeanTween.alphaCanvas(gameOverScreen, 1f, 1f);
        LeanTween.moveLocal(gameOverTitle, new Vector2(0, 90), 0.5f)
            .setEase(LeanTweenType.easeInExpo).setDelay(1f);
        LeanTween.alphaCanvas(runtimeStats, 1f, 0.5f).setDelay(1.25f);
        LeanTween.alphaCanvas(button, 1f, 0.5f).setDelay(1.25f);

        // Set screen to interactable
        mainScreen.alpha = 1f;
        mainScreen.interactable = true;
        mainScreen.blocksRaycasts = true;
        AudioPlayer.Play(gameOverSound, false);

        // Stop music and pause game
        MusicPlayer.StopMusic();
        Dealer.isOpen = true;
    }
}
