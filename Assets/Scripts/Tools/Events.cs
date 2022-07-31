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
    public event Action<SynergyData> onSynergyAvailable;
    public void SynergyAvailable(SynergyData synergy)
    {
        if (onSynergyAvailable != null)
            onSynergyAvailable(synergy);
    }

    // On crystal broken
    public event Action onCrystalBroken;
    public void CrystalBroken()
    {
        if (onCrystalBroken != null)
            onCrystalBroken();
    }

    // On blood crystal broken 
    public event Action onBloodCrystalBroken;
    public void BloodCrystalBroken()
    {
        if (onBloodCrystalBroken != null)
            onBloodCrystalBroken();
    }

    // On boss hurt
    public event Action onBossHurt;
    public void BossHurt()
    {
        if (onBossHurt != null)
            onBossHurt();
    }

    // On boss hurt
    public event Action onBossDestroyed;
    public void BossDestroyed()
    {
        if (onBossDestroyed != null)
            onBossDestroyed();
    }

    // On boss spawned
    public event Action<LevelData, int> onLevelUp;
    public void LevelUp(LevelData levelData, int level)
    {
        if (onLevelUp != null)
            onLevelUp(levelData, level);
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
    public event Action<SecondaryData> onSecondarySet;
    public void SetSecondary(SecondaryData secondary)
    {
        if (onSecondarySet != null)
            onSecondarySet(secondary);
    }

    // Cosmetic applied
    public event Action<float> onVolumeChanged;
    public void VolumeChanged(float volume)
    {
        if (onVolumeChanged != null)
            onVolumeChanged(volume);
    }

    // Player died
    public event Action onMusicPitchChanged;
    public void ResetPitch()
    {
        if (onMusicPitchChanged != null)
            onMusicPitchChanged();
    }
    
    // Cosmetic applied
    public event Action<float> onLightChanged;
    public void LightChanged(float volume)
    {
        if (onLightChanged != null)
            onLightChanged(volume);
    }

    // Ship coloring change
    public event Action<bool> onShipColoringChange;
    public void ShipColoring(bool toggle)
    {
        if (onShipColoringChange != null)
            onShipColoringChange(toggle);
    }

    // Ship coloring change
    public event Action<bool> onUpdateShowHP;
    public void UpdateShowHP(bool toggle)
    {
        if (onUpdateShowHP != null)
            onUpdateShowHP(toggle);
    }

    public event Action<BlackmarketData> onBlackmarketItemClicked;
    public void BlackmarketItemClicked(BlackmarketData data)
    {
        if (onBlackmarketItemClicked != null)
            onBlackmarketItemClicked(data);
    }
}