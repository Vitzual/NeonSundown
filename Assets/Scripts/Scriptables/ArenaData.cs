using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public ArenaData arenaReward;
        public ShipData playerReward;
        public CardData cardReward;
    }

    // Arena Information
    public new string name;
    [TextArea] public string desc;
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
