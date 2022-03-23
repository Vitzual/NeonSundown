using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    // Active instance
    public static BulletHandler active;
    public static bool stickyBullets = false;
    public static bool energyBullets = false;
    public static float bulletSize = 1f;

    // List of all active bullets
    public List<Bullet> bullets;
    public Bullet energyBullet;
    public List<LayerMask> _laserLayers;
    private LayerMask laserLayers;

    // Start method
    public void Start() 
    {
        active = this; 
        stickyBullets = false;
        energyBullets = false;
        bulletSize = 1;

        foreach (LayerMask layer in _laserLayers)
            laserLayers = layer | laserLayers;
    }

    // Move normal enemies
    public void Update()
    {
        // Check if something is open
        if (Dealer.isOpen) return;

        // Move enemies each frame towards their target
        for (int a = 0; a < bullets.Count; a++)
        {
            if (bullets[a] != null)
            {
                bullets[a].Move();
            }
            else
            {
                bullets.RemoveAt(a);
                a--;
            }
        }
    }

    // Create a new active bullet instance
    public void CreateBullet(Weapon parent, WeaponData weapon, Vector2 position, Quaternion rotation, int amount, Material material, 
        bool overrideAudioCooldown = false, bool explosiveRound = false, bool useSize = false, Transform target = null)
    {
        // Loop depending on bullet amount
        for (int i = 0; i < amount; i++)
        {
            // Create the tile
            GameObject lastObj = Instantiate(weapon.bullet.gameObject, position, rotation);
            lastObj.name = weapon.bullet.gameObject.name;

            // Set size if bigger then 1
            if (bulletSize > 1 && useSize)
                lastObj.transform.localScale = new Vector2(bulletSize, bulletSize);

            // Adjust for rotational offset
            Vector3 rotationOffset = new Vector3(lastObj.transform.eulerAngles.x, lastObj.transform.eulerAngles.y,
                (lastObj.transform.eulerAngles.z - 90f) + Random.Range(-parent.bloom, parent.bloom));
            lastObj.transform.eulerAngles = rotationOffset;

            // Attempt to set enemy variant
            Bullet bullet = lastObj.GetComponent<Bullet>();
            bullet.Setup(parent, weapon, material, target, false, explosiveRound);

            // Add to enemies list
            bullets.Add(bullet);
        }

        // Check if bullet has a sound
        if (weapon.bulletSound != null)
            AudioPlayer.Play(weapon.bulletSound, true, weapon.minPitch, weapon.maxPitch, overrideAudioCooldown, weapon.audioScale);
    }

    // Create a new active bullet instance
    public void CreateEnergyBullet(Weapon parent, WeaponData weapon, Vector2 position, Quaternion rotation, int amount, Material material,
        bool overrideAudioCooldown = false, bool explosiveRound = false, bool useSize = false, Transform target = null)
    {
        // Loop depending on bullet amount
        for (int i = 0; i < amount; i++)
        {
            // Create the tile
            GameObject lastObj = Instantiate(energyBullet.gameObject, position, rotation);
            lastObj.name = weapon.bullet.gameObject.name;

            // Set size if bigger then 1
            if (bulletSize > 1 && useSize)
                lastObj.transform.localScale = new Vector2(bulletSize, bulletSize);

            // Adjust for rotational offset
            Vector3 rotationOffset = new Vector3(lastObj.transform.eulerAngles.x, lastObj.transform.eulerAngles.y,
                (lastObj.transform.eulerAngles.z - 90f) + Random.Range(-parent.bloom, parent.bloom));
            lastObj.transform.eulerAngles = rotationOffset;

            // Attempt to set enemy variant
            Bullet bullet = lastObj.GetComponent<Bullet>();
            bullet.Setup(parent, weapon, material, target, false, explosiveRound);

            // Add to enemies list
            bullets.Add(bullet);
        }

        // Check if bullet has a sound
        if (weapon.bulletSound != null)
            AudioPlayer.Play(weapon.bulletSound, true, weapon.minPitch, weapon.maxPitch, overrideAudioCooldown, weapon.audioScale);
    }

    // Creates a splitshot bullet instance
    public void CreateSplitshot(Weapon parent, WeaponData weapon, Vector2 position, Quaternion rotation,
    int amount, Material material, float rotationAmount, bool explosiveRound = false)
    {
        // Loop depending on bullet amount
        for (int i = 0; i < amount; i++)
        {
            // Create the tile
            GameObject lastObj = Instantiate(weapon.bullet.gameObject, position, rotation);
            lastObj.name = weapon.bullet.gameObject.name;

            // Adjust for rotational offset
            lastObj.transform.rotation = rotation;
            lastObj.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, rotationAmount));

            // Attempt to set enemy variant
            Bullet bullet = lastObj.GetComponent<Bullet>();
            bullet.Setup(parent, weapon, material, null, true, explosiveRound);

            // Add to enemies list
            bullets.Add(bullet);
        }
    }

    // Creates a laser bullet
    public void CreateLaserBullet(Weapon parent, Transform barrel, Quaternion rotation, Material material,
        AudioClip sound, AudioClip hitSound, float damage, float knockback, float duration, float length, int amount)
    {
        // Loop through stated amount times
        for (int i = 0; i < amount; i++)
        {
            // Offset rotation slightly
            Vector3 rotationOffset = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y,
                rotation.eulerAngles.z + Random.Range(-parent.bloom, parent.bloom));
            Quaternion newRotation = new Quaternion();
            newRotation.eulerAngles = rotationOffset;

            // Draw the line (cosmetic only)
            LineDrawer.active.DrawFromParent(parent.transform, newRotation,
                material, 1f, duration, length);

            // Raycast for enemies
            RaycastHit2D[] hits = Physics2D.RaycastAll(barrel.position, barrel.up, Mathf.Infinity, laserLayers);
            if (hits.Length > 0) AudioPlayer.Play(hitSound);

            // Loop through all hits and apply damage
            foreach (RaycastHit2D hit in hits)
            {
                Entity entity = hit.collider.GetComponent<Entity>();
                if (entity != null)
                {
                    // Set target to null and cast for entities
                    entity.Damage(damage * 2, knockback * 2);
                    ExplosiveHandler.CreateKnockback(entity.transform.position, 10f, -1000, -1500, 50);
                }
            }            
        }

        // Play the sound
        AudioPlayer.Play(sound, true, 0.9f, 1.1f, true, 0.7f);
    }
}
