using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class LevelData : IdentifiableScriptableObject
{
    // Rank information
    [HideIf("IsOptionalRewardPicked", true), BoxGroup("Level Reward")]
    public CardData cardReward;
    [HideIf("IsOptionalRewardPicked", true), BoxGroup("Level Reward")]
    public ArenaData arenaReward;
    [HideIf("IsOptionalRewardPicked", true), BoxGroup("Level Reward")]
    public ShipData shipReward;
    [HideIf("redrawReward", true), BoxGroup("Level Reward")]
    public bool crystalReward;
    [HideIf("crystalReward", true), BoxGroup("Level Reward")]
    public bool redrawReward;

    // Rank and reward models
    [BoxGroup("Level Reward")]
    public float xpRequirement = 2500;
    [HideIf("IsRewardPicked", true), BoxGroup("Level Info")]
    public string rewardName;
    [HideIf("IsRewardPicked", true), BoxGroup("Level Info")]
    public string rewardDesc;
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

    // Check if optional flag rewards are picked
    private bool IsOptionalRewardPicked()
    {
        return crystalReward || redrawReward;
    }
} 
