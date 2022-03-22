using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWall : MonoBehaviour
{
    public SpriteRenderer border;
    public SpriteRenderer fill;

    // On start set the thing
    public void Start()
    {
        border.material = Gamemode.arena.wallBorder;
        fill.color = Gamemode.arena.wallFill;
        border.transform.position = border.transform.position * Gamemode.arena.wallAdjustment;
        fill.transform.position = fill.transform.position * Gamemode.arena.wallAdjustment;
    }

    // Kill anything that moves :)
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Ship ship = collision.GetComponent<Ship>();
        if (ship != null) ship.Kill();
    }
}
