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
    public CardData cardTwo;
    [ShowIf("tier", 2)]
    public int order;
    public bool removeCardOne;
    public bool removeCardTwo;
    public CardData outputCard;
    public AchievementObject achievement;
    [ShowIf("tier", 2)]
    public SynergyData synergyOne, synergyTwo;

    public bool IsMasterSynergy() { return tier == 2; }
}
