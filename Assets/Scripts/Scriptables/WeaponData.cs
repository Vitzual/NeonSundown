using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Weapon")]
public class WeaponData : CardData
{
    public float damage;
    public float cooldown;
    public float speed;
    public float bloom;
    public float pierces;
    public float bullets;
    public Bullet bullet;
    public bool tracking;

    public void Shoot()
    {
        
    }
}
