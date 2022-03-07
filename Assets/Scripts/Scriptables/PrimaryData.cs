using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Primary")]
public class PrimaryData : CardData
{
    [BoxGroup("Primary Info")]
    public List<StatValue> stats;
}
