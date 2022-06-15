using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromaHandler : MonoBehaviour
{
    // Active chroma handler instance
    public static ChromaHandler active;

    // Get active instance
    public void Awake()
    {
        active = this;
    }

    // Setup chroma card
    public void Setup(ChromaData chroma)
    {
        switch(chroma.type)
        {
            case ChromaType.InverseExplosions:
                ExplosiveHandler.inverse = true;
                break;
            case ChromaType.StickyBullets:
                BulletHandler.stickyBullets = true;
                break;
            case ChromaType.XPHealing:
                XPHandler.active.EnableXPHealing();
                break;
            case ChromaType.EnergyBullets:
                BulletHandler.energyBullets = true;
                Deck.player.pierces += 10;
                Deck.player.stunLength += 0.2f;
                break;
            case ChromaType.Warrior:
                Ship.warrior = true;
                break;
            case ChromaType.Champion:
                Ship.champion = true;
                break;
            case ChromaType.Lasers:
                Ship.lasers = true;
                break;
        }      
    }
}
