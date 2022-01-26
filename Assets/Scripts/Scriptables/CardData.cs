using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : IdentifiableScriptableObject
{
    // Card variables
    public new string name;
    [TextArea] public string description;
    public float dropchance;
}
