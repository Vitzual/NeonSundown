using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    // Canvas group
    public CanvasGroup gameOverScreen;

    // Start is called before the first frame update
    public void Start()
    {
        Events.active.onShipDestroyed += ShowScreen;
    }

    // Update is called once per frame
    private void ShowScreen()
    {
        Dealer.active.pitchDown = 0f;
        Dealer.isOpen = true;
        LeanTween.alphaCanvas(gameOverScreen, 1f, 1f);
        gameOverScreen.interactable = true;
        gameOverScreen.blocksRaycasts = true;
    }
}
