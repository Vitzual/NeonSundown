using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stage", menuName = "Stage")]
public class StageData : IdentifiableScriptableObject
{
    [System.Serializable]
    public class Enemy
    {
        public EnemyData data;
        public float cooldown;
    }

    public new string name;
    public List<Enemy> enemies;
    public float time;
}
