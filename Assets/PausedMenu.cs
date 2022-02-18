using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedMenu : MonoBehaviour
{
    // Pause menu variables
    private static CanvasGroup canvasGroup;
    private bool isOpen = false;

    // On start grab the canvas group
    public void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    public void Update()
    {
        if (Input.GetKeyDown(Keybinds.escape) && !Dealer.isOpen)
        {
            if (isOpen)
            {
                Close();
                isOpen = false;
                Dealer.isOpen = false;
            }
            else
            {
                Open();
                isOpen = true;
                Dealer.isOpen = true;
            }
        }
    }

    // Open the pause menu
    public static void Open()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    // Close the pause menu
    public static void Close()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
