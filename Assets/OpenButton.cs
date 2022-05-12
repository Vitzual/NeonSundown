using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenButton : MonoBehaviour
{
    public CanvasGroup canvasToOpen;
    public CanvasGroup canvasToClose;

    public void Start()
    {
        if (canvasToClose == null)
            canvasToClose = GetComponent<CanvasGroup>();
    }

    public void Open()
    {
        canvasToOpen.alpha = 1.0f;
        canvasToOpen.interactable = true;
        canvasToOpen.blocksRaycasts = true;

        canvasToClose.alpha = 0f;
        canvasToClose.interactable = false;
        canvasToClose.blocksRaycasts = false;
    }
}
