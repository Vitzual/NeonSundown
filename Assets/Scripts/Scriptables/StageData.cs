using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stage", menuName = "Arena/Stage")]
public class StageData : IdentifiableScriptableObject
{
    [System.Serializable]
    public class Enemy
    {
        public EnemyData data;
        public Variant variant;
        public float cooldown;
        public int amount = 1;
    }

    public new string name;
    public bool isMenuStage;
    public List<Enemy> enemies;
    public float time;
}
