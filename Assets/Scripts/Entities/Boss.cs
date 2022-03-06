using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // Reference the main enemy script
    public Sprite bossModel;
    public Gunner gunner;
    protected Enemy enemy;

    // On start, get reference to enemy script
    public void Start()
    {
        // Get enemy and target reference
        enemy = GetComponent<Enemy>();

        // Setup gunner
        gunner.Setup(enemy.deathMaterial);

        // Set the boss bar
        Events.active.BossSpawned(this, enemy);

        // Play boss sound
        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = Settings.sound;
        audio.Play();
    }
}
