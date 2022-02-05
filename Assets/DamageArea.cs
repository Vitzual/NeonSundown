using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : Weapon
{
    // The collider attached to this object
    public bool followPlayer;
    private CircleCollider2D areaOfEffect;

    // Keep a list of recent contacts
    private List<Entity> recentContacts;
    private List<float> recentCooldowns;

    // Override setup
    public override void Setup(WeaponData data, Transform target = null)
    { 
        recentContacts = new List<Entity>();
        recentCooldowns = new List<float>();
        areaOfEffect = GetComponent<CircleCollider2D>();
        areaOfEffect.radius = data.range;
        base.Setup(data, target);
    }

    // Shoots projectiles
    public override void Use()
    {
        // If follow player, do it
        if (followPlayer)
            transform.position = Deck.active.transform.position;

        // Update cooldowns
        for(int i = 0; i < recentContacts.Count; i++)
        {
            if (recentCooldowns[i] <= 0)
            {
                recentContacts.RemoveAt(i);
                recentCooldowns.RemoveAt(i);
                i--;
            }
            else recentCooldowns[i] -= Time.deltaTime;
        }
    }

    // On trigger enter 
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Entity newEntity = collision.GetComponent<Entity>();
        if (newEntity != null && !recentContacts.Contains(newEntity))
        {
            newEntity.Damage(damage);
            recentCooldowns.Add(cooldown);
            recentContacts.Add(newEntity);

            if (weapon.onDamageSound != null)
                AudioPlayer.Play(weapon.onDamageSound);
        }
    }
}
