using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Entities/Enemy")]
public class EnemyData : IdentifiableScriptableObject
{
    public GameObject obj;
    public Material material;
    public ParticleSystem deathParticle;
    public new string name;
    [TextArea] public string desc;
    public float health;
    public float damage;
    public float speed;
    public int xpDrops;
    public float spawnTime;
    [Range(0, 100)]
    public int spawnChance;
    public bool rotate;
    public float rotateSpeed;
    public bool isDashResistent;
}
