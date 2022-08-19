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
    public List<float> values;
    public bool multi;
    public CrystalData cost;

    public int GetCost(int level)
    {
        switch(level)
        {
            case 0:
                return 10;
            case 1:
                return 20;
            case 2:
                return 35;
            case 3:
                return 50;
            default:
                return 5;
        }
    }
}
