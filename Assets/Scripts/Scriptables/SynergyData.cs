using HeathenEngineering.SteamworksIntegration;
using UnityEngine;

[CreateAssetMenu(fileName = "New Synergy", menuName = "Cards/Synergy")]
public class SynergyData : IdentifiableScriptableObject
{
    public new string name;
    [TextArea]
    public string desc;
    public CardData cardOne, cardTwo;
    public CardData outputCard;
    public AchievementObject achievement;
}
