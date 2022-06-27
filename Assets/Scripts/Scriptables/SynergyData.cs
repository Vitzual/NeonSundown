using HeathenEngineering.SteamworksIntegration;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Synergy", menuName = "Cards/Synergy")]
public class SynergyData : IdentifiableScriptableObject
{
    public new string name;
    [TextArea]
    public string desc;
    [Range(1, 3)]
    public int tier;
    public Color backgroundColor;
    public CardData cardOne;
    [HideIf("tier", 3)]
    public CardData cardTwo;
    [ShowIf("tier", 3)]
    public List<ChipData> arenaChips;
    public bool removeCardOne;
    [HideIf("tier", 3)]
    public bool removeCardTwo;
    public CardData outputCard;
    public AchievementObject achievement;
    [ShowIf("tier", 2)]
    public SynergyData synergyOne, synergyTwo;

    public bool IsTierOne() { return tier <= 1; }
}
