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
                return 25;
            case 1:
                return 50;
            case 2:
                return 100;
            default:
                return 10;
        }
    }
}
