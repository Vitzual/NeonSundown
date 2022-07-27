using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chip", menuName = "Arena/Chip")]
public class ChipData : IdentifiableScriptableObject
{
    public Stat stat;
    public CrystalType crystal;
    public int cost;
    public float modifier;
    public float xpBoost;
    public float crystalBoost;
}
