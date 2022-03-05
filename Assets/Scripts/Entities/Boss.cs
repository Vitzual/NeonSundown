using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // Reference the main enemy script
    public List<Turret> turrets;
    public WeaponData weapon;
    public Sprite bossModel;
    public AudioClip spawnSound;
    protected Enemy enemy;
    protected Transform target;

    // On start, get reference to enemy script
    public void Start()
    {
        // Get enemy and target reference
        enemy = GetComponent<Enemy>();
        if (EnemyHandler.active.player != null) 
            target = EnemyHandler.active.player;

        // Setup turrets
        foreach(Turret turret in turrets)
            turret.Setup(weapon, Random.Range(1f, 2f), target);

        // Set the boss bar
        Events.active.BossSpawned(this, enemy);

        // Play spawn sound
        if (!MusicPlayer.isMenu)
            AudioPlayer.Play(spawnSound, false);
    }

    // Control turrets
    public void Update()
    {
        // Check if paused
        if (Dealer.isOpen) return;

        // Keep turrets up to date
        foreach (Turret turret in turrets)
            turret.Use();
    }
}
