using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // Press space text element
    public TextMeshProUGUI pressSpaceText;
    public bool increaseAlpha = false;
    public float alphaAdjustSpeed = 0.1f;

    // Start is called before the first frame update
    public void Awake()
    {
        Scriptables.GenerateAllScriptables();
    }

    // Update menu
    public void FixedUpdate()
    {
        if (increaseAlpha)
        {
            pressSpaceText.alpha += alphaAdjustSpeed;
            if (pressSpaceText.alpha >= 1f) increaseAlpha = false;
        }
        else
        {
            pressSpaceText.alpha -= alphaAdjustSpeed;
            if (pressSpaceText.alpha <= 0f) increaseAlpha = true;
        }
    }
}
