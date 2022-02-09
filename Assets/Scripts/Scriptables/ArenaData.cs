using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaData : IdentifiableScriptableObject
{
    public new string name;
    public string desc;
    public List<StageData> stages;
    public List<CardData> blacklistedCards;
}
