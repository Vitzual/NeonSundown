using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    // Active events instance
    public static Events active;

    // Start is called before the first frame update
    public void Awake()
    {
        active = this;
    }

    // Cosmetic applied
    public event Action<ShipData> onSetupShip;
    public void SetupShip(ShipData ship)
    {
        if (onSetupShip != null)
            onSetupShip(ship);
    }

    // Cosmetic applied
    public event Action<ArenaData> onArenaButtonClicked;
    public void ArenaButtonClicked(ArenaData arena)
    {
        if (onArenaButtonClicked != null)
            onArenaButtonClicked(arena);
    }
}