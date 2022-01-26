using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Primary")]
public class PrimaryData : CardData
{
    public float damage;
    public float cooldown;
    public float speed;
    public float bloom;
    public float pierces;
    public float bullets;
    public float lifetime;
    public Bullet bullet;
    public bool tracking;

    public Sprite model;
    public Material material;
    public Material trail;
    public ParticleSystem effect;
}
