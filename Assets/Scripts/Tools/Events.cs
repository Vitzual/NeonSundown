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

    public event Action onAuthenticationFinished;
    public void FinishAuthentication()
    {
        if (onAuthenticationFinished != null)
            onAuthenticationFinished();
    }

    public event Action<string> onAuthenticationFailed;
    public void AuthenticationFailed(string msg)
    {
        if (onAuthenticationFailed != null)
            onAuthenticationFailed(msg);
    }

    public event Action onResetCooldown;
    public void ResetCooldown()
    {
        if (onResetCooldown != null)
            onResetCooldown();
    }

    // Card added event
    public event Action<CardData> onAddCard;
    public void AddCard(CardData card)
    {
        if (onAddCard != null)
            onAddCard(card);
    }

    // Card added event
    public event Action<CardData> onShipOnlyCardAdded;
    public void AddShipOnlyCard(CardData card)
    {
        if (onShipOnlyCardAdded != null)
            onShipOnlyCardAdded(card);
    }

    // Synergy added event
    public event Action<SynergyData> onAddSynergy;
    public void AddSynergy(SynergyData card)
    {
        if (onAddSynergy != null)
            onAddSynergy(card);
    }

    // Synergy added event
    public event Action<XPReceiver> onXPReceiverStart;
    public void XPReceiverStart(XPReceiver xpReceiver)
    {
        if (onXPReceiverStart != null)
            onXPReceiverStart(xpReceiver);
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

    public event Action<BlackmarketData> onBlackmarketItemBought;
    public void BlackmarketItemBought(BlackmarketData data)
    {
        if (onBlackmarketItemBought != null)
            onBlackmarketItemBought(data);
    }
}