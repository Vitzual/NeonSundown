using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnHoverAdjustScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Hover variables
    public float scaleAdjustment = 0.1f;
    public bool overrideSize = false;
    public Vector3 originalSize;
    public bool playSound = true;
    private RectTransform rect;

    // On start grab rect transform
    public void Start() 
    {
        rect = GetComponent<RectTransform>();
        if (!overrideSize) originalSize = rect.localScale;
    }

    // On mouse enter make local scale bigger
    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.localScale = originalSize;
        rect.localScale = new Vector3(rect.localScale.x + scaleAdjustment,
                                      rect.localScale.y + scaleAdjustment,
                                      rect.localScale.z + scaleAdjustment);
        if (playSound)
            AudioPlayer.PlayButton();
    }

    // On mouse exit make local scale smaller
    public void OnPointerExit(PointerEventData eventData)
    {
        rect.localScale = originalSize;
    }
}
