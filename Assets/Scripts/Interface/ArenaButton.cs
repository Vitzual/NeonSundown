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
    public new TextMeshProUGUI name;
    public TextMeshProUGUI bestRun;
    public TextMeshProUGUI unlockReq;
    public Image buttonImage;
    public GameObject locked;
    private bool isLocked = false;

    // Generates at runtime
    public void Set(ArenaData arena, string bestRun)
    {
        // Set arena info
        this.arena = arena;
        name.text = arena.name.ToUpper();
        this.bestRun.text = bestRun;
        buttonImage.color = arena.buttonColor;

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

        // Set button state to locked
        this.arena = arena;
        buttonImage.enabled = false;
        unlockReq.text = arena.unlockObjective;
        locked.SetActive(true);

        // Reset the rect transform
        RectTransform rect = GetComponent<RectTransform>();
        rect.localScale = new Vector3(1, 1, 1);
    }
}
