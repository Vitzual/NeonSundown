using Michsky.UI.ModernUIPack;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ship : Weapon
{
    // Controller associated with the player
    private Controller controller;

    // Player model and data
    public ShipData shipData;
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

    // XP amount
    public List<float> levels;
    private float xp = 0;
    private float rankup = 50;
    private float xpMultiplier = 1;
    public float rankupMultiplier;
    public ProgressBar xpBar;
    public CircleCollider2D xpRange;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;

    // View distance
    public Camera cam;

    // Default card
    private float shipCooldown;
    private float regenCooldown;
    private bool isDead = false;

    // Subscribe to setup event
    public void Start()
    {
        if (Gamemode.ship != null)
            Events.active.onSetupShip += Setup;
        else if (shipData != null) Setup(shipData);
    }

    // On start, setup
    public void Setup(ShipData data)
    {
        // Setup the ship data
        shipData = data;

        // Setup the player model
        border.sprite = shipData.border;
        fill.sprite = shipData.fill;
        border.material = shipData.borderMaterial;
        fill.color = shipData.fillColor;

        // Set positioning of models
        border.transform.localPosition = shipData.barrelPosition;
        fill.transform.localPosition = shipData.modelOffset;
        border.transform.localScale = shipData.playerSize;
        fill.transform.localScale = shipData.playerSize;

        // Get controller instance
        controller = GetComponent<Controller>();
        controller.moveSpeed = shipData.playerSpeed;
        controller.dashSpeed = shipData.dashSpeed;
        controller.dashSpeed = shipData.dashSpeed;
        controller.canRotate = shipData.playerControlledRotation;

        // Setup base stuff
        level = 0;
        xp = 0;
        health = shipData.startingHealth;
        maxHealth = health;
        regenCooldown = shipData.regenRate;

        // Set weapon variables
        damage = shipData.weapon.damage;
        cooldown = shipData.weapon.cooldown;
        moveSpeed = shipData.weapon.moveSpeed;
        range = shipData.weapon.range;
        bloom = shipData.weapon.bloom;
        pierces = shipData.weapon.pierces;
        bullets = shipData.weapon.bullets;
        lifetime = shipData.weapon.lifetime;

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
        if (Input.GetKey(Keybinds.shoot) && shipData.canFire) Use();
        if (Input.GetKey(Keybinds.debug)) Dealer.active.OpenDealer();
        if (shipCooldown > 0) shipCooldown -= Time.deltaTime;

        // If can regen, regenerate
        if (shipData.canRegen)
        {
            if (regenCooldown > 0)
            {
                regenCooldown -= Time.deltaTime;
            }
            else if (health < maxHealth)
            {
                regenCooldown = shipData.regenRate;
                Heal(0.1f);
            }
        }

        // If player can rotate, rotate
        if (!shipData.playerControlledRotation)
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

            // Upgrades the damage 
            case Stat.Damage:

                // Upgrade damage output
                damage += 0.5f;
                break;

            // Increases firerate 
            case Stat.Cooldown:

                // Upgrade firerate
                cooldown -= 0.01f;
                if (cooldown < 0.05f)
                    cooldown = 0.05f;
                break;

            // Increases piercing rounds
            case Stat.Pierces:

                // Upgrade piercing
                pierces += 1;
                break;

            // Increases bullet lifetime
            case Stat.Lifetime:

                // Upgrade lfietime
                lifetime += 0.1f;
                break;

            // Increases accuracy
            case Stat.Bloom:

                // Increase accuracy
                bloom -= 2.5f;
                if (bloom < 0f)
                    bloom = 0f;
                break;

            // Increase XP gain
            case Stat.XPGain:

                // Increase xp multiplier
                xpMultiplier += 0.25f;
                break;

            // Increase XP range
            case Stat.XPRange:

                // Increases XP range
                xpRange.radius += 5;
                break;
        }
    }

    // Add XP
    public void AddXP(int amount)
    {
        // Add the XP amount
        xp += amount * xpMultiplier;

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
        if (shipCooldown <= 0)
        {
            // Create bullet
            BulletHandler.active.CreateBullet(this, shipData.weapon, barrel.position, 
                model.rotation, shipData.weapon.bullets, true);
            shipCooldown = cooldown;
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
        if (!isDead)
        {
            healthCanvas.alpha = 1f;
            LeanTween.reset();
            LeanTween.alphaCanvas(healthCanvas, 0f, 0.5f).setDelay(3f);
        }
    }

    // Kill the player
    public void Kill()
    {
        // Open game over screen
        Events.active.ShipDestroyed();

        // Set is dead flag to true
        isDead = true;
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
