using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour
{
    public enum Type
    {
        Healing,
        Radiation
    }
    public Type type;
    public TotemController totemController;
    public AudioClip sound;
    public Transform radius;
    public float targetRadius;
    public float scaleSpeed;

    // On start, play animation
    public void Update()
    {
        if (radius.localScale.x < targetRadius)
            radius.localScale = new Vector2(radius.localScale.x, radius.localScale.y) * scaleSpeed * Time.deltaTime;
    }

    // On collision with enemy, apply damage
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the enemy component
        Ship ship = collision.GetComponent<Ship>();
        if (ship != null)
        {
            AudioPlayer.Play(sound, false, 1f, 1f, false, 0.6f);
            totemController.isHealing = true;
        }
    }

    // On collision with enemy, apply damage
    public void OnTriggerExit2D(Collider2D collision)
    {
        // Get the enemy component
        Ship ship = collision.GetComponent<Ship>();
        if (ship != null)
        {
            AudioPlayer.Play(sound, false, 0.8f, 0.8f, false, 0.6f);
            totemController.isHealing = false;
        }
    }
}
