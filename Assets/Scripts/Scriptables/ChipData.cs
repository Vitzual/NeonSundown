using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chip", menuName = "Arena/Chip")]
public class ChipData : IdentifiableScriptableObject
{
    public Stat stat;
    public float effect;
    public CrystalType crystal;
    public int cost;
    public Difficulty difficulty;
}
