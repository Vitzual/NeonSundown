using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChillAura : Weapon
{
    // Enemies being chilled?
    protected Dictionary<Enemy, float> _chilledEntities = new Dictionary<Enemy, float>();

    // Set the weapon data
    public override void Setup(WeaponData data, Transform target = null)
    {
        Events.active.onAddCard += AddCard;
        UpdateStat(Stat.Range);
        UpdateStat(Stat.MoveSpeed);
    }

    // On collision with enemy, apply damage
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the enemy component
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && !enemy.IsSeeded())
        {
            _chilledEntities.Add(enemy, enemy.moveSpeed);
            enemy.moveSpeed *= moveSpeed;
        }
    }

    // On collision with enemy, apply damage
    public void OnTriggerExit2D(Collider2D collision)
    {
        // Get the enemy component
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && !enemy.IsSeeded() && _chilledEntities.ContainsKey(enemy))
        {
            enemy.moveSpeed = _chilledEntities[enemy];
            _chilledEntities.Remove(enemy);
        }
    }
}
