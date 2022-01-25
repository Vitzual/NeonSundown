using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Card")]
public class CardData : IdentifiableScriptableObject
{
    // Card variables
    public new string name;
    [TextArea] public string description;
    public float dropchance;
}
