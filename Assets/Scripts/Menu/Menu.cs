using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // Title leen element
    public RectTransform title;
    public Vector2 titleTargetPos = new Vector2(0, 200);
    public float titleAnimSpeed;
    public Vector2 titleTargetSize = new Vector2(0.7f, 0.7f);
    public float titleSizeSpeed;

    // Press space text element
    public TextMeshProUGUI pressSpaceText;
    public bool increaseAlpha = false;
    public float alphaAdjustSpeed = 0.1f;

    // Menu flags
    private bool spacePressed = false;

    // Start is called before the first frame update
    public void Awake()
    {
        Scriptables.GenerateAllScriptables();
    }

    // Update user input
    public void Update()
    {
        if (!spacePressed && Input.anyKey)
        {
            
            spacePressed = true;
        }
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
