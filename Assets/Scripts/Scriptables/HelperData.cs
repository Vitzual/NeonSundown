using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Helper", menuName = "Cards/Helper")]
public class HelperData : CardData
{
    // Card levels
    [System.Serializable]
    public class Level
    {
        public StatValue stat;
        public string description;
    }

    public Helper obj;
    public List<Level> levels;
    public float moveSpeed, rotationSpeed, statAmount, knockback;
}

