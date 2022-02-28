using Sirenix.OdinInspector;
using UnityEngine;

// Variant class
[System.Serializable]
public class VariantData
{
    [BoxGroup("Variant Type")]
    public Variant variant;

    [BoxGroup("Variant Stats")]
    public float health, damage, speed;
    [BoxGroup("Variant Stats")]
    public int minXP, maxXP;
    [BoxGroup("Variant Stats")]
    public bool canDropCrystal;
    [BoxGroup("Variant Stats")]
    public Crystal crystal;
    [BoxGroup("Variant Stats")]
    public float crystalDropChance;

    [BoxGroup("Movement Variables")]
    public bool lockTarget;
    [BoxGroup("Movement Variables")]
    public bool rotate;
    [BoxGroup("Movement Variables")]
    public float rotateSpeed;

    [BoxGroup("Rendering Variables")]
    public bool shakeScreenOnDeath;
}
