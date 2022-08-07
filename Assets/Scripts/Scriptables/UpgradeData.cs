using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

// Card upgrade class
[CreateAssetMenu(fileName = "New Upgrade", menuName = "Cards/Upgrade")]
public class UpgradeData : IdentifiableScriptableObject
{
    // Special types
    public enum Special
    {
        None,
        Sense,
        Orbital,
        Ninja,
        Sacrifice
    }

    [System.Serializable]
    public class Upgrade
    {
        public Quality quality;
        public float positiveEffect;
        public float negativeEffect;
        public Special special;
    }

    [FoldoutGroup("Upgrade Info")]
    public new string name;
    [FoldoutGroup("Upgrade Info")]
    public List<Upgrade> qualities;
    [FoldoutGroup("Positive Effect")]
    public Stat positiveStat;
    [FoldoutGroup("Positive Effect")]
    public string positiveDesc;
    [FoldoutGroup("Positive Effect")]
    public bool positiveMultiplier;
    [FoldoutGroup("Positive Effect")]
    public bool positiveReduction;
    [FoldoutGroup("Negative Effect")]
    public Stat negativeStat;
    [FoldoutGroup("Negative Effect")]
    public string negativeDesc;
    [FoldoutGroup("Negative Effect")]
    public bool negativeMultiplier;
    [FoldoutGroup("Negative Effect")]
    public bool negativeReduction;
}