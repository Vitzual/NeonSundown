using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ship", menuName = "Ship")]
public class ShipData : IdentifiableScriptableObject
{
    [BoxGroup("Ship Info")]
    public new string name;
    [BoxGroup("Ship Info")]
    public Sprite glowIcon;
    [BoxGroup("Ship Info"), TextArea]
    public string desc;
    [BoxGroup("Ship Info")]
    public bool hasPassive;
    [BoxGroup("Ship Info"), ShowIf("hasPassive")]
    public string passive;
    [BoxGroup("Ship Info")]
    public string unlockRequirement;
    [BoxGroup("Ship Info")]
    public string levelRequirement;
    [BoxGroup("Ship Info")]
    public bool unlocked;
    [BoxGroup("Ship Info")]
    public List<CardData> incompatibleCards;

    [BoxGroup("Ship Selection Variables")]
    public string subTitle, shortDesc;
    [BoxGroup("Ship Selection Variables")]
    public Color mainColor, lightColor, darkColor, veryDarkColor;
    [BoxGroup("Ship Selection Variables")]
    public int listOrder;

    [BoxGroup("Ship Model")]
    public Sprite model;
    [BoxGroup("Ship Model")]
    public Sprite border;
    [BoxGroup("Ship Model")]
    public Sprite fill;
    [BoxGroup("Ship Model")]
    public Material borderMaterial;
    [BoxGroup("Ship Model")]
    public Vector2 playerSize;
    [BoxGroup("Ship Model")]
    public Vector2 barrelPosition;
    [BoxGroup("Ship Model")]
    public Vector2 modelOffset;

    [BoxGroup("Ship Data")]
    public bool cryoShip;
    [BoxGroup("Ship Data"), ShowIf("cryoShip", true)]
    public float slowMultiplier;

    [BoxGroup("Ship Data")]
    public bool droneShip;
    [BoxGroup("Ship Data"), ShowIf("droneShip", true)]
    public HelperData drone;
    [BoxGroup("Ship Data"), ShowIf("droneShip", true)]
    public int droneAmount;
    [BoxGroup("Ship Data"), ShowIf("droneShip", true)]
    public float droneDamage;
    [BoxGroup("Ship Data"), ShowIf("droneShip", true)]
    public float droneMoveSpeed;
    [BoxGroup("Ship Data"), ShowIf("droneShip", true)]
    public float droneRotateSpeed;

    [BoxGroup("Ship Data")]
    public bool chargingShip;
    [BoxGroup("Ship Data"), ShowIf("chargingShip", true)]
    public float maxChargeTime;
    [BoxGroup("Ship Data"), ShowIf("chargingShip", true)]
    public float chargeSpeed;
    [BoxGroup("Ship Data"), ShowIf("chargingShip", true)]
    public float chargeDamage;

    [BoxGroup("Ship Data")]
    public bool seedShip;
    [BoxGroup("Ship Data"), ShowIf("seedShip", true)]
    public Seed seed;
    [BoxGroup("Ship Data"), ShowIf("seedShip", true)]
    public AudioClip seedDeploySound;
    [BoxGroup("Ship Data"), ShowIf("seedShip", true)]
    public AudioClip seedBoomSound;

    [BoxGroup("Ship Data")]
    public bool speedAffectsDamage;
    [BoxGroup("Ship Data"), ShowIf("speedAffectsDamage", true)]
    public float speedDamageMultiplier;

    [BoxGroup("Ship Data")]
    public WeaponData weapon;
    [BoxGroup("Ship Data")]
    public bool playerControlledRotation;
    [BoxGroup("Ship Data")]
    public bool canFire;
    [BoxGroup("Ship Data")]
    public float regenAmount;
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

    // Returns a stat
    public float GetStat(Stat stat)
    {
        switch (stat)
        {
            case Stat.Health:
                return startingHealth;
            case Stat.MoveSpeed:
                return playerSpeed;
            case Stat.DashSpeed:
                return dashSpeed;
            case Stat.Regen:
                return regenAmount;
            default:
                return weapon.GetStat(stat);
        }
    }
}
