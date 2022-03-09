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

    // On boss hurt
    public event Action onBossHurt;
    public void BossHurt()
    {
        if (onBossHurt != null)
            BossHurt();
    }

    // On boss spawned
    public event Action<Boss, Enemy> onBossSpawned;
    public void BossSpawned(Boss boss, Enemy enemy)
    {
        if (onBossSpawned != null)
            onBossSpawned(boss, enemy);
    }

    // Player died
    public event Action onShipDestroyed;
    public void ShipDestroyed()
    {
        if (onShipDestroyed != null)
            onShipDestroyed();
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

    // Cosmetic applied
    public event Action<float> onVolumeChanged;
    public void VolumeChanged(float volume)
    {
        if (onVolumeChanged != null)
            onVolumeChanged(volume);
    }

    // Cosmetic applied
    public event Action<float> onGlowChanged;
    public void GlowChanged(float volume)
    {
        if (onGlowChanged != null)
            onGlowChanged(volume);
    }
}