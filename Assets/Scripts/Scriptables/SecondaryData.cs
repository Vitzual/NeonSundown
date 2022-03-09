using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryData : CardData
{
    public enum Type
    {
        Mine,
        EMPMine
    }

    public Type type;
    public Secondary script;
    public float cooldown;
}
