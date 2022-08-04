using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTotem : Drone
{
    // Sound that plays when entering totems range
    public AudioClip totemSound;

    // Base totem variables
    public Transform radius;
    public float targetRadius;
    public Vector3 scaleSpeed;
    public float maxDistance;
    protected float cooldown;
    private AudioSource audioSource;

    // Within range flag
    protected bool shipInsideRange;

    // Startup override
    public override void Setup(Ship ship, HelperData data)
    {
        audioSource = GetComponent<AudioSource>();
        base.Setup(ship, data);
    }

    // On start, play animation
    public override void CustomUpdate()
    {
        if (Dealer.isOpen) return;

        if (radius.localScale.x < targetRadius)
            radius.localScale += scaleSpeed * Time.deltaTime;
    }

    // On collision with enemy, apply damage
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the enemy component
        PlaySound(totemSound, 1);
        shipInsideRange = true;
    }

    // On collision with enemy, apply damage
    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        // Get the enemy component
        PlaySound(totemSound, 0.8f);
        shipInsideRange = false;
    }

    // Play sound
    public void PlaySound(AudioClip clip, float pitch)
    {
        if (audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.pitch = pitch;
            audioSource.volume = Settings.sound / 2f;
            audioSource.Play();
        }
    }
}
