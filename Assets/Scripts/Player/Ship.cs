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
    
    // Controller associated with the player
    private Controller controller;
    private XPReceiver xpReceiver;
    public GameObject autoFireObj;
    private bool autoFire = false;
    public bool _lasers = false;
    private static bool lowHealth = false;

    // Player model and data
    public ShipData shipData;
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

    // Multipliers
    private float enemyDamageMultiplier = 1;
    private int buckshots = 0;
    private int buckshotCountdown = 4;
    private bool droneShip = false;

    // Ship specific stats
    private float regenAmount;

    // View distance
    public Camera cam;

    // Default card
    private float shipCooldown;
    private float regenCooldown;

    // List of drones
    private List<Drone> drones;

    // Scriptable and weapon reference
    private Dictionary<WeaponData, Weapon> weaponInstances;
    private Dictionary<HelperData, Helper> helperInstances;
    private Dictionary<SecondaryData, Secondary> secondaryInstances;
    private SecondaryData secondary;

    // Debug car
    public CardData debugCard;

    // Create new instances
    public void Awake()
    {
        // Create starting slots
        drones = new List<Drone>();
        weaponInstances = new Dictionary<WeaponData, Weapon>();
        helperInstances = new Dictionary<HelperData, Helper>();
        secondaryInstances = new Dictionary<SecondaryData, Secondary>();
        xpReceiver = GetComponent<XPReceiver>();
    }

    // Subscribe to setup event
    public void Start()
    {
        // Reset effects always
        Effects.ToggleMainGlitchEffect(false);
        Events.active.onAddSynergy += AddSynergy;

        // Reset ship flags
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
        // Subscribe to add card event
        Events.active.onAddCard += AddCard;

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
        controller.isChargeShip = shipData.chargingShip;
        
        // Check if charing ship
        if (shipData.chargingShip)
            ChromaHandler.active.Setup(ChromaType.Warrior);

        // Setup base stuff
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
            speedAffectsDamage = shipData.speedAffectsDamage;
            speedDamageMultiplier = shipData.speedDamageMultiplier;
            informOnHit = shipData.weapon.informOnHit;
        }
        splitshots = 0;

        // Setup any attached modules
        SetupModules();

        // Check if ship is drone ship
        if (shipData.droneShip)
        {
            drones = new List<Drone>();
            droneShip = true;

            for (int i = 0; i < shipData.droneAmount; i += 1)
            {
                Vector2 spawnPos = new Vector2(transform.position.x + Random.Range(-1f, 1f),
                    transform.position.y + Random.Range(-1f, 1f));
                Drone newDrone = Instantiate(shipData.drone.obj, spawnPos, transform.rotation).GetComponent<Drone>();
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
        // Check if something is open
        if (Dealer.isOpen) return;

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

        // Iterate through all weapon instances
        foreach (KeyValuePair<WeaponData, Weapon> weapon in weaponInstances) weapon.Value.Use();
        foreach (KeyValuePair<HelperData, Helper> helper in helperInstances) helper.Value.CustomUpdate();
        foreach (KeyValuePair<SecondaryData, Secondary> secondary in secondaryInstances) secondary.Value.CustomUpdate();
    }

    public override void AddCard(CardData card)
    {
        // Check card type
        if (card is StatData)
        {
            // Get weapon data
            StatData statData = (StatData)card;

            // Add card
            Debug.Log("Adding stat card " + statData.name + " to deck");
            foreach (StatValue stat in statData.stats) UpdateStat(stat.type);

            // Update all drones
            if (!card.isShipOnlyCard && drones != null)
            {
                // Check for multishot
                int multishots = 0;

                // Iterate through all drones and update their stats
                foreach (Drone drone in drones)
                {
                    foreach (StatValue stat in statData.stats)
                    {
                        drone.UpdateStat(stat.type);
                        if (multishots == 0 && stat.type == Stat.Bullets && !stat.multiply)
                            multishots = (int)stat.modifier;
                    }
                }

                // Add new drone if multishot
                if (multishots != 0)
                {
                    for (int i = 0; i < multishots; i++)
                    {
                        Vector2 spawnPos = new Vector2(transform.position.x + Random.Range(-1f, 1f),
                            transform.position.y + Random.Range(-1f, 1f));
                        Drone newDrone = Instantiate(shipData.drone.obj, spawnPos, transform.rotation).GetComponent<Drone>();
                        newDrone.Setup(this, shipData.drone);
                        drones.Add(newDrone);
                    }
                }
            }
        }
        else if (card is WeaponData)
        {
            // Get weapon data
            WeaponData weapon = (WeaponData)card;
            if (weaponInstances.ContainsKey(weapon)) return;

            // Create the new weapon instance
            Debug.Log("Adding weapon card " + weapon.name + " to deck");
            Weapon newWeapon = Instantiate(weapon.obj, transform.position, Quaternion.identity).GetComponent<Weapon>();
            if (weapon.setPlayerAsParent) newWeapon.transform.SetParent(transform);
            newWeapon.Setup(weapon, transform);
            weaponInstances.Add(weapon, newWeapon);
        }
        else if (card is HelperData)
        {
            // Get weapon data
            HelperData helper = (HelperData)card;
            if (helperInstances.ContainsKey(helper)) return;

            Debug.Log("Adding helper card " + helper.name + " to deck");
            Helper newHelper = Instantiate(helper.obj, transform.position, Quaternion.identity).GetComponent<Helper>();
            newHelper.Setup(this, helper);
            helperInstances.Add(helper, newHelper);
        }
        else if (card is SecondaryData)
        {
            // Get weapon data
            SecondaryData secondary = (SecondaryData)card;
            if (secondaryInstances.ContainsKey(secondary)) return;

            Debug.Log("Adding secondary card " + secondary.name + " to deck");
            Secondary newSecondary = Instantiate(secondary.obj, transform.position, Quaternion.identity).GetComponent<Secondary>();
            newSecondary.Setup(this, secondary);
            secondaryInstances.Add(secondary, newSecondary);
        }
        else if (card is ChromaData)
        {
            // Get weapon data
            ChromaData chroma = (ChromaData)card;

            Debug.Log("Adding chroma card " + chroma.name + " to deck");
            ChromaHandler.active.Setup(chroma.type);
        }
    }

    public void AddSynergy(SynergyData synergy)
    {
        // Remove old card instances
        if (synergy.removeCardOne) RemoveCardInstance(synergy.cardOne);
        if (synergy.removeCardTwo) RemoveCardInstance(synergy.cardTwo);
        AddCard(synergy.outputCard);
    }

    public void RemoveCardInstance(CardData card)
    {
        if (card is WeaponData)
        {
            WeaponData weapon = (WeaponData)card;
            if (weaponInstances.ContainsKey(weapon)) 
            { 
                Destroy(weaponInstances[weapon].gameObject);
                weaponInstances.Remove(weapon);
            }
        }
        else if (card is HelperData)
        {
            HelperData helper = (HelperData)card;
            if (helperInstances.ContainsKey(helper))
            {
                Destroy(helperInstances[helper].gameObject);
                helperInstances.Remove(helper);
            }
        }
        else if (card is SecondaryData)
        {
            SecondaryData secondary = (SecondaryData)card;
            if (secondaryInstances.ContainsKey(secondary))
            {
                Destroy(secondaryInstances[secondary].gameObject);
                secondaryInstances.Remove(secondary);
            }
        }
    }

    // Set the secondary weapon
    public void SetSecondary(SecondaryData secondary)
    {
        // Remove old instance
        this.secondary = secondary;
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
                    model.rotation, bulletsToFire, bloom, size, shipData.weapon.material, true, explosiveRounds);
                else BulletHandler.active.CreateEnergyBullet(this, shipData.weapon, barrel.position,
                    model.rotation, bulletsToFire, bloom, size, defaultGlow, true, explosiveRounds);
            }
            else
            {
                if (Settings.shipColoring) BulletHandler.active.CreateBullet(this, shipData.weapon, barrel.position,
                    model.rotation, bulletsToFire, bloom, size, shipData.weapon.material, true, explosiveRounds);
                else BulletHandler.active.CreateBullet(this, shipData.weapon, barrel.position,
                    model.rotation, bulletsToFire, bloom, size, defaultGlow, true, explosiveRounds);
            }
            shipCooldown = cooldown;
        }
    }

    // Set drone targets
    public override void TargetHit(Entity entity)
    {
        if (drones != null)
        {
            foreach (Drone drone in drones)
                drone.SetTarget(entity);
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
        damage = damage * enemyDamageMultiplier;

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
    public override void UpdateStat(Stat type)
    {
        switch(type)
        {
            case Stat.Damage:
                damage = (Deck.CalculateStat(type, weapon.damage)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Cooldown:
                cooldown = (Deck.CalculateStat(type, weapon.cooldown)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Spread:
                bloom = (Deck.CalculateStat(type, weapon.bloom)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Pierces:
                pierces = (Deck.CalculateStat(type, weapon.pierces)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Bullets:
                bullets = (Deck.CalculateStat(type, weapon.bullets)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Lifetime:
                lifetime = (Deck.CalculateStat(type, weapon.lifetime)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Knockback:
                knockback = (Deck.CalculateStat(type, weapon.knockback)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.StunLength:
                stunLength = (Deck.CalculateStat(type, weapon.stun)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.BulletSize:
                size = (Deck.CalculateStat(type, weapon.bulletSize)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Range:
                range = (Deck.CalculateStat(type, weapon.range)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Criticals:
                critical = (Deck.CalculateStat(type, 0) + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Health:
                float oldHealth = maxHealth;
                maxHealth = Deck.CalculateStat(type, shipData.startingHealth);
                health += maxHealth - oldHealth;
                UpdateHealth();
                break;
            case Stat.View:
                cam.orthographicSize = (Deck.CalculateStat(type, Gamemode.arena.startingViewRange)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.MoveSpeed:
                controller.moveSpeed = (Deck.CalculateStat(type, weapon.moveSpeed)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.DashSpeed:
                controller.dashSpeed = (Deck.CalculateStat(type, shipData.dashSpeed)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.XPGain:
                xpReceiver.SetXPMultiplier((Deck.CalculateStat(type, 1)
                    + GetAdditions(type)) * GetMultiplier(type));
                break;
            case Stat.Regen:
                regenAmount = (Deck.CalculateStat(type, shipData.regenAmount)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Splitshot:
                splitshots = (Deck.CalculateStat(type, 0)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Explosive:
                explosiveRounds = Deck.GetAdditions(type) > 0 || GetAdditions(type) > 0;
                break;
            case Stat.EnemyDmg:
                enemyDamageMultiplier = (Deck.CalculateStat(type, 1)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Syphon:
                EnemyHandler.syphon = (Deck.CalculateStat(type, 0f)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Buckshot:
                buckshots = (int)((Deck.CalculateStat(type, 0)
                    + GetAdditions(type)) * GetMultiplier(type));
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
                return xpReceiver.GetXPMultiplier();

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
                return enemyDamageMultiplier;

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

    // Get weapon
    public Weapon GetWeapon(WeaponData weapon)
    {
        if (weaponInstances.ContainsKey(weapon))
            return weaponInstances[weapon];
        else return null;
    }

    // Get helper
    public Helper GetHelper(HelperData helper)
    {
        if (helperInstances.ContainsKey(helper))
            return helperInstances[helper];
        else return null;
    }

    // Get secondary
    public Secondary GetSecondary(SecondaryData secondary)
    {
        if (secondaryInstances.ContainsKey(secondary))
            return secondaryInstances[secondary];
        else return null;
    }
}
