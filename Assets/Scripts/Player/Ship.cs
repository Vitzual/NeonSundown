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
    private Secondary secondary;
    public SpriteRenderer border;
    public SpriteRenderer fill;

    // Default player models
    public Material defaultGlow;
    public Color defaultColor;

    // Barrel location
    public Transform barrel;
    public Transform model;

    // Health amount
    private static float health;
    private static float maxHealth;
    public ProgressBar healthBar;
    public CanvasGroup healthCanvas;

    // XP amount
    public List<float> levels;
    private float xp = 0;
    private float rankup = 50;
    private float xpMultiplier = 1;
    private float enemyDamage = 1;
    public float rankupMultiplier;
    public ProgressBar xpBar;
    public CircleCollider2D xpRange;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;

    // Ship specific stats
    private float regenAmount;

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
        {
            Events.active.onSetupShip += Setup;
            Events.active.onShipColoringChange += SetupShipColoring;
            Events.active.onSecondarySet += SetSecondary;
        }
        else if (shipData != null) Setup(shipData);
    }

    // On start, setup
    public void Setup(ShipData data)
    {
        // Setup the ship data
        shipData = data;
        weapon = data.weapon;
        SetupShipModel();
        SetupShipColoring(Settings.shipColoring);

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

        // Set weapon variables
        damage = shipData.weapon.damage;
        cooldown = shipData.weapon.cooldown;
        moveSpeed = shipData.weapon.moveSpeed;
        range = shipData.weapon.range;
        bloom = shipData.weapon.bloom;
        pierces = shipData.weapon.pierces;
        bullets = shipData.weapon.bullets;
        lifetime = shipData.weapon.lifetime;
        knockback = shipData.weapon.knockback;
        splitshots = 0;

        // Set starting rankup cost
        if (levels.Count > 0)
            rankup = levels[0];
        else rankup = 25;

        // Update UI element
        levelText.text = "LEVEL " + level;
        xpText.text = Mathf.Round(xp) + " / " + Mathf.Round(rankup);
        xpBar.currentPercent = (float)xp / rankup * 100;
        xpBar.UpdateUI();

        // Setup any attached modules
        SetupModules();
    }

    // Setup ship model
    public void SetupShipModel()
    {
        // Set border and fill models
        border.sprite = shipData.border;
        fill.sprite = shipData.fill;

        // Set positioning of models
        border.transform.localPosition = shipData.barrelPosition;
        fill.transform.localPosition = shipData.modelOffset;
        border.transform.localScale = shipData.playerSize;
        fill.transform.localScale = shipData.playerSize;
    }

    // Setup ship coloring
    public void SetupShipColoring(bool useColoring)
    {
        if (useColoring)
        {
            border.material = shipData.borderMaterial;
            fill.color = shipData.fillColor;
        }
        else
        {
            border.material = defaultGlow;
            fill.color = defaultColor;
        }
    }

    // Update method 
    public void Update()
    {
        // Check if deck is open
        if (Dealer.isOpen) return;

        // Check if LMB input detected
        if (Input.GetKey(Keybinds.primary) && shipData.canFire) Use();
        if (shipCooldown > 0) shipCooldown -= Time.deltaTime;

        // If can regen, regenerate
        if (regenAmount > 0)
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

    // Set the secondary weapon
    public void SetSecondary(SecondaryData secondary)
    {
        // Remove old instance
        if (this.secondary != null)
        {
            Deck.active.TakeCard(this.secondary.data);
            this.secondary.Destroy();
        }

        // Create new instance
        this.secondary = Instantiate(secondary.obj, transform.position, Quaternion.identity);
        if (secondary.setShipAsParent) this.secondary.transform.SetParent(transform);
        this.secondary.Setup(this, secondary);
    }

    // Add XP
    public void AddXP(float amount)
    {
        // Add the XP amount
        float addAmount = amount * xpMultiplier;
        Levels.AddXP(addAmount);
        xp += addAmount;

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
            if (BulletHandler.energyBullets)
            {
                if (Settings.shipColoring) BulletHandler.active.CreateEnergyBullet(this, shipData.weapon, barrel.position,
                    model.rotation, (int)bullets, shipData.weapon.material, true, explosiveRounds, true);
                else BulletHandler.active.CreateEnergyBullet(this, shipData.weapon, barrel.position,
                    model.rotation, (int)bullets, defaultGlow, true, explosiveRounds, true);
            }
            else
            {
                if (Settings.shipColoring) BulletHandler.active.CreateBullet(this, shipData.weapon, barrel.position,
                    model.rotation, (int)bullets, shipData.weapon.material, true, explosiveRounds, true);
                else BulletHandler.active.CreateBullet(this, shipData.weapon, barrel.position,
                    model.rotation, (int)bullets, defaultGlow, true, explosiveRounds, true);
            }
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
        // Set enemy damage
        damage = damage * enemyDamage;

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
        // Check if modules exist
        if (Gamemode.modules == null) return;

        // Iterate through all modules
        foreach (KeyValuePair<int, ModuleData> module in Gamemode.modules)
        {
            // Check if module is empty
            if (module.Value == null) continue;

            // Setup the module
            ModuleData newModule = module.Value;
            Debug.Log("Setting up module " + newModule.name + " with value " + newModule.value);
            if (module.Value.multi) Deck.AddMultiplier(newModule.stat, newModule.value);
            else Deck.AddAddition(newModule.stat, newModule.value);
            UpdateStat(newModule.stat);
        }
    }

    public static float GetHealth() { return health; }

    // Update stat
    public override void UpdateStat(Stat stat)
    {
        switch(stat)
        {
            // Upgrades the health
            case Stat.Health:
                float oldHealth = maxHealth;
                maxHealth = Deck.CalculateStat(stat, shipData.startingHealth);
                health += maxHealth - oldHealth;
                UpdateHealth();
                break;

            // Upgrades the view distance
            case Stat.View:
                cam.orthographicSize = Deck.CalculateStat(stat, 45);
                break;

            // Upgrades the speed 
            case Stat.MoveSpeed:
                controller.moveSpeed = Deck.CalculateStat(stat, shipData.playerSpeed);
                controller.dashSpeed = Deck.CalculateStat(stat, shipData.dashSpeed);
                break;

            // Upgrades the speed 
            case Stat.DashSpeed:
                controller.dashSpeed = Deck.CalculateStat(stat, shipData.dashSpeed);
                break;

            // Upgrades the damage 
            case Stat.Damage:
                damage = Deck.CalculateStat(stat, weapon.damage);
                break;

            // Increases firerate 
            case Stat.Cooldown:
                cooldown = Mathf.Clamp(Deck.CalculateStat(stat, 
                    weapon.cooldown), 0.05f, Mathf.Infinity);
                break;

            // Increases bullets
            case Stat.Bullets:
                bullets = Deck.CalculateStat(stat, weapon.bullets);
                break;

            // Increases piercing rounds
            case Stat.Pierces:
                pierces = Deck.CalculateStat(stat, weapon.pierces);
                break;

            // Increases bullet lifetime
            case Stat.Lifetime:
                lifetime = Deck.CalculateStat(stat, weapon.lifetime);
                break;

            // Increases accuracy
            case Stat.Spread:
                bloom = Mathf.Clamp(Deck.CalculateStat(stat,
                    weapon.bloom), 0f, Mathf.Infinity);
                break;

            // Increase XP gain
            case Stat.XPGain:
                xpMultiplier = Deck.CalculateStat(stat, 1);
                break;

            // Increase XP range
            case Stat.XPRange:
                xpRange.radius = Deck.CalculateStat(stat, 15);
                break;

            // Increase regen rate
            case Stat.Regen:
                regenAmount = Deck.CalculateStat(stat, shipData.regenAmount);
                break;

            // Increase regen rate
            case Stat.Knockback:
                knockback = Deck.CalculateStat(stat, weapon.knockback);
                break;

            // Increase splitshots
            case Stat.Splitshot:
                splitshots = Deck.CalculateStat(stat, 0);
                break;

            // Increase explosive rounds
            case Stat.Explosive:
                explosiveRounds = true;
                break;

            // Increase explosive rounds
            case Stat.EnemyDmg:
                enemyDamage = Deck.CalculateStat(stat, 1);
                break;

            // Increases bullet size
            case Stat.BulletSize:
                BulletHandler.bulletSize = Deck.CalculateStat(stat, 1);
                break;

            // Increases bullet size
            case Stat.StunLength:
                stunLength = Deck.CalculateStat(stat, weapon.stun);
                break;

            // Increases bullet size
            case Stat.Criticals:
                DamageHandler.critChance = Deck.CalculateStat(stat, 0.1f);
                break;

            // Increases bullet size
            case Stat.Syphon:
                EnemyHandler.syphon = Deck.CalculateStat(stat, 0f);
                break;
        }
    }

    // Returns a stat
    public override float GetStat(Stat stat)
    {
        switch (stat)
        {
            // Upgrades the health
            case Stat.Health:
                return health;

            // Upgrades the view distance
            case Stat.View:
                return cam.orthographicSize;

            // Upgrades the speed 
            case Stat.MoveSpeed:
                return controller.moveSpeed;

            // Upgrades the speed 
            case Stat.DashSpeed:
                return controller.dashSpeed;

            // Upgrades the damage 
            case Stat.Damage:
                return damage;

            // Increases firerate 
            case Stat.Cooldown:
                return cooldown;

            // Increases bullets
            case Stat.Bullets:
                return bullets;

            // Increases piercing rounds
            case Stat.Pierces:
                return pierces;

            // Increases bullet lifetime
            case Stat.Lifetime:
                return lifetime;

            // Increases accuracy
            case Stat.Spread:
                return bloom;

            // Increase XP gain
            case Stat.XPGain:
                return xpMultiplier;

            // Increase XP range
            case Stat.XPRange:
                return xpRange.radius;

            // Increase regen rate
            case Stat.Regen:
                return regenAmount;

            // Increase regen rate
            case Stat.Knockback:
                return knockback;

            // Get splitshots
            case Stat.Splitshot:
                return splitshots;

            // Get explosive rounds
            case Stat.Explosive:
                if (explosiveRounds) return 1;
                else return 0;

            // Get splitshots
            case Stat.EnemyDmg:
                return enemyDamage;

            // Get bullet size
            case Stat.BulletSize:
                return BulletHandler.bulletSize;

            // Get bullet size
            case Stat.StunLength:
                return stunLength;

            // Crit thing
            case Stat.Criticals:
                return DamageHandler.critChance;

            // Syphon thing
            case Stat.Syphon:
                return EnemyHandler.syphon;

            // Default case
            default:
                return 0;
        }
    }

    // Returns a stat
    public override float GetDefaultStat(Stat stat)
    {
        switch (stat)
        {
            // Upgrades the health
            case Stat.Health:
                return shipData.startingHealth;

            // Upgrades the view distance
            case Stat.View:
                return 45f;

            // Upgrades the speed 
            case Stat.MoveSpeed:
                return shipData.playerSpeed;

            // Upgrades the speed 
            case Stat.DashSpeed:
                return shipData.dashSpeed;

            // Upgrades the damage 
            case Stat.Damage:
                return weapon.damage;

            // Increases firerate 
            case Stat.Cooldown:
                return weapon.cooldown;

            // Increases bullets
            case Stat.Bullets:
                return weapon.bullets;

            // Increases piercing rounds
            case Stat.Pierces:
                return weapon.pierces;

            // Increases bullet lifetime
            case Stat.Lifetime:
                return weapon.lifetime;

            // Increases accuracy
            case Stat.Spread:
                return weapon.bloom;

            // Increase XP gain
            case Stat.XPGain:
                return 1f;

            // Increase XP range
            case Stat.XPRange:
                return 15f;

            // Increase regen rate
            case Stat.Regen:
                return shipData.regenAmount;

            // Increase regen rate
            case Stat.Knockback:
                return weapon.knockback;

            // Increase regen rate
            case Stat.Splitshot:
                return 0;

            // Increase regen rate
            case Stat.Explosive:
                return 0;

            // Increase regen rate
            case Stat.EnemyDmg:
                return 1;

            // Increase thing
            case Stat.BulletSize:
                return 1;

            // Increase thing
            case Stat.StunLength:
                return weapon.stun;

            // Crit thing
            case Stat.Criticals:
                return 0.1f;

            // Crit thing
            case Stat.Syphon:
                return 0f;

            // Default case
            default:
                return 0;
        }
    }

    // Returns a secondary instance
    public Secondary GetSecondary() { return secondary; }
}
