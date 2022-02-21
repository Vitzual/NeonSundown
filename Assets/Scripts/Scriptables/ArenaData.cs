using Sirenix.OdinInspector;
using System.Collections;
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
        public string name;
        public string reward;

        // Objective enum and rewards
        public Objective objective;

        // Objectives
        public float surviveTime;
        public EnemyData enemyData;
        public int enemyAmount;

        // Rewards
        public ShipData shipReward;
        public CardData cardReward;
    }

    // Arena Information
    [BoxGroup("Arena Info")]
    public new string name;
    [BoxGroup("Arena Info"), TextArea]
    public string desc;
    [BoxGroup("Arena Info")]
    public TileBase arenaBackground;
    [BoxGroup("Arena Info")]
    public AudioClip arenaMusic;
    [BoxGroup("Arena Info")]
    public StageData menuStage;

    // Other information
    [BoxGroup("Arena Requirement")]
    public ArenaData arenaRequirement;
    [BoxGroup("Arena Requirement")]
    public bool unlockByDefault;
    [BoxGroup("Arena Requirement")]
    public string unlockObjective;

    // Arena stats
    [BoxGroup("Arena Stats")]
    public string difficulty;
    [BoxGroup("Arena Stats")]
    public string length;
    [BoxGroup("Arena Stats")]
    public int order;
    [BoxGroup("Arena Stats")]
    public Color buttonColor;
    [BoxGroup("Arena Stats")]
    public Color lineColor;
    [BoxGroup("Arena Stats")]
    public Color difficultyColor;

    // Arena objectives
    [BoxGroup("Arena Objective")]
    public ArenaObjective primaryObjective;
    [BoxGroup("Arena Objective")]
    public ArenaObjective secondaryObjective;

    // Arena rule set
    [BoxGroup("Arena Rules")]
    public List<StageData> stages;
    [BoxGroup("Arena Rules")]
    public List<CardData> startingCards;
    [BoxGroup("Arena Rules")]
    public List<CardData> blacklistCards;
    [BoxGroup("Arena Rules")]
    public bool useWall = false;
}