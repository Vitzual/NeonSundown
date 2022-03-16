using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedMenu : MonoBehaviour
{
    // Pause menu variables
    private static CanvasGroup canvasGroup;
    private static bool isOpen = false;

    // On start grab the canvas group
    public void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    public void Update()
    {
        if (Input.GetKeyDown(Keybinds.escape))
        {
            if (isOpen) Close();
            else Open();
        }
    }

    // Open the pause menu
    public static void Open()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        CardEffects.UpdateEffects();

        isOpen = true;
        Dealer.isOpen = true;
    }

    // Close the pause menu
    public static void Close()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        isOpen = false;
        Dealer.isOpen = false;
    }
}
