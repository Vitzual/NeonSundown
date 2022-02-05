using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Weapon")]
public class WeaponData : CardData
{
    // Card levels
    [System.Serializable]
    public class Level
    {
        public Stat stat;
        public string description;
        public bool multiply;
        public float modifier;
    }

    [BoxGroup("Weapon Levels")]
    public Sprite prestigeSprite;
    [BoxGroup("Weapon Levels")]
    public List<Level> baseLevels;
    [BoxGroup("Weapon Levels")]
    public List<Level> prestigeLevels;

    [BoxGroup("Weapon Stats")]
    public float damage;
    [BoxGroup("Weapon Stats")]
    public float cooldown;
    [BoxGroup("Weapon Stats")]
    public float moveSpeed;
    [BoxGroup("Weapon Stats")]
    public bool rotate;
    [BoxGroup("Weapon Stats")]
    public float rotateSpeed;
    [BoxGroup("Weapon Stats")]
    public float range;

    [BoxGroup("Bullet Stats")]
    public float bloom;
    [BoxGroup("Bullet Stats")]
    public float pierces;
    [BoxGroup("Bullet Stats")]
    public float bullets;
    [BoxGroup("Bullet Stats")]
    public float lifetime;
    [BoxGroup("Bullet Stats"), Tooltip("Points the weapon in random directions.")]
    public bool randomDirection;
    [BoxGroup("Bullet Stats"), Tooltip("Chooses a random enemy to target.")]
    public bool randomTarget;
    [BoxGroup("Bullet Stats"), Tooltip("Determins if a bullet should follow its target.")]
    public bool trackTarget;
    [BoxGroup("Bullet Stats"), Tooltip("Locks rotation to a target. Only applied if track target is true.")]
    public bool lockTarget;

    [BoxGroup("Object Instances"), Tooltip("The bullet object associated with this SO. Optional.")]
    public Bullet bullet;
    [BoxGroup("Object Instances"), Tooltip("The sound associated with the bullet. Optional.")]
    public AudioClip bulletSound;
    [BoxGroup("Object Instances"), Tooltip("The weapon instance associated with this SO. Optional.")]
    public Weapon obj;
    [BoxGroup("Object Instances"), Tooltip("The sound played when the weapon hits something. Optional.")]
    public AudioClip onDamageSound;
    [BoxGroup("Object Instances"), Tooltip("The sound played when the weapon gets destroyed. Optional.")]
    public AudioClip onDeathSound;
    [BoxGroup("Object Instances"), Tooltip("The minimum pitch value applied to the audio clip. Optional."), Range(0.5f, 1.5f)]
    public float minPitch = 0.9f;
    [BoxGroup("Object Instances"), Tooltip("The maximum pitch value applied to the audio clip. Optional."), Range(0.5f, 1.5f)]
    public float maxPitch = 1.1f;
    [BoxGroup("Object Instances"), Tooltip("If the weapon instance above should be set as a child of the parent.")]
    public bool setPlayerAsParent;

    [BoxGroup("Rendering Variables"), Tooltip("Determines if material should be used.")]
    public bool useMaterial;
    [BoxGroup("Rendering Variables"), Tooltip("Determines if trail should be used.")]
    public bool useTrail;
    [BoxGroup("Rendering Variables"), Tooltip("Determines if particles should be used.")]
    public bool useParticle;

    [BoxGroup("Rendering Variables"), Tooltip("Material that gets applied to particles. Optional.")]
    public Material material;
    [BoxGroup("Rendering Variables"), Tooltip("Trail associated with the weapon OR bullet. Optional.")]
    public Material trail;
    [BoxGroup("Rendering Variables"), Tooltip("Particle effect associated with this weapon. Optional.")]
    public ParticleSystem particle;

    [BoxGroup("Effect Variables"), Tooltip("Shakes the screen on spawn")]
    public bool shakeScreenOnSpawn;
    [BoxGroup("Effect Variables"), Tooltip("Shakes the screen on death")]
    public bool shakeScreenOnDeath;
}
