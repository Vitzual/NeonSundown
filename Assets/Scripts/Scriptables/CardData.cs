using Sirenix.OdinInspector;
using UnityEngine;

public class CardData : IdentifiableScriptableObject
{
    // Card variables
    [BoxGroup("Card Info")]
    public new string name;
    [BoxGroup("Card Info"), TextArea]
    public string description;
    [BoxGroup("Card Info")]
    public Sprite sprite;
    [BoxGroup("Card Info")]
    public Color color;
    [BoxGroup("Card Info")]
    public int maximumAmount;
    [BoxGroup("Card Info")]
    public bool canDrop;
    [BoxGroup("Card Info")]
    public bool isUnlocked;
}
