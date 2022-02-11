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
    public Image buttonImage;

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
        if (Events.active != null && arena != null)
            Events.active.ArenaButtonClicked(arena);
    }
}
