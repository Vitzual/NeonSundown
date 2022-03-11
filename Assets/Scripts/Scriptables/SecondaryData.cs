using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Secondary")]
public class SecondaryData : CardData
{
    // Card levels
    [System.Serializable]
    public class Level
    {
        public StatValue stat;
        public string description;
    }

    public Secondary obj;
    public bool setShipAsParent;
    public float cooldown;
    public List<Level> levels;
}
