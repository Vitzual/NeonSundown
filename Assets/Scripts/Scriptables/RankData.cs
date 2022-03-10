using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rank", menuName = "Rank")]
public class RankData : IdentifiableScriptableObject
{ 
    // Rank and reward models
    public Sprite rankIcon;
    public Sprite rewardIcon;

    // Rank information
    public float xpRequirement;
    public CardData cardReward;
    public ArenaData arenaReward;
    public ModuleData moduleReward;
    public int moduleAmount;
    public CrystalData crystalReward;
    public int crystalAmount;
} 
