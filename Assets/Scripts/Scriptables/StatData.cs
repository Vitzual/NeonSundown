using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Stat")]
public class StatData : CardData
{
    [BoxGroup("Stat Info")]
    public StatValue value;
    [BoxGroup("Stat Info")]
    public bool applyToCards;
    [BoxGroup("Stat Info")]
    public bool applyToPlayer;
}
