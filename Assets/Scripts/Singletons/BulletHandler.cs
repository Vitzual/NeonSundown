using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    // Active instance
    public static BulletHandler active;

    // List of all active bullets
    public List<Bullet> bullets;
    public Bullet energyBullet;
    public Laser laserBullet;
    public Beam beamBullet;
    public AudioClip laserSound;

    // Runtime variables
    private List<Beam> beams;

    // Start method
    public void Start() 
    {
        active = this;
        beams = new List<Beam>();
    }

    // Move normal enemies
    public void Update()
    {
        // Check if something is open
        if (Dealer.isOpen) return;

        // Update timer
        if (beams.Count > 0)
        {
            for (int i = 0; i < beams.Count; i++)
            {
                if (beams[i] != null) beams[i].Move();
                else
                {
                    beams.RemoveAt(i);
                    i--;
                }
            }
        }

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
        int amount, float bloom, float bulletSize, Material material, bool overrideAudioCooldown = false, 
        bool explosiveRound = false, bool autoLockRound = false, Transform target = null)
    {
        // Loop depending on bullet amount
        for (int i = 0; i < amount; i++)
        {
            // Create the tile
            GameObject lastObj = Instantiate(weapon.bullet.gameObject, position, rotation);
            lastObj.name = weapon.bullet.gameObject.name;

            // Set size if bigger then 1
            lastObj.transform.localScale = new Vector2(bulletSize, bulletSize);

            // Adjust for rotational offset
            Vector3 rotationOffset = new Vector3(lastObj.transform.eulerAngles.x, lastObj.transform.eulerAngles.y,
                (lastObj.transform.eulerAngles.z - 90f) + Random.Range(-bloom, bloom));
            lastObj.transform.eulerAngles = rotationOffset;

            // Attempt to set enemy variant
            Bullet bullet = lastObj.GetComponent<Bullet>();
            bullet.Setup(parent, weapon, material, target, false, explosiveRound, autoLockRound);

            // Add to enemies list
            bullets.Add(bullet);
        }

        // Check if bullet has a sound
        if (weapon.bulletSound != null)
            AudioPlayer.Play(weapon.bulletSound, true, weapon.minPitch, weapon.maxPitch, overrideAudioCooldown, weapon.audioScale);
    }

    // Create a new active bullet instance
    public void CreateEnergyBullet(Weapon parent, WeaponData weapon, Vector2 position, Quaternion rotation, 
        int amount, float bloom, float bulletSize, Material material, bool overrideAudioCooldown = false, 
        bool explosiveRound = false, bool autoLockRound = false, Transform target = null)
    {
        // Loop depending on bullet amount
        for (int i = 0; i < amount; i++)
        {
            // Create the tile
            GameObject lastObj = Instantiate(energyBullet.gameObject, position, rotation);
            lastObj.name = weapon.bullet.gameObject.name;

            // Set size if bigger then 1
            lastObj.transform.localScale = new Vector2(bulletSize, bulletSize);

            // Adjust for rotational offset
            Vector3 rotationOffset = new Vector3(lastObj.transform.eulerAngles.x, lastObj.transform.eulerAngles.y,
                (lastObj.transform.eulerAngles.z - 90f) + Random.Range(-bloom, bloom));
            lastObj.transform.eulerAngles = rotationOffset;

            // Attempt to set enemy variant
            Bullet bullet = lastObj.GetComponent<Bullet>();
            bullet.Setup(parent, weapon, material, target, false, explosiveRound, autoLockRound);

            // Add to enemies list
            bullets.Add(bullet);
        }

        // Check if bullet has a sound
        if (weapon.bulletSound != null)
            AudioPlayer.Play(weapon.bulletSound, true, weapon.minPitch, weapon.maxPitch, overrideAudioCooldown, weapon.audioScale);
    }

    // Creates a splitshot bullet instance
    public void CreateSplitshot(Weapon parent, WeaponData weapon, Vector2 position, Quaternion rotation, float bulletSize,
        int amount, Material material, float rotationAmount, bool explosiveRound = false, bool autoLockRound = false)
    {
        // Loop depending on bullet amount
        for (int i = 0; i < amount; i++)
        {
            // Create the tile
            GameObject lastObj = Instantiate(weapon.bullet.gameObject, position, rotation);
            lastObj.name = weapon.bullet.gameObject.name;

            // Set size if bigger then 1
            lastObj.transform.localScale = new Vector2(bulletSize, bulletSize);

            // Adjust for rotational offset
            lastObj.transform.rotation = rotation;
            lastObj.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, rotationAmount));
            
            // Attempt to set enemy variant
            Bullet bullet = lastObj.GetComponent<Bullet>();
            bullet.Setup(parent, weapon, material, null, true, explosiveRound, autoLockRound);

            // Add to enemies list
            bullets.Add(bullet);
        }
    }

    // Creates a laser bullet
    public void CreateLaserBullet(Weapon parent, WeaponData weapon, Material material, Transform barrel, float bulletSize,
        float length, float pierces, int amount, bool explosive, bool isBeam = false)
    {
        // Check if already firing
        if (isBeam && beams.Count > 0) return;

        // Calculate spread
        float spread = 0;

        // Check if amount greater than 1
        if (amount > 1) spread -= (2.5f * amount) - 2.5f;

        // Create laser shots
        for (int i = 0; i < amount; i++)
        {
            // Set beam sound effect
            if (isBeam)
            {
                Beam newBeam = Instantiate(beamBullet, Vector3.zero, Quaternion.identity);
                newBeam.SetupBeam(barrel, bulletSize, length, spread, pierces, weapon.bulletSound, material, i == 0);
                newBeam.Setup(parent, weapon, material, null, false, explosive);
                beams.Add(newBeam);
            }

            // Create line
            else
            {
                Laser newLaser = Instantiate(laserBullet, Vector3.zero, Quaternion.identity);
                newLaser.SetupLaser(barrel, bulletSize, length, spread);
                newLaser.Setup(parent, weapon, material, null, false, explosive);
                bullets.Add(newLaser);
            }

            spread += 5f;
        }

        // Play laser sound
        if (!isBeam) AudioPlayer.Play(laserSound, true, 0.8f, 1.2f, true, 0.8f);
    }

    // Forces the beam to end
    public void EndBeam()
    {
        // Update timer
        if (beams.Count > 0)
        {
            for (int i = 0; i < beams.Count; i++)
            {
                if (beams[i] != null) beams[i].EndEarly();
                else
                {
                    beams.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
