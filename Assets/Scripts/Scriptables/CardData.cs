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
    public bool canDrop;
    [BoxGroup("Card Sprite")]
    public Sprite sprite;
    [BoxGroup("Card Info")]
    public Color color;
}
