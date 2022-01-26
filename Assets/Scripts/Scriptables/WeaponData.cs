using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Weapon")]
public class WeaponData : CardData
{
    public Weapon obj;
    public float damage;
    public float speed;
    public float rotateSpeed;
    public float orbitSpeed;
    public Material material;
    public bool hasActiveInstance;
    public bool setPlayerAsParent;
}
