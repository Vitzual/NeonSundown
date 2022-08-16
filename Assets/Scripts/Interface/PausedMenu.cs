using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedMenu : MonoBehaviour
{
    // Pause menu variables
    public CanvasGroup _stats;
    public static CanvasGroup stats;
    public static CanvasGroup canvasGroup;
    private static bool isOpen = false;

    // On start grab the canvas group
    public void Start()
    {
        stats = _stats;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    public void Update()
    {
        if (!isOpen)
        {
            if (Input.GetKeyDown(Keybinds.stats)) stats.alpha = 1f;
            else if (Input.GetKeyUp(Keybinds.stats)) stats.alpha = 0f;
        }

        if (Input.GetKeyDown(Keybinds.escape))
        {
            if (!Dealer.isOpen)
            {
                if (isOpen) Close();
                else Open();
            }
            else if (canvasGroup.alpha == 1f) Close();
        }
        else if (Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            if (!Dealer.isOpen)
            {
                if (isOpen) Close();
                else Open();
            }
            else if (canvasGroup.alpha == 1f) Close();
        }
    }

    // Open the pause menu
    public static void Open()
    {
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
        if (stats != null) stats.alpha = 0f;
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        isOpen = false;
        Dealer.isOpen = false;
    }
}
