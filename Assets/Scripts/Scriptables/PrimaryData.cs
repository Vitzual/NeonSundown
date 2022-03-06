using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Primary")]
public class PrimaryData : CardData
{
    // Stat class
    [System.Serializable]
    public class StatType
    {
        public Stat type;
        public float modifier;
        public bool multiply;
        public bool positive;
    }

    [BoxGroup("Primary Info")]
    public List<StatType> stats;
}
