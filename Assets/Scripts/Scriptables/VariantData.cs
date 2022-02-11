using Sirenix.OdinInspector;
using UnityEngine;

// Variant class
[System.Serializable]
public class VariantData
{
    [BoxGroup("Variant Model")]
    public Variant type;
    [BoxGroup("Variant Model")]
    public Color color;
    [BoxGroup("Variant Model")]
    public Material material;
    [BoxGroup("Variant Model")]
    public ParticleSystem deathParticle;

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
    public bool rotate;
    [BoxGroup("Movement Variables")]
    public float rotateSpeed;
}
