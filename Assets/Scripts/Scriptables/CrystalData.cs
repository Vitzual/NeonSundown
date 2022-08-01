using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crystal", menuName = "Entities/Crystal")]
public class CrystalData : IdentifiableScriptableObject
{
    public CrystalType type;
    public Material color;
    public Color lightColor, darkColor;
}
