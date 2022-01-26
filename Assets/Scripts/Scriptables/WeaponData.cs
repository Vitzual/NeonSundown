using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Weapon")]
public class WeaponData : CardData
{
    public Weapon obj;

    public float damage;
    public float cooldown;
    public float moveSpeed;
    public float rotateSpeed;
    public float bloom;
    public float pierces;
    public float bullets;
    public float lifetime;
    public bool randomTarget;
    public bool trackTarget;
    public bool lockTarget;
    public Bullet bullet;

    public bool useSprite;
    public bool useMaterial;
    public bool useTrail;

    public Sprite sprite;
    public Material material;
    public Material trail;
    public ParticleSystem effect;
    public bool shakeScreen;

    public bool setPlayerAsParent;
}
