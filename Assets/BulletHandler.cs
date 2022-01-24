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

    // Handles enemy movement every frame
    public void Update() { MoveBullets(); }

    // Move normal enemies
    public virtual void MoveBullets()
    {
        // Move enemies each frame towards their target
        for (int a = 0; a < bullets.Count; a++)
        {
            if (bullets[a] != null)
            {
                if (bullets[a].target != null)
                {
                    bullets[a].Move();
                }
            }
            else
            {
                bullets.RemoveAt(a);
                a--;
            }
        }
    }
    // Create a new active enemy instance
    public void CreateBullet(WeaponData weapon, Vector2 position, Quaternion rotation)
    {
        // Create the tile
        GameObject lastObj = Instantiate(weapon.bullet.gameObject, position, rotation);
        lastObj.name = weapon.bullet.gameObject.name;

        // Attempt to set enemy variant
        Bullet bullet = lastObj.GetComponent<Bullet>();
        bullet.Setup(weapon.damage * Deck.GetMultiplier(Stat.Damage),
                     weapon.speed * Deck.GetMultiplier(Stat.Speed),
                     weapon.pierces * Deck.GetMultiplier(Stat.Pierces),
                     weapon.tracking || Deck.GetFlag(Stat.Tracking));

        // Add to enemies list
        bullets.Add(bullet);
    }

}
