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
    public void CreateLaserBullet(Weapon parent, WeaponData weapon, Quaternion rotation, Material material,
        float damage, float knockback, float duration, float length, int amount, bool useSize = false)
    {
        // Draw the line (cosmetic only)
        if (bulletSize > 1 && useSize) LineDrawer.active.DrawFromParent(parent.transform, 
            rotation, material, bulletSize, duration, length);
        else LineDrawer.active.DrawFromParent(parent.transform, rotation,
            material, 1f, duration, length);

        // Raycast for enemies
        RaycastHit2D[] hits = Physics2D.RaycastAll(parent.transform.position, 
            transform.right, Mathf.Infinity, laserLayers);

        // Loop through all hits and apply damage
        foreach(RaycastHit2D hit in hits)
        {
            Entity entity = hit.collider.GetComponent<Entity>();
            if (entity != null) entity.Damage(damage, knockback);
        }
    }
}
