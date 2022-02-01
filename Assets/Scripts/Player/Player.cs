using Michsky.UI.ModernUIPack;
using UnityEngine;

public class Player : Weapon
{
    // Health amount
    private float health;


    // XP amount
    private float xp = 0;
    private float rankup = 50;
    public float rankupMultiplier;
    public ProgressBar xpBar;

    // Barrel location
    public Transform barrel;
    public Transform rotator;

    // Default card
    public WeaponData defaultWeapon;
    private float weaponCooldown;

    // On start, setup
    public void Start()
    {
        weapon = defaultWeapon;
        Setup(defaultWeapon);
    }

    // Update method 
    public void Update()
    {
        // Check if deck is open
        if (Dealer.isOpen) return;

        // Check if LMB input detected
        if (Input.GetKey(Keybinds.shoot)) Use();
        if (Input.GetKey(Keybinds.debug)) Dealer.active.OpenDealer();
        if (weaponCooldown > 0) weaponCooldown -= Time.deltaTime;
    }

    // Add XP
    public void AddXP(int amount)
    {
        // Add the XP amount
        xp += Deck.CalculateStat(Stat.XPGain, amount);

        // Check if XP over rankup
        if (xp >= rankup)
        {
            xp = 0;
            rankup = (int)(rankup * rankupMultiplier);
            Dealer.active.OpenDealer();
        }

        // Set XP bar
        xpBar.currentPercent = (float)xp / rankup * 100;
        xpBar.UpdateUI();
    }

    // Shoot method
    public override void Use()
    {
        if (weaponCooldown <= 0)
        {
            // Create bullet
            BulletHandler.active.CreateBullet(this, defaultWeapon, barrel.position, rotator.rotation);
            weaponCooldown = cooldown;
        }
    }
}
