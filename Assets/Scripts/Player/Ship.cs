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

    // Ship specific stats
    private float regenAmount;
    private float firerateMulti;

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

        // Setup ship specific variables
        regenAmount = shipData.regenAmount;

        // Setup ship weapon specific variables
        if (shipData.weapon != null) 
        {
            // Set firerate multiplier
            firerateMulti = shipData.weapon.cooldown / 10;
            Debug.Log("Set firerate multi to " + firerateMulti);
        }

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

        // Setup starting cards
        Deck.active.SetupStartingCards();

        // Setup any attached modules
        SetupModules();
    }

    // Update method 
    public void Update()
    {
        // Check if deck is open
        if (Dealer.isOpen) return;

        // Check if LMB input detected
        if (Input.GetKey(Keybinds.shoot) && shipData.canFire) Use();
        // if (Input.GetKey(Keybinds.debug)) Dealer.active.OpenDealer();
        if (shipCooldown > 0) shipCooldown -= Time.deltaTime;

        // If can regen, regenerate
        if (shipData.canRegen)
        {
            if (regenCooldown > 0)
                regenCooldown -= Time.deltaTime;

            else if (health < maxHealth)
            {
                regenCooldown = 1f;
                Heal(regenAmount);
            }
        }

        // If player can rotate, rotate
        if (!shipData.playerControlledRotation)
            model.Rotate(Vector3.forward, weapon.rotateSpeed * Time.deltaTime);
    }

    // Update stat
    // THIS METHOD IS GOING TO BE REDONE
    public void UpdateStat(Stat stat, float amount, bool multiply = false)
    {
        switch(stat)
        {
            // Upgrades the health
            case Stat.Health:

                // Upgrade health
                if (multiply)
                {
                    float newHealth = maxHealth * amount;
                    float change = maxHealth - newHealth;
                    maxHealth *= newHealth;
                    health += change;
                }
                else
                {
                    maxHealth += amount;
                    health += amount;
                }

                UpdateHealth();
                break;

            // Upgrades the view distance
            case Stat.View:
                
                // Upgrade the view distance
                if (multiply) cam.orthographicSize *= amount;
                else cam.orthographicSize += amount;
                break;

            // Upgrades the speed 
            case Stat.MoveSpeed:

                // Upgrade speed
                if (multiply)
                {
                    controller.moveSpeed *= amount;
                    controller.dashSpeed *= amount;
                }
                else
                {
                    controller.moveSpeed += amount;
                    controller.dashSpeed += amount;
                }
                break;

            // Upgrades the speed 
            case Stat.DashSpeed:

                // Upgrade speed
                if (multiply) controller.dashSpeed *= amount;
                else controller.dashSpeed += amount;
                break;

            // Upgrades the damage 
            case Stat.Damage:

                // Upgrade damage output
                if (multiply) damage *= amount;
                else damage += amount;
                break;

            // Increases firerate 
            case Stat.Cooldown:

                // Upgrade firerate
                if (multiply) cooldown *= amount;
                else cooldown -= amount;
                if (cooldown < 0.05f)
                    cooldown = 0.05f;
                break;

            // Increases bullets
            case Stat.Bullets:

                // Upgrade bullets
                bullets += amount;
                break;

            // Increases piercing rounds
            case Stat.Pierces:

                // Upgrade piercing
                pierces += amount;
                break;

            // Increases bullet lifetime
            case Stat.Lifetime:

                // Upgrade lfietime
                if (multiply) lifetime *= amount;
                else lifetime += amount;
                break;

            // Increases accuracy
            case Stat.Bloom:

                // Increase accuracy
                if (multiply) bloom *= amount;
                else bloom -= amount;
                if (bloom < 0f)
                    bloom = 0f;
                break;

            // Increase XP gain
            case Stat.XPGain:

                // Increase xp multiplier
                if (multiply) xpMultiplier *= amount;
                else xpMultiplier += amount;
                break;

            // Increase XP range
            case Stat.XPRange:

                // Increases XP range
                if (multiply) xpRange.radius *= amount;
                else xpRange.radius += amount;
                break;

            // Increase regen rate
            case Stat.Regen:

                // Increases regen rate
                if (multiply) regenAmount *= amount;
                else regenAmount += amount;
                break;
        }
    }

    // Returns a stat
    public float GetStat(Stat stat)
    {
        switch (stat)
        {
            // Upgrades the health
            case Stat.Health:

                // Upgrade health
                return health;

            // Upgrades the view distance
            case Stat.View:

                // Upgrade the view distance
                return cam.orthographicSize;

            // Upgrades the speed 
            case Stat.MoveSpeed:

                // Upgrade speed
                return controller.moveSpeed;

            // Upgrades the speed 
            case Stat.DashSpeed:

                // Upgrade speed
                return controller.dashSpeed;

            // Upgrades the damage 
            case Stat.Damage:

                // Upgrade damage output
                return damage;

            // Increases firerate 
            case Stat.Cooldown:

                // Upgrade firerate
                return cooldown;

            // Increases bullets
            case Stat.Bullets:

                // Upgrade bullets
                return bullets;

            // Increases piercing rounds
            case Stat.Pierces:

                // Upgrade piercing
                return pierces;

            // Increases bullet lifetime
            case Stat.Lifetime:

                // Upgrade lfietime
                return lifetime;

            // Increases accuracy
            case Stat.Bloom:

                // Increase accuracy
                return bloom;

            // Increase XP gain
            case Stat.XPGain:

                // Increase xp multiplier
                return xpMultiplier;

            // Increase XP range
            case Stat.XPRange:

                // Increases XP range
                return xpRange.radius;

            // Increase regen rate
            case Stat.Regen:

                // Increases regen rate
                return regenAmount;

            // Default case
            default:
                return 0;
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
                model.rotation, (int)bullets, shipData.weapon.material, true);
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

        // Get the other enemy component 
        Bullet bullet = collision.collider.GetComponent<Bullet>();

        // If is bullet, invoke on hit method
        if (bullet != null)
        {
            Damage(bullet.GetDamage());
            bullet.Destroy();
        }
    }

    // Sets up any attached power modules
    public void SetupModules()
    {
        // Iterate through all modules
        foreach (KeyValuePair<Stat, float> module in Gamemode.moduleEffects)
        {
            Debug.Log("Setting up module " + module.Key.ToString() + " with value " + module.Value);
            UpdateStat(module.Key, module.Value, true);
        }
        Gamemode.moduleEffects = new Dictionary<Stat, float>();
    }
}
