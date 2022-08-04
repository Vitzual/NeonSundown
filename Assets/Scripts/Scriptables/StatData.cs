using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Stat")]
public class StatData : CardData
{
    [BoxGroup("Primary Info")]
    public List<StatValue> stats;
}
