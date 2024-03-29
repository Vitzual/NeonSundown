using System;
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

    [Header("Limited time variables")]
    public GameObject limitedTimeObject;
    public Image bannerLeft, bannerRight;
    public TextMeshProUGUI title;
    public TextMeshProUGUI countdown;

    // Button vairables
    [Header("Button customization")]
    public Color lockedBackgroundColor;
    public Color lockedBorderColor;
    public Color lockedObjectiveColor;
    
    // Generates at runtime
    public void Set(ArenaData arena, string bestRun, bool showTime = false)
    {
        // Check if unlocked
        if (SaveSystem.IsArenaUnlocked(arena.InternalID))
        {
            // Set hover adjust
            OnHoverAdjustScale onHover = GetComponent<OnHoverAdjustScale>();
            if (onHover != null) onHover.enabled = true;
            isLocked = false;
        }

        // Set arena info
        this.arena = arena;
        name.text = arena.name.ToUpper();
        desc.text = bestRun.ToUpper();

        // Set button colors
        desc.color = arena.lightColor;
        icon.sprite = arena.unlockedIcon;
        iconBorder.color = arena.buttonColor;
        buttonBackground.color = arena.darkColor;
        buttonBorder.color = arena.buttonColor;

        // Reset the rect transform
        RectTransform rect = GetComponent<RectTransform>();
        rect.localScale = new Vector3(1, 1, 1);

        // Check if limited time arena
        if (arena.limitedTimeArena && showTime)
        {
            limitedTimeObject.SetActive(true);
            bannerLeft.color = arena.buttonColor;
            title.color = arena.darkColor;
            bannerRight.color = arena.buttonColor;
            countdown.color = arena.darkColor;

            UpdateTimer();
        }
        else limitedTimeObject.SetActive(false);
    }

    public void UpdateTimer()
    {
        DateTime endTime = new DateTime(
            arena.limitedTimeYear,
            arena.limitedTimeMonth,
            arena.limitedTimeDay);
        DateTime nowTime = DateTime.Now;
        TimeSpan time = endTime - nowTime;

        string days = time.Days.ToString();
        if (days.Length == 1) days = "0" + days;

        string hours = time.Hours.ToString();
        if (hours.Length == 1) hours = "0" + hours;

        string minutes = time.Minutes.ToString();
        if (minutes.Length == 1) minutes = "0" + minutes;

        string seconds = time.Seconds.ToString();
        if (seconds.Length == 1) seconds = "0" + seconds;

        countdown.text = string.Format("{0}:{1}:{2}:{3}", days, hours, minutes, seconds);

        if (time.TotalSeconds <= 0f) gameObject.SetActive(false);
    }

    // Pass arena info to panel
    public void OnClick()
    {
        if (!isLocked && Events.active != null && arena != null)
            Events.active.ArenaButtonClicked(arena);
    }

    // Locks the button
    public void Lock(ArenaData arena, bool showTime = false)
    {
        // Set is locked flag
        isLocked = true;

        // Set hover adjust
        OnHoverAdjustScale onHover = GetComponent<OnHoverAdjustScale>();
        if (onHover != null) onHover.enabled = false;

        // Set arena info
        this.arena = arena;
        name.text = "LOCKED";

        // Set button colors
        desc.color = lockedObjectiveColor;
        icon.sprite = arena.lockedIcon;
        iconBorder.color = lockedBorderColor;
        buttonBackground.color = lockedBackgroundColor;
        buttonBorder.color = lockedBorderColor;

        // Reset the rect transform
        RectTransform rect = GetComponent<RectTransform>();
        rect.localScale = new Vector3(1, 1, 1);

        // Check if limited time arena
        if (arena.limitedTimeArena && showTime)
        {
            desc.text = arena.limitedTimeUnlockObjective.ToUpper();

            limitedTimeObject.SetActive(true);
            bannerLeft.color = lockedBorderColor;
            title.color = lockedBackgroundColor;
            bannerRight.color = lockedBorderColor;
            countdown.color = lockedBackgroundColor;

            UpdateTimer();
        }
        else
        {
            desc.text = arena.unlockObjective.ToUpper();
            limitedTimeObject.SetActive(false);
        }
    }
}
