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
    [HideIf("IsOptionalRewardPicked", true), BoxGroup("Level Reward")]
    public SynergyData synergyReward;
    [HideIf("redrawReward", true), HideIf("marketReward", true), BoxGroup("Level Reward")]
    public bool crystalReward;
    [HideIf("crystalReward", true), HideIf("marketReward", true), BoxGroup("Level Reward")]
    public bool redrawReward;
    [HideIf("crystalReward", true), HideIf("redrawReward", true), BoxGroup("Level Reward")]
    public bool marketReward;
    [BoxGroup("Level Reward")]
    public float xpRequirement = 25000;

    // Rank and reward models
    [HideIf("IsRewardPicked", true), BoxGroup("Level Info")]
    public string rewardName;
    [HideIf("IsRewardPicked", true), BoxGroup("Level Info")]
    public string rewardDesc;
    [HideIf("IsRewardPicked", true), BoxGroup("Level Info")]
    public Sprite rewardIcon;
    [HideIf("IsRewardPicked", true), BoxGroup("Level Info")]
    public Sprite lockedIcon;
    [HideIf("IsRewardPicked", true), BoxGroup("Level Info")]
    public Color levelColor;
    [HideIf("IsRewardPicked", true), BoxGroup("Level Info")]
    public Color rewardColor;

    // Check if rewards not null
    private bool IsRewardPicked()
    {
        return cardReward != null || arenaReward != null || shipReward != null || synergyReward != null;
    }

    // Check if optional flag rewards are picked
    private bool IsOptionalRewardPicked()
    {
        return crystalReward || redrawReward;
    }

    // Get name
    public string GetName()
    {
        if (cardReward != null) return cardReward.name;
        else if (arenaReward != null) return arenaReward.name;
        else if (shipReward != null) return shipReward.name;
        else if (synergyReward != null) return synergyReward.name;
        else return rewardName;
    }

    // Get color
    public Color GetColor()
    {
        if (cardReward != null) return cardReward.color;
        else if (arenaReward != null) return arenaReward.lightColor;
        else if (shipReward != null) return shipReward.mainColor;
        else if (synergyReward != null) return synergyReward.outputCard.color;
        else return levelColor;
    }

    // Get icon
    public Sprite GetIcon()
    {
        if (cardReward != null) return cardReward.sprite;
        else if (arenaReward != null) return arenaReward.enemyIcon;
        else if (shipReward != null) return shipReward.glowIcon;
        else if (synergyReward != null) return synergyReward.outputCard.sprite;
        else return rewardIcon;
    }

    // Check if should be colored
    public bool IsColored()
    {
        return cardReward != null || arenaReward != null || shipReward != null || synergyReward != null || redrawReward;
    }
} 
