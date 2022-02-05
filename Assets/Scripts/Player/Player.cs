using Michsky.UI.ModernUIPack;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : Weapon
{
    // Health amount
    private float health;
    private float maxHealth;
    public ProgressBar healthBar;
    public CanvasGroup healthCanvas;
    public CanvasGroup gameOverScreen;

    // XP amount
    public List<float> levels;
    private float xp = 0;
    private float rankup = 50;
    public float rankupMultiplier;
    public ProgressBar xpBar;
    public CircleCollider2D xpRange;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;

    // View distance
    public Camera cam;

    // Barrel location
    public Transform barrel;
    public Transform rotator;

    // Default card
    public WeaponData defaultWeapon;
    private float weaponCooldown;
    
    // On start, setup
    public void Start()
    {
        // Setup base stuff
        level = 0;
        xp = 0;
        health = 10;
        maxHealth = health;

        // Set starting rankup cost
        if (levels.Count > 0)
            rankup = levels[0];
        else rankup = 25;

        // Update UI element
        levelText.text = "LEVEL " + level;
        xpText.text = Mathf.Round(xp) + " / " + Mathf.Round(rankup);
        xpBar.currentPercent = (float)xp / rankup * 100;
        xpBar.UpdateUI();

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

    // Update stat
    public override void UpdateStat(Stat stat)
    {
        switch(stat)
        {
            // Upgrades the health
            case Stat.Health:

                // Upgrade health
                maxHealth += 5;
                health += 5;

                UpdateHealth();
                break;

            // Upgrades the view distance
            case Stat.View:

                // Upgrade the view distance
                cam.orthographicSize += 2.5f;
                break;
        }
    }

    // Add XP
    public void AddXP(int amount)
    {
        // Add the XP amount
        xp += Deck.CalculateStat(Stat.XPGain, amount);

        // Check if XP over rankup
        if (xp >= rankup)
        {
            // Increase level
            level += 1;
            xp -= rankup;
            if (levels.Count < level) rankup = (int)(rankup * rankupMultiplier);
            else rankup = levels[level];

            // Set text
            levelText.text = "LEVEL " + level;
            Dealer.active.OpenDealer();
        }

        // Set XP bar
        xpText.text = Mathf.Round(xp) + " / " + Mathf.Round(rankup);
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

    // Heal amount
    public void Health(float amount)
    {
        // Update health
        health += amount;
        UpdateHealth();
    }

    // Damage method
    public void Damage(float damage)
    {
        // Update health
        health -= damage;
        if (health <= 0) Kill();

        // Update health UI bar
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        // Update health UI bar
        healthBar.currentPercent = (float)health / maxHealth * 100;
        healthBar.UpdateUI();

        // Show bar for short period of time
        healthCanvas.alpha = 1f;
        LeanTween.alphaCanvas(healthCanvas, 0f, 0.5f).setDelay(3f);
    }

    // Kill the player
    public void Kill()
    {
        // Open game over screen
        Dealer.active.pitchDown = 0f;
        Dealer.isOpen = true;
        LeanTween.alphaCanvas(gameOverScreen, 1f, 1f);
        gameOverScreen.interactable = true;
        gameOverScreen.blocksRaycasts = true;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the other enemy component
        Enemy enemy = collision.collider.GetComponent<Enemy>();

        // If is player, invoke on hit method
        if (enemy != null)
            enemy.OnHitPlayer(this);
    }
}
