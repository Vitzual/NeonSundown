using HeathenEngineering.SteamworksIntegration;
using UnityEngine;

[CreateAssetMenu(fileName = "New Synergy", menuName = "Cards/Synergy")]
public class SynergyData : IdentifiableScriptableObject
{
    public new string name;
    [TextArea]
    public string desc;
    public Color primaryColor, backgroundColor;
    public CardData cardOne, cardTwo;
    public bool removeCardOne, removeCardTwo;
    public CardData outputCard;
    public AchievementObject achievement;
    public Sprite achievementIcon;
}
