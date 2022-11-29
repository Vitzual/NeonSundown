using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChillAura : Weapon
{
    // Set the weapon data
    public override void Setup(WeaponData data, Transform target = null)
    {
        moveSpeed = 1f;
        Events.active.onAddCard += AddCard;
        Events.active.onShipOnlyCardAdded += AddCard;
        UpdateStat(Stat.MoveSpeed);
        this.target = target;
    }

    public override void AddCard(CardData card)
    {
        UpdateStat(Stat.MoveSpeed);
    }

    public override void Use()
    {
        if (target != null)
            transform.position = target.position;
    }

    // On collision with enemy, apply damage
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the enemy component
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && !enemy.IsSeeded() && !enemy.isSlowed)
        {
            enemy.moveSpeed *= moveSpeed;
            enemy.isSlowed = true;
        }
    }

    // Update stat
    public override void UpdateStat(Stat stat)
    {
        switch (stat)
        {
            // Increase explosive rounds
            case Stat.MoveSpeed:
                moveSpeed -= 0.1f;
                if (moveSpeed < 0.5f)
                    moveSpeed = 0.5f;
                break;
        }
    }
}
