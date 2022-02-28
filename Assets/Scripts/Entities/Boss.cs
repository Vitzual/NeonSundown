using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // Reference the main enemy script
    public List<Turret> turrets;
    public WeaponData weapon;
    public Bullet bullet;
    protected Enemy enemy;
    protected Transform target;

    // On start, get reference to enemy script
    public void Start()
    {
        // Get enemy and target reference
        enemy = GetComponent<Enemy>();
        target = EnemyHandler.active.player;

        // Setup turrets
        foreach(Turret turret in turrets)
            turret.Setup(weapon, bullet, Random.Range(1f, 2f), target);
    }

    // Control turrets
    public void Update()
    {
        // Keep turrets up to date
        foreach(Turret turret in turrets)
        {
            // Rotate to the target
            float angle = Mathf.Atan2(target.transform.position.y - transform.position.y,
                target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
            transform.rotation = targetRotation;

            // Calculate cooldown
            turret.cooldown -= Time.deltaTime;
            if (turret.cooldown <= 0)
            {
                turret.cooldown = Random.Range(1f, 2f);
                turret.Fire();
            }
        }
    }
}
