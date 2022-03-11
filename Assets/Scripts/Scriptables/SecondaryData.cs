using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Secondary")]
public class SecondaryData : CardData
{
    public Secondary obj;
    public bool setShipAsParent;
    public float cooldown;
}
