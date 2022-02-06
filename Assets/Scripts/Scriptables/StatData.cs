using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Stat")]
public class StatData : CardData
{
    public Stat type;
    public bool multiply;
    public float modifier;
    public bool applyToCards;
    public bool applyToPlayer;
}
