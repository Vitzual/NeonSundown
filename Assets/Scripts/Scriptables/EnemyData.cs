using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Entities/Enemy")]
public class EnemyData : ScriptableObject
{
    public GameObject obj;
    public Material material;
    public new string name;
    [TextArea] public string desc;
    public float health;
    public float damage;
    public float speed;
    public float minXP;
    public float maxXP;
    public int minRound;
    public int maxRound;
}
