using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // Reference the main enemy script
    public Sprite bossModel;
    public Gunner gunner;
    public AudioClip music;
    public bool overrideMusic = true;
    public bool playSpawnSound = true;
    protected Enemy enemy;

    // On start call setup
    public void Start() { Setup(); }

    // On start, get reference to enemy script
    public virtual void Setup()
    {
        // Get enemy and target reference
        enemy = GetComponent<Enemy>();

        // Setup gunner
        if (gunner != null)
            gunner.Setup(enemy.deathMaterial);
        
        // Set the boss bar
        Events.active.BossSpawned(this, enemy);

        // Play boss sound
        if (playSpawnSound)
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.volume = Settings.sound;
            audio.Play();
        }
    }
}
