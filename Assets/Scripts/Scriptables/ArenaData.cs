using HeathenEngineering.SteamworksIntegration;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Arena", menuName = "Arena/Arena")]
public class ArenaData : IdentifiableScriptableObject
{
    [System.Serializable]
    public class ArenaCard
    {
        // Card data
        public CardData card;
        public int amount;
    }

    // Arena Information
    [BoxGroup("Arena Info")]
    public new string name;
    [BoxGroup("Arena Info")]
    public Sprite enemyIcon;
    [BoxGroup("Arena Info")]
    public Sprite unlockedIcon;
    [BoxGroup("Arena Info")]
    public Sprite lockedIcon;
    [BoxGroup("Arena Info"), TextArea]
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
    public Difficulty difficulty;
    [BoxGroup("Arena Info")]
    public EnemyData boss;
    [BoxGroup("Arena Info")]
    public float startingViewRange = 45f;

    [BoxGroup("Limited Time")]
    public bool limitedTimeArena;
    [BoxGroup("Limited Time"), ShowIf("limitedTimeArena", true)]
    public int limitedTimeYear;
    [BoxGroup("Limited Time"), ShowIf("limitedTimeArena", true)]
    public int limitedTimeMonth;
    [BoxGroup("Limited Time"), ShowIf("limitedTimeArena", true)]
    public int limitedTimeDay;

    [BoxGroup("Achievement Info")]
    public string achievementObjective;
    [BoxGroup("Achievement Info")]
    public bool showObjectiveOnStart = true;
    [BoxGroup("Achievement Info"), HideIf("useAchievementKills", true)]
    public bool useAchievementTime;
    [BoxGroup("Achievement Info"), ShowIf("useAchievementTime", true)]
    public float achievementTime;
    [BoxGroup("Achievement Info"), HideIf("useAchievementTime", true)]
    public bool useAchievementKills;
    [BoxGroup("Achievement Info"), ShowIf("useAchievementKills", true)]
    public EnemyData enemyToKill;
    [BoxGroup("Achievement Info")]
    public AchievementObject achievement;

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
    [BoxGroup("Arena Stats")]
    public Color darkColor;

    // Arena rule set
    [BoxGroup("Arena Rules")]
    public List<StageData> stages;
    [BoxGroup("Arena Rules")]
    public List<CardData> blacklistCards;
    [BoxGroup("Arena Rules")]
    public bool useVault;
    [BoxGroup("Arena Rules")]
    public bool useWall;
    [HideIf("NoWall"), BoxGroup("Arena Rules")]
    public Material wallBorder;
    [HideIf("NoWall"), BoxGroup("Arena Rules")]
    public Color wallFill;
    [HideIf("NoWall"), BoxGroup("Arena Rules")]
    public float wallAdjustment;

    public bool NoWall() { return !useWall; }

    public bool IsAchievementUnlocked() { return achievement != null && achievement.IsAchieved; }
}
