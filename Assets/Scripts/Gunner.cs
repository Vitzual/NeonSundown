using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : MonoBehaviour
{
    // List of turrets
    public List<Turret> turrets;
    public WeaponData weapon;
    public bool selfSetup = false;
    protected Transform target;

    // Setup on staRt
    public void Start()
    {
        if (selfSetup)
            Setup(weapon.material);
    }

    // Sets up a thing
    public void Setup(Material material = null)
    {
        // Get enemy and target reference
        if (EnemyHandler.active != null && EnemyHandler.active.player != null)
            target = EnemyHandler.active.player;

        // Setup turrets
        foreach (Turret turret in turrets)
            turret.Setup(weapon, Random.Range(1f, 2f), target, material);
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
