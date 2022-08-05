using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class CardData : IdentifiableScriptableObject
{
    // Card upgrade class
    [System.Serializable]
    public class Upgrade
    {
        public Stat stat;
        public string name, desc;
        public float minValue, maxValue;
        public bool multiply, lowerIsBetter;
    }

    // Card variables
    [BoxGroup("Card Info")]
    public new string name;
    [BoxGroup("Card Info"), TextArea]
    public string description;
    [BoxGroup("Card Info")]
    public bool enableDescriptionOverrides;
    [BoxGroup("Card Info"), ShowIf("enableDescriptionOverrides")]
    public SerializableDictionary<int, string> levelDescriptionOverrides;
    [BoxGroup("Card Info")]
    public Sprite sprite;
    [BoxGroup("Card Info")]
    public Color color;
    [BoxGroup("Card Info")]
    public int maximumAmount;
    [BoxGroup("Card Info"), Range(0f, 1f)]
    public float dropChance = 1f;
    [BoxGroup("Card Info")]
    public bool isUpgradeable;
    [ShowIf("isUpgradeable"), BoxGroup("Weapon Levels")]
    public List<Upgrade> upgrades;
    [BoxGroup("Card Info")]
    public bool canDrop;
    [BoxGroup("Card Info")]
    public bool isUnlocked;
    [BoxGroup("Card Info")]
    public bool isShipOnlyCard;
}
