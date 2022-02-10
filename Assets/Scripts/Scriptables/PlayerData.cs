using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Cards/Player")]
public class PlayerData : WeaponData
{
    // Start is called before the first frame update
    [BoxGroup("Player Data")]
    public Vector2 playerSize;
    [BoxGroup("Player Data")]
    public Vector2 barrelPosition;
    [BoxGroup("Player Data")]
    public Vector2 modelOffset;
    [BoxGroup("Player Data")]
    public Sprite border;
    [BoxGroup("Player Data")]
    public Sprite fill;
    [BoxGroup("Player Data")]
    public Material borderMaterial;
    [BoxGroup("Player Data")]
    public Color fillColor;
    [BoxGroup("Player Data")]
    public bool playerControlledRotation;
    [BoxGroup("Player Data")]
    public bool canRegen;
    [BoxGroup("Player Data")]
    public bool canFire;
    [BoxGroup("Player Data")]
    public float regenRate;
    [BoxGroup("Player Data")]
    public float playerSpeed;
    [BoxGroup("Player Data")]
    public float dashSpeed;
    [BoxGroup("Player Data")]
    public float startingHealth;
}
