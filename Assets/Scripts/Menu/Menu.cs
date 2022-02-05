using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Title leen element
    public CanvasGroup menuButtons;
    public CanvasGroup pressSpace;
    public CanvasGroup socialsLeft;
    public CanvasGroup socialsRight;
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
            LeanTween.scale(title, titleTargetSize, titleSizeSpeed).setEase(LeanTweenType.easeInExpo).setDelay(0.2f);
            LeanTween.moveLocal(title.gameObject, titleTargetPos, titleAnimSpeed).setEase(LeanTweenType.easeInExpo).setDelay(0.2f);
            LeanTween.alphaCanvas(pressSpace, 0f, titleAnimSpeed);
            LeanTween.alphaCanvas(menuButtons, 1f, titleAnimSpeed).setDelay(titleAnimSpeed);
            LeanTween.alphaCanvas(socialsLeft, 1f, titleAnimSpeed).setDelay(titleAnimSpeed);
            LeanTween.alphaCanvas(socialsRight, 1f, titleAnimSpeed).setDelay(titleAnimSpeed);
            menuButtons.interactable = true;
            menuButtons.blocksRaycasts = true;
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

    // Load the main scene
    public void LoadMain()
    {
        SceneManager.LoadScene("Main");
    }

    // Exit the application
    public void QuitGame()
    {
        Application.Quit();
    }
}
