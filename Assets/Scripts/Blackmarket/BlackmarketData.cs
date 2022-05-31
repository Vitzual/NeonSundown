using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Blackmarket Item", menuName = "Blackmarket Item")]
public class BlackmarketData : IdentifiableScriptableObject
{
    public enum Type
    {
        Ship,
        Card,
        Arena,
        Chip,
        Audio,
        XP,
        DLC
    }

    [BoxGroup("Item Type")]
    public Type type;
    [ShowIf("type", Type.Ship), BoxGroup("Item Type")]
    public ShipData ship;
    [ShowIf("type", Type.Card), BoxGroup("Item Type")]
    public CardData card;
    [ShowIf("type", Type.Arena), BoxGroup("Item Type")]
    public ArenaData arena;
    [ShowIf("type", Type.Chip), BoxGroup("Item Type")]
    public ChipData chip;

    [BoxGroup("Item Info")]
    public Sprite icon;
    [BoxGroup("Item Info")]
    public new string name;
    [BoxGroup("Item Info"), TextArea]
    public string desc;
    [BoxGroup("Item Info")]
    public int order;

    [BoxGroup("Item Cost")]
    public CrystalType crystal;
    [BoxGroup("Item Cost")]
    public int amountRequired;

    [BoxGroup("Item Color")]
    public Color lightColor;
    [BoxGroup("Item Color")]
    public Color darkColor;
    [BoxGroup("Item Color")]
    public bool useLightColorOnIcon;
}
