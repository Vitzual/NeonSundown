using HeathenEngineering.SteamworksIntegration;
using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievement")]
public class AchievementData : ScriptableObject
{
    public AchievementObject obj;
    public Sprite lockedIcon, unlockedIcon;
    public Color color;
}
