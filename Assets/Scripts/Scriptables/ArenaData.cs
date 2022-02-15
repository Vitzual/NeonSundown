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
    public new string name;
    [TextArea] public string desc;
    public TileBase arenaBackground;
    public AudioClip arenaMusic;
    public StageData menuStage;
    public ArenaData arenaRequirement;
    public bool unlockByDefault;

    // Other information
    public string unlockObjective;
    public string difficulty;
    public string length;
    public int order;
    public Color buttonColor;
    public Color lineColor;
    public Color difficultyColor;

    // Arena objectives
    public ArenaObjective primaryObjective;
    public ArenaObjective secondaryObjective;

    // Arena rule set
    public List<StageData> stages;
    public List<CardData> startingCards;
    public List<CardData> blacklistCards;
}
