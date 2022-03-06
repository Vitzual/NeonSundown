using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    // Active instance
    public static BulletHandler active;

    // List of all active bullets
    public List<Bullet> bullets;

    // Start method
    public void Start() { active = this; }

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
    public void CreateBullet(Weapon parent, WeaponData weapon, Vector2 position, Quaternion rotation, 
        int amount, Material material, bool overrideAudioCooldown = false, Transform target = null)
    {
        // Loop depending on bullet amount
        for (int i = 0; i < amount; i++)
        {
            // Create the tile
            GameObject lastObj = Instantiate(weapon.bullet.gameObject, position, rotation);
            lastObj.name = weapon.bullet.gameObject.name;

            // Adjust for rotational offset
            Vector3 rotationOffset = new Vector3(lastObj.transform.eulerAngles.x, lastObj.transform.eulerAngles.y,
                (lastObj.transform.eulerAngles.z - 90f) + Random.Range(-weapon.bloom, weapon.bloom));
            lastObj.transform.eulerAngles = rotationOffset;

            // Attempt to set enemy variant
            Bullet bullet = lastObj.GetComponent<Bullet>();
            bullet.Setup(parent, weapon, material, target);

            // Add to enemies list
            bullets.Add(bullet);
        }

        // Check if bullet has a sound
        if (weapon.bulletSound != null)
            AudioPlayer.Play(weapon.bulletSound, true, weapon.minPitch, weapon.maxPitch, overrideAudioCooldown, weapon.audioScale);
    }
}
