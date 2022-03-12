using HeathenEngineering.SteamworksIntegration;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Arena", menuName = "Arena/Arena")]
public class ArenaData : IdentifiableScriptableObject
{
    [System.Serializable]
    public class ArenaObjective
    {
        // Objective description
        public string rewardName;
        public Sprite rewardImage;
        public Color rewardColor = Color.white;
        public float timeRequired;

        // Rewards
        public ArenaData arenaReward;
        public ShipData shipReward;
        public CardData cardReward;
        public AchievementObject achievementReward;
    }

    [System.Serializable]
    public class ArenaCard
    {
        // Card data
        public CardData card;
        public int amount;
        public bool prestige;
    }

    // Arena Information
    [BoxGroup("Arena Info")]
    public new string name;
    [BoxGroup("Arena Info")]
    public Sprite icon;
    [BoxGroup("Arena Info")]
    public Sprite iconEnemy;
    [BoxGroup("Arena Info"), TextArea]
    public string desc;
    [BoxGroup("Arena Info")]
    public string shortDesc;
    [BoxGroup("Arena Info")]
    public TileBase arenaBackground;
    [BoxGroup("Arena Info")]
    public float arenaMenuPitch = 1f;
    [BoxGroup("Arena Info")]
    public AudioClip arenaMusic;
    [BoxGroup("Arena Info")]
    public StageData menuStage;
    [BoxGroup("Arena Info")]
    public bool availableInDemo;
    
    // Other information
    [BoxGroup("Arena Requirement")]
    public bool unlockByDefault;
    [BoxGroup("Arena Requirement")]
    public string unlockObjective;

    // Arena stats
    [BoxGroup("Arena Stats")]
    public int order;
    [BoxGroup("Arena Stats")]
    public Color buttonColor;
    [BoxGroup("Arena Stats")]
    public Color lightColor;

    // Arena objectives
    [BoxGroup("Arena Objective")]
    public ArenaObjective objectiveOne;
    [BoxGroup("Arena Objective")]
    public ArenaObjective objectiveTwo;
    [BoxGroup("Arena Objective")]
    public ArenaObjective objectiveThree;
    [BoxGroup("Arena Objective")]
    public ArenaObjective objectiveFour;

    // Arena rule set
    [BoxGroup("Arena Rules")]
    public List<StageData> stages;
    [BoxGroup("Arena Rules")]
    public List<ArenaCard> startingCards;
    [BoxGroup("Arena Rules")]
    public List<CardData> blacklistCards;
    [BoxGroup("Arena Rules")]
    public bool useWall = false;
}
