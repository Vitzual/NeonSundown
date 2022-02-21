using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ship", menuName = "Ship")]
public class ShipData : IdentifiableScriptableObject
{
    [BoxGroup("Ship Info")]
    public new string name;
    [BoxGroup("Ship Info"), TextArea]
    public string desc;
    [BoxGroup("Ship Info")]
    public bool unlocked;

    [BoxGroup("Ship Model")]
    public Sprite model;
    [BoxGroup("Ship Model")]
    public Sprite border;
    [BoxGroup("Ship Model")]
    public Sprite fill;
    [BoxGroup("Ship Model")]
    public Material borderMaterial;
    [BoxGroup("Ship Model")]
    public Color fillColor;
    [BoxGroup("Ship Model")]
    public Vector2 playerSize;
    [BoxGroup("Ship Model")]
    public Vector2 barrelPosition;
    [BoxGroup("Ship Model")]
    public Vector2 modelOffset;

    [BoxGroup("Ship Data")]
    public WeaponData weapon;
    [BoxGroup("Ship Data")]
    public bool playerControlledRotation;
    [BoxGroup("Ship Data")]
    public bool canRegen;
    [BoxGroup("Ship Data")]
    public bool canFire;
    [BoxGroup("Ship Data")]
    public float regenRate;
    [BoxGroup("Ship Data")]
    public float playerSpeed;
    [BoxGroup("Ship Data")]
    public float dashSpeed;
    [BoxGroup("Ship Data")]
    public float startingHealth;

    // Checks if the ship is unlocked
    public bool IsUnlocked()
    {
        return unlocked || SaveSystem.IsShipUnlocked(InternalID);
    }
}