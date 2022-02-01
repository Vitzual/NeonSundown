using System.Collections.Generic;
using UnityEngine;

// List of all events
public class Stage : IdentifiableScriptableObject
{
    [SerializeField]
    public class Enemy
    {
        public EnemyData data;
        public float cooldown;
    }

    public List<Enemy> enemies;
    public float time;
}
