using HeathenEngineering.SteamworksIntegration;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Synergy", menuName = "Cards/Synergy")]
public class SynergyData : IdentifiableScriptableObject
{
    public new string name;
    [TextArea]
    public string desc;
    public int tier;
    public Color backgroundColor;
    public CardData cardOne, cardTwo;
    public bool removeCardOne, removeCardTwo;
    public CardData outputCard;
    public AchievementObject achievement;
    [HideIf("IsTierOne", true)]
    public SynergyData synergyOne, synergyTwo;

    public bool IsTierOne() { return tier <= 1; }
}
