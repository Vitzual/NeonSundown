using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Weapon")]
public class WeaponData : CardData
{
    public Weapon weapon;
    public float damage;
    public float speed;
    public Material material;
}
