using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Entities/Enemy")]
public class EnemyData : IdentifiableScriptableObject
{
    [BoxGroup("Enemy Info")]
    public new string name;
    [BoxGroup("Enemy Info"), TextArea]
    public string desc;
    [BoxGroup("Enemy Info")]
    public GameObject obj;

    [BoxGroup("Enemy Stats")]
    public float health;
    [BoxGroup("Enemy Stats")]
    public float damage;
    [BoxGroup("Enemy Stats")]
    public float speed;
    [BoxGroup("Enemy Stats")]
    public bool rotate;
    [BoxGroup("Enemy Stats")]
    public float rotateSpeed;
    [BoxGroup("Enemy Stats")]
    public int minXP;
    [BoxGroup("Enemy Stats")]
    public int maxXP;

    [BoxGroup("Rendering Variables")]
    public Material material;
    [BoxGroup("Rendering Variables")]
    public ParticleSystem deathParticle;
}
