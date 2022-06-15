using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Background : MonoBehaviour
{
    // Tilemap instance
    private Tilemap tilemap;

    // Start is called before the first frame update
    public void Start()
    {
        tilemap = GetComponent<Tilemap>();
        Events.active.onLightChanged += SetLighting;
        SetLighting(Settings.lightAmount);
    }

    // Update is called once per frame
    public void SetLighting(float amount)
    {
        tilemap.color = new Color(amount, amount, amount, 1f);
    }
}
