using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : Bullet
{
    private Ship ship;
    private AudioClip convertSound;

    public override void Setup(Weapon parent, WeaponData weapon, Material material, 
        Transform target = null, bool isSplitShot = false, bool explosiveRound = false, bool autoLock = false)
    {
        ship = parent.GetComponent<Ship>();
        convertSound = ship.shipData.seedBoomSound;
        base.Setup(parent, weapon, material, target, isSplitShot, explosiveRound, autoLock);
    }

    // On collision
    public override void OnHit(Entity entity)
    {
        // Check if entity is dead
        if (entity.IsDead()) return;

        // Check if enemy
        Enemy enemy = entity.GetComponent<Enemy>();
        if (enemy != null && !enemy.IsSeeded())
        {
            enemy.gameObject.AddComponent<EnemyOnEnemyCollision>().Setup(enemy);
            foreach (SpriteRenderer sprite in enemy.glows) sprite.material = ship.border.material;
            foreach (SpriteRenderer sprite in enemy.fills) sprite.color = ship.fill.color;
            enemy.SeedEnemy(ship.transform);
            ship.AddSeededEnemy(enemy);
        }

        // Play convert sound
        if (convertSound != null) 
            AudioPlayer.Play(convertSound);

        // Destroy
        Destroy();
    }
}
