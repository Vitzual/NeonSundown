using Michsky.UI.ModernUIPack;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : Weapon
{
    // Controller associated with the player
    private Controller controller;

    // Player model and data
    public PlayerData playerData;
    public SpriteRenderer border;
    public SpriteRenderer fill;

    // Barrel location
    public Transform barrel;
    public Transform model;

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

    // Default card
    public WeaponData defaultWeapon;
    private float weaponCooldown;
    private float regenCooldown;
    
    // On start, setup
    public void Start()
    {
        // Setup the player model
        border.sprite = playerData.border;
        fill.sprite = playerData.fill;
        border.material = playerData.borderMaterial;
        fill.color = playerData.fillColor;

        // Set positioning of models
        border.transform.localPosition = playerData.barrelPosition;
        fill.transform.localPosition = playerData.modelOffset;
        border.transform.localScale = playerData.playerSize;
        fill.transform.localScale = playerData.playerSize;

        // Get controller instance
        controller = GetComponent<Controller>();
        controller.moveSpeed = playerData.playerSpeed;
        controller.dashSpeed = playerData.dashSpeed;
        controller.dashSpeed = playerData.dashSpeed;
        controller.canRotate = playerData.playerControlledRotation;

        // Setup base stuff
        level = 0;
        xp = 0;
        health = playerData.startingHealth;
        maxHealth = health;
        regenCooldown = playerData.regenRate;

        // Set weapon info
        defaultWeapon = playerData;
        weapon = playerData;
        Setup(defaultWeapon);

        // Set starting rankup cost
        if (levels.Count > 0)
            rankup = levels[0];
        else rankup = 25;

        // Update UI element
        levelText.text = "LEVEL " + level;
        xpText.text = Mathf.Round(xp) + " / " + Mathf.Round(rankup);
        xpBar.currentPercent = (float)xp / rankup * 100;
        xpBar.UpdateUI();
    }

    // Update method 
    public void Update()
    {
        // Check if deck is open
        if (Dealer.isOpen) return;

        // Check if LMB input detected
        if (Input.GetKey(Keybinds.shoot) && playerData.canFire) Use();
        if (Input.GetKey(Keybinds.debug)) Dealer.active.OpenDealer();
        if (weaponCooldown > 0) weaponCooldown -= Time.deltaTime;

        // If can regen, regenerate
        if (playerData.canRegen)
        {
            if (regenCooldown > 0)
            {
                regenCooldown -= Time.deltaTime;
            }
            else if (health < maxHealth)
            {
                regenCooldown = playerData.regenRate;
                Heal(0.1f);
            }
        }

        // If player can rotate, rotate
        if (!playerData.playerControlledRotation)
            model.Rotate(Vector3.forward, weapon.rotateSpeed * Time.deltaTime);
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

            // Upgrades the speed 
            case Stat.MoveSpeed:

                // Upgrade speed
                controller.moveSpeed += 2.5f;
                controller.dashSpeed += 2.5f;
                //controller.dashTimer += 0.25f;
                break;

            // Upgrades the speed 
            case Stat.DashSpeed:

                // Upgrade speed
                controller.dashSpeed += 2.5f;
                //controller.dashTimer += 0.25f;
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
            if (levels.Count <= level) rankup = (int)(rankup * rankupMultiplier);
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
            BulletHandler.active.CreateBullet(this, defaultWeapon, barrel.position, 
                model.rotation, defaultWeapon.bullets, true);
            weaponCooldown = cooldown;
        }
    }

    // Heal amount
    public void Heal(float amount)
    {
        // Update health
        health += amount;
        if (health > maxHealth)
            health = maxHealth;
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
        LeanTween.reset();
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
