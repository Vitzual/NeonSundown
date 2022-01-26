using Sirenix.OdinInspector;
using UnityEngine;

public class CardData : IdentifiableScriptableObject
{
    // Card variables
    [BoxGroup("Card Info")]
    public new string name;
    [BoxGroup("Card Info")]
    [TextArea] public string description;
    [BoxGroup("Card Info")]
    public float dropchance;
}
