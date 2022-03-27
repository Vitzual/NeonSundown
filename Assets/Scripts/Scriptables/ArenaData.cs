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


    [BoxGroup("Achievement Info")]
    public string achievementObjective;
    [BoxGroup("Achievement Info"), HideIf("useAchievementBoss", true)]
    public bool useAchievementTime;
    [BoxGroup("Achievement Info"), HideIf("useAchievementBoss", true)]
    public float achievementTime;
    [BoxGroup("Achievement Info"), HideIf("useAchievementTime", true)]
    public bool useAchievementBoss;
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

    // Arena rule set
    [BoxGroup("Arena Rules")]
    public List<StageData> stages;
    [BoxGroup("Arena Rules")]
    public bool useWall;
    [HideIf("NoWall"), BoxGroup("Arena Rules")]
    public Material wallBorder;
    [HideIf("NoWall"), BoxGroup("Arena Rules")]
    public Color wallFill;
    [HideIf("NoWall"), BoxGroup("Arena Rules")]
    public float wallAdjustment;

    public bool NoWall() { return !useWall; }
}
