using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chip", menuName = "Arena/Chip")]
public class ChipData : IdentifiableScriptableObject
{
    [System.Serializable]
    public class Tier
    {
        public int cost;
        public float modifier;
    }

    public Stat stat;
    public CrystalType crystal;
    public Difficulty difficulty;
    public List<Tier> tiers;
}
