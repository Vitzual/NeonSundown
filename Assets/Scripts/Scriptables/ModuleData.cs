using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Module", menuName = "Cards/Module")]
public class ModuleData : IdentifiableScriptableObject
{
    // Module variables
    public new string name;
    [TextArea] public string desc;
    public Sprite icon;
    public Color color;
    public Stat stat;
    public float value;
    public bool multi;
    public CrystalData cost;
}
