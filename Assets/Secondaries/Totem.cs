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
    private float cooldown;
    private bool isHealing;

    // On start, play animation
    public void Update()
    {
        if (radius.localScale.x < targetRadius)
            radius.localScale = new Vector2(radius.localScale.x, radius.localScale.y) * scaleSpeed * Time.deltaTime;

        if (cooldown <= 0)
        {
            if (isHealing)
            {
                Ship.Heal(totemController.healAmount);
                cooldown = totemController.healCooldown;
            }
        }
        else cooldown -= Time.deltaTime;
    }

    // On collision with enemy, apply damage
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the enemy component
        Ship ship = collision.GetComponent<Ship>();
        if (ship != null)
        {
            AudioPlayer.Play(sound, false, 1f, 1f, false, 0.6f);
            isHealing = true;
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
            isHealing = false;
        }
    }
}
