using Michsky.UI.ModernUIPack;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ship : Weapon
{
    // Auto collect XP flag
    public static bool warrior = false;
    public static bool champion = false;
    public static bool lasers = false;

    // Custom cursor textures
    public Texture2D normalMouse;
    public Texture2D crosshairMouse;
    public Vector2 crosshairOffset;
    private bool crosshairOn = true;

    // Controller associated with the player
    private Controller controller;
    public GameObject autoFireObj;
    private bool autoFire = false;
    public bool _lasers = false;
    private static bool lowHealth = false;

    // Player model and data
    public ShipData shipData;
    private Secondary secondary;
    public SpriteRenderer border;
    public SpriteRenderer fill;

    // Default player models
    public Material defaultGlow;
    public Color defaultColor;
    public ParticleSystem deathParticle;
    public AudioClip deathSound;

    // Barrel location
    public Transform barrel;
    public Transform model;

    // Health amount
    private static float health;
    private static float maxHealth;
    public ProgressBar _healthBar;
    public CanvasGroup _healthCanvas;
    private static ProgressBar healthBar;
    private static CanvasGroup healthCanvas;
    public AudioClip damageSound;
    public AudioClip warriorSound;
    public AudioClip laserSound;
    public AudioClip nearDeathSound;
    public AudioClip buckshotSound;
    public AudioSource deathMusic; // i know im bad putting this here

    // XP amount
    public List<float> levels;
    private float xp = 0;
    private float rankup = 50;
    private float xpMultiplier = 1;
    private float enemyDamage = 1;
    private int buckshots = 0;
    private int buckshotCountdown = 4;
    public float rankupMultiplier;
    public ProgressBar xpBar;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;
    public bool droneShip = false;

    // Ship specific stats
    private float regenAmount;

    // View distance
    public Camera cam;

    // Default card
    private float shipCooldown;
    private float regenCooldown;

    // List of drones
    private List<Drone> drones;

    // Debug car
    public CardData debugCard;

    // Subscribe to setup event
    public void Start()
    {
        // Reset effects always
        Effects.ToggleMainGlitchEffect(false);

        // Set crosshair cursor
        Cursor.SetCursor(crosshairMouse, crosshairOffset, CursorMode.Auto);

        lowHealth = false;
        warrior = false;
        champion = false;
        lasers = _lasers;
        healthBar = _healthBar;
        healthCanvas = _healthCanvas;

        Events.active.onUpdateShowHP += UpdateShowHP;

        if (Gamemode.shipData != null)
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
        if (shipData.weapon != null)
        {
            damage = shipData.weapon.damage;
            cooldown = shipData.weapon.cooldown;
            moveSpeed = shipData.weapon.moveSpeed;
            range = shipData.weapon.range;
            bloom = shipData.weapon.bloom;
            pierces = shipData.weapon.pierces;
            bullets = shipData.weapon.bullets;
            lifetime = shipData.weapon.lifetime;
            knockback = shipData.weapon.knockback;
            size = shipData.weapon.bulletSize;
            rotateSpeed = shipData.weapon.rotateSpeed;
        }
        splitshots = 0;

        // Set starting rankup cost
        if (levels.Count > 0)
            rankup = levels[0];
        else rankup = 25;

        // Update UI element
        levelText.text = "LEVEL " + level;
        xpText.text = Mathf.Round(xp) + " / " + Mathf.Round(rankup) + "xp";
        xpBar.currentPercent = (float)xp / rankup * 100;
        xpBar.UpdateUI();

        // Setup any attached modules
        SetupModules();

        // Check if ship is drone ship
        if (shipData.droneShip)
        {
            drones = new List<Drone>();

            for (int i = 0; i < shipData.droneAmount; i += 1)
            {
                Drone newDrone = Instantiate(shipData.drone.obj, transform.position, 
                    transform.rotation).GetComponent<Drone>();
                newDrone.damage = shipData.droneDamage;
                newDrone.movementSpeed = shipData.droneMoveSpeed;
                newDrone.rotationSpeed = shipData.droneRotateSpeed;
                newDrone.Setup(this, shipData.drone);
                drones.Add(newDrone);
            }
        }

        // Add debug card 
        if (debugCard != null)
            Deck.active.AddCard(debugCard);
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
            fill.color = shipData.mainColor;
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
        if (Dealer.isOpen)
        {
            if (crosshairOn)
            {
                Cursor.SetCursor(normalMouse, Vector2.zero, CursorMode.Auto);
                crosshairOn = false;
            }
            return;
        }
        else if (!crosshairOn)
        {
            Cursor.SetCursor(crosshairMouse, crosshairOffset, CursorMode.Auto);
            crosshairOn = true;
        }

        // Check if space pressed for auto fire
        if (Input.GetKeyDown(Keybinds.autofire))
        {
            autoFire = !autoFire;
            autoFireObj.SetActive(autoFire);
        }

        // Check if LMB input detected
        if ((autoFire || Input.GetKey(Keybinds.primary) || Input.GetAxis("Primary") > 0.5) && shipData.canFire) Use();
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
            model.Rotate(Vector3.forward, 60 * Time.deltaTime);
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
            if (levels.Count <= level)
            {
                rankup = (int)(rankup * rankupMultiplier);
                if (rankup > 80000) rankup = 80000;
            }
            else rankup = levels[level];

            // Set text
            levelText.text = "LEVEL " + level;
            Dealer.active.OpenDealer();
        }

        // Set XP bar
        xpText.text = Mathf.Round(xp) + " / " + Mathf.Round(rankup) + "xp";
        xpBar.currentPercent = (float)xp / rankup * 100;
        xpBar.UpdateUI();
    }

    // Shoot method
    public override void Use()
    {
        if (shipCooldown <= 0)
        {
            // Bullets to fire variable
            int bulletsToFire = (int)bullets;
            RuntimeStats.bulletsFired += bulletsToFire;
            float bloom = weapon.bloom;

            // Add bullet fired
            if (buckshots > 0)
            {
                if (buckshotCountdown != 0)
                {
                    buckshotCountdown -= 1;
                }
                else
                {
                    bulletsToFire += buckshots;
                    RuntimeStats.bulletsFired += buckshots;
                    buckshotCountdown = 4;
                    bloom *= 2;

                    AudioPlayer.Play(buckshotSound, true, 0.9f, 1.1f, true, 0.5f);
                }
            }

            // Create bullet
            if (lasers)
            {
                if (Settings.shipColoring) BulletHandler.active.CreateLaserBullet(this, shipData.weapon, shipData.weapon.material,
                    barrel, size, 100f, bulletsToFire, explosiveRounds);
                else BulletHandler.active.CreateLaserBullet(this, shipData.weapon, defaultGlow,
                    barrel, size, 100f, bulletsToFire, explosiveRounds);
            }
            else if (BulletHandler.energyBullets)
            {
                if (Settings.shipColoring) BulletHandler.active.CreateEnergyBullet(this, shipData.weapon, barrel.position,
                    model.rotation, bulletsToFire, bloom, size, shipData.weapon.material, explosiveRounds, false);
                else BulletHandler.active.CreateEnergyBullet(this, shipData.weapon, barrel.position,
                    model.rotation, bulletsToFire, bloom, size, defaultGlow, explosiveRounds, false);
            }
            else
            {
                if (Settings.shipColoring) BulletHandler.active.CreateBullet(this, shipData.weapon, barrel.position,
                    model.rotation, bulletsToFire, bloom, size, shipData.weapon.material, explosiveRounds, true);
                else BulletHandler.active.CreateBullet(this, shipData.weapon, barrel.position,
                    model.rotation, bulletsToFire, bloom, size, defaultGlow, explosiveRounds, true);
            }
            shipCooldown = cooldown;
        }
    }

    // Heal amount
    public static void Heal(float amount)
    {
        // Update health
        health += amount;
        if (health > maxHealth)
            health = maxHealth;
        UpdateHealth();

        // Check if low health
        if (lowHealth && health / maxHealth > 0.25f)
        {
            MusicPlayer.SetPitch(1f);
            lowHealth = false;
        }
    }

    // Damage method
    public void Damage(float damage)
    {
        // Check if warrior active
        if (champion && controller.isDashing)
        {
            // Play audio clip
            ExplosiveHandler.CreateKnockback(transform.position, 20f, -2000f, -2500f, 25);
            AudioPlayer.Play(warriorSound, true, 0.8f, 1.2f, false, 0.8f);
            return;
        }
        else if (warrior && controller.isDashing)
        {
            // Play audio clip
            AudioPlayer.Play(warriorSound, true, 0.8f, 1.2f, false, 0.8f);
            return;
        }

        // Add runtime stat
        RuntimeStats.damageTaken += damage;

        // Set enemy damage
        damage = damage * enemyDamage;

        // Update health
        health -= damage;
        if (health <= 0)
        {
            // Unleash pulse
            ExplosiveHandler.CreateKnockback(transform.position, 100f, -2000f, -2500f);

            // Disable health bar
            LeanTween.cancel(healthCanvas.gameObject);
            healthCanvas.alpha = 0f;
            
            // Kill the player
            Kill();
        }
        else
        {
            // Check if under 25%
            if (!lowHealth && health / maxHealth <= 0.25f)
            {
                // Unleash pulse
                ExplosiveHandler.CreateKnockback(transform.position, 200f, -3000f, -3500f);
                AudioPlayer.Play(nearDeathSound, false, 1f, 1f, true, 1f);
                MusicPlayer.SetPitch(1.1f);
                lowHealth = true;
            }
            else if (lowHealth && health / maxHealth > 0.25f)
            {
                MusicPlayer.SetPitch(1f);
                lowHealth = false;
            }

            // Unleash pulse
            ExplosiveHandler.CreateKnockback(transform.position, 20f, -1000f, -1500f);

            // Play hurt sound
            AudioPlayer.PlayHurtSound();

            // Update health UI bar
            UpdateHealth();
        }
    }

    public void UpdateShowHP(bool toggle)
    {
        if (!GameOverScreen.isActive)
            LeanTween.cancel(healthCanvas.gameObject);

        if (toggle) healthCanvas.alpha = 1f;
        else healthCanvas.alpha = 0f;
    }

    private static void UpdateHealth()
    {
        // Update health UI bar
        healthBar.currentPercent = (float)health / maxHealth * 100;
        healthBar.UpdateUI();

        // Show bar for short period of time
        if (!GameOverScreen.isActive && !Settings.alwaysShowHP)
        {
            healthCanvas.alpha = 1f;
            LeanTween.cancel(healthCanvas.gameObject);
            LeanTween.alphaCanvas(healthCanvas, 0f, 0.5f).setDelay(3f);
        }
    }

    // Kill the player
    public void Kill()
    {
        // Play death effect
        ParticleSystem newParticle = Instantiate(deathParticle, transform.position, Quaternion.identity);
        ParticleSystemRenderer renderer = newParticle.GetComponent<ParticleSystemRenderer>();

        // Set death material
        renderer.material = border.material;
        border.enabled = false;
        fill.enabled = false;

        // Open game over screen
        Events.active.ShipDestroyed();

        // I'll make this event later
        Effects.ToggleMainGlitchEffect(true);
        deathMusic.Play();
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
            int amount = SaveSystem.GetModuleAmount(newModule.InternalID);
            if (amount >= newModule.values.Count)
            {
                Debug.Log("Module amount exceeds values setup!");
                amount = newModule.values.Count - 1;
            }
            float value = newModule.values[amount];
            Debug.Log("Setting up module " + newModule.name + " with value " + value);
            if (module.Value.multi) Deck.AddMultiplier(newModule.stat, value);
            else Deck.AddAddition(newModule.stat, value);
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
                if (drones != null)
                {
                    float droneDamage = Deck.CalculateStat(stat, shipData.droneDamage);
                    foreach (Drone drone in drones)
                    {
                        if (drone != null)
                            drone.damage = droneDamage;
                    }
                }

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
                BulletHandler.projectileSize = Deck.CalculateStat(stat, 1);
                size = weapon.bulletSize + BulletHandler.projectileSize - 1;
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

            // Increases bullet size
            case Stat.Buckshot:
                buckshots = (int)Deck.CalculateStat(stat, 0f);
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
                return BulletHandler.projectileSize;

            // Get bullet size
            case Stat.StunLength:
                return stunLength;

            // Crit thing
            case Stat.Criticals:
                return DamageHandler.critChance;

            // Syphon thing
            case Stat.Syphon:
                return EnemyHandler.syphon;

            // Syphon thing
            case Stat.Buckshot:
                return buckshots;

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
            case Stat.AutoCollect:
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

            // Crit thing
            case Stat.Buckshot:
                return 0;

            // Default case
            default:
                return 0;
        }
    }

    // Returns a secondary instance
    public Secondary GetSecondary() { return secondary; }
}
