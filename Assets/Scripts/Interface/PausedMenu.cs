using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedMenu : MonoBehaviour
{
    // Pause menu variables
    public CanvasGroup _stats;
    public GameOverScreen endScreen;
    public static CanvasGroup stats;
    public static CanvasGroup canvasGroup;
    private static bool isOpen = false;
    
    // On start grab the canvas group
    public void Start()
    {
        stats = _stats;
        canvasGroup = GetComponent<CanvasGroup>();

        InputEvents.Instance.onEscapePressed.AddListener(Toggle);
        InputEvents.Instance.onRightButton.AddListener(QuitGame);
    }

    // Start is called before the first frame update
    public void Update()
    {
        if (!isOpen)
        {
            if (CIN._action_stats.IsPressed()) stats.alpha = 1f;
            else if (!CIN._action_stats.IsPressed()) stats.alpha = 0f;
        }
    }
    
    /// <summary>
    /// Togges the dealer menu
    /// </summary>
    public static void Toggle()
    {
        if (!Dealer.isOpen)
        {
            if (isOpen) Close();
            else Open();
        }
        else if (canvasGroup.alpha == 1f) Close();
    }

    protected void QuitGame()
    {
        Close();
        endScreen.ShowScreen();
    }

    // Open the pause menu
    public static void Open()
    {
        // Call pause event
        if (Events.active != null)
            Events.active.PauseGame(true);

        if (stats != null) stats.alpha = 1f;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        isOpen = true;
        Dealer.isOpen = true;
    }

    // Close the pause menu
    public static void Close()
    {
        // Call pause event
        if (Events.active != null)
            Events.active.PauseGame(false);

        // Reset cursor lock state
        // Cursor.lockState = CursorLockMode.Confined;

        if (stats != null) stats.alpha = 0f;
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        isOpen = false;
        Dealer.isOpen = false;
    }
}
