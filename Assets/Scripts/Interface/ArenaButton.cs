using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArenaButton : MonoBehaviour
{
    // Arena data
    [HideInInspector]
    public ArenaData arena;

    [Header("Button variables")]
    public new TextMeshProUGUI name;
    public TextMeshProUGUI desc;
    public Image icon;
    public Image iconBorder;
    public Image buttonBackground;
    public Image buttonBorder;
    private bool isLocked = false;

    // Button vairables
    [Header("Button customization")]
    public Color lockedBackgroundColor;
    public Color lockedBorderColor;
    public Color lockedObjectiveColor;
    
    // Generates at runtime
    public void Set(ArenaData arena, string bestRun)
    {
        // Set arena info
        this.arena = arena;
        name.text = arena.name.ToUpper();
        desc.text = bestRun;

        // Set button colors
        desc.color = arena.lightColor;
        icon.sprite = arena.unlockedIcon;
        iconBorder.color = arena.buttonColor;
        buttonBackground.color = new Color(arena.buttonColor.r, 
            arena.buttonColor.g, arena.buttonColor.b, 0.2f);
        buttonBorder.color = arena.buttonColor;

        // Reset the rect transform
        RectTransform rect = GetComponent<RectTransform>();
        rect.localScale = new Vector3(1, 1, 1);
    }

    // Pass arena info to panel
    public void OnClick()
    {
        if (!isLocked && Events.active != null && arena != null)
            Events.active.ArenaButtonClicked(arena);
    }

    // Locks the button
    public void Lock(ArenaData arena)
    {
        // Set is locked flag
        isLocked = true;

        // Set hover adjust
        OnHoverAdjustScale onHover = GetComponent<OnHoverAdjustScale>();
        if (onHover != null) onHover.enabled = false;

        // Set arena info
        this.arena = arena;
        name.text = arena.name.ToUpper();
        desc.text = arena.unlockObjective;

        // Set button colors
        desc.color = lockedObjectiveColor;
        icon.sprite = arena.lockedIcon;
        iconBorder.color = lockedBorderColor;
        buttonBackground.color = lockedBackgroundColor;
        buttonBorder.color = lockedBorderColor;

        // Reset the rect transform
        RectTransform rect = GetComponent<RectTransform>();
        rect.localScale = new Vector3(1, 1, 1);
    }
}
