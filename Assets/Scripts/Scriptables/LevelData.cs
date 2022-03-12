using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class LevelData : IdentifiableScriptableObject
{
    // Rank information
    [BoxGroup("Level Reward")]
    public CardData cardReward;
    [BoxGroup("Level Reward")]
    public ArenaData arenaReward;
    [BoxGroup("Level Reward")]
    public ShipData shipReward;
    [BoxGroup("Level Reward")]
    public bool crystalReward;

    // Rank and reward models
    [BoxGroup("Level Reward")]
    public float xpRequirement = 1000;
    [HideIf("IsRewardPicked", true), BoxGroup("Level Info")]
    public new string name;
    [HideIf("IsRewardPicked", true), BoxGroup("Level Info")]
    public string desc;
    [HideIf("IsRewardPicked", true), BoxGroup("Level Info")]
    public Sprite rewardIcon;
    [HideIf("IsRewardPicked", true), BoxGroup("Level Info")]
    public Color levelColor;
    [HideIf("IsRewardPicked", true), BoxGroup("Level Info")]
    public Color rewardColor;

    // Check if rewards not null
    private bool IsRewardPicked()
    {
        return cardReward != null || arenaReward != null || shipReward != null;
    }
} 
