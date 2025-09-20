using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ArenaPanel : MonoBehaviour
{
    [System.Serializable]
    public class Chip
    {
        public GameObject obj;
        public Image icon;
        public Image model;
        public TextMeshProUGUI modifier, amount;
    }

    // Menu volume profile
    public VolumeProfile menuVolume;

    // Stages panel
    public StagesPanel stagesPanel;

    // Arena elements
    public ArenaButton arenaButton;
    public Transform arenaList, cardList;

    [BoxGroup("Arena Boss")]
    public GameObject noBoss, arenaBoss;
    [BoxGroup("Arena Boss")]
    public Image bossIcon;
    [BoxGroup("Arena Boss")]
    public TextMeshProUGUI bossName;
    
    // Background tilebase
    public Tilemap backgroundTilemap;

    // Card and chip slots
    public BlacklistCard blacklistCard;
    public SerializableDictionary<Difficulty, Color> difficultyColors;
    public AudioClip nightmareArenaSound;
    private List<GameObject> activeBlacklistCards;
    private List<ArenaButton> buttonList;
    protected List<ArenaButton> limitedTimeArenas = new List<ArenaButton>();

    // Objective sprite options
    public Color incompleteObjectiveColor, completeObjectiveColor, limitedTimeArenaColor;
    public Sprite incompleteObjectiveIcon, completeObjectiveIcon;

    // Image components
    public Image arenaIcon, panelBackground, panelBorder, objectiveIcon;

    // Text components
    public TextMeshProUGUI arenaName, arenaDesc, arenaTime, objective, limitedTimeArena;

    // Game object components
    public GameObject blacklistEmpty, chipsLocked;

    // Private internal flag
    private bool arenasGenerated = false, chipsActivated = false;

    // Subscribe to the arena button event
    public void Start()
    {
        // Setup events
        Events.active.onArenaButtonClicked += SetPanel;
        Events.active.onBlackmarketItemBought += UpdateArenaListing;

        // Check if arenas already generated
        if (!arenasGenerated)
        {
            // Setup arenas
            buttonList = new List<ArenaButton>();

            ArenaData[] arenas = new ArenaData[Scriptables.arenas.Count];
            List<ArenaData> unsorted = new List<ArenaData>(Scriptables.arenas);
            for (int i = 0; i < arenas.Length; i++)
            {
                int lowest = int.MaxValue;
                ArenaData lowestArena = null;
                foreach (ArenaData arena in unsorted)
                {
                    if (arena.order < lowest) 
                    {
                        lowest = arena.order;
                        lowestArena = arena;
                    }
                }
                arenas[i] = lowestArena;
                unsorted.Remove(lowestArena);
            }

            // Iterate through all arenas
            foreach (ArenaData arena in arenas)
            {
                // Check if limited time
                bool limitedTimeStillAvailable = false;
                if (arena.limitedTimeArena)
                {
                    DateTime endTime = new DateTime(
                        arena.limitedTimeYear,
                        arena.limitedTimeMonth,
                        arena.limitedTimeDay);
                    DateTime nowTime = DateTime.Now;
                    TimeSpan time = endTime - nowTime;

                    if (time.TotalSeconds > 0f) 
                        limitedTimeStillAvailable = true;
                }

                // Create arena buttons
                ArenaButton newButton = Instantiate(arenaButton, Vector2.zero, Quaternion.identity);
                newButton.transform.SetParent(arenaList);
                RectTransform rect = newButton.GetComponent<RectTransform>();
                rect.localScale = new Vector3(1, 1, 1);
                buttonList.Add(newButton);

                // Add arena button to limited time list
                if (arena.limitedTimeArena)
                {
                    if (limitedTimeStillAvailable)
                    {
                        limitedTimeArenas.Add(newButton);
                        if (SaveSystem.GetPlayerLevel() >= 50)
                        {
                            if (SaveSystem.saveData.arenaTimes.ContainsKey(arena.InternalID))
                            {
                                newButton.Set(arena, "<b>Best Run:</b> " + Formatter.Time
                                    (SaveSystem.saveData.arenaTimes[arena.InternalID]), true);
                            }
                            else newButton.Set(arena, "<b>Best Run:</b> 0:00", true);
                        }
                        else newButton.Lock(arena, true);
                    }
                    else
                    {
                        if (SaveSystem.saveData.arenaTimes.ContainsKey(arena.InternalID))
                        {
                            newButton.Set(arena, "<b>Best Run:</b> " + Formatter.Time
                                (SaveSystem.saveData.arenaTimes[arena.InternalID]));
                        }
                        else
                        {
                            if (SaveSystem.IsArenaUnlocked(arena.InternalID))
                                newButton.Set(arena, "<b>Best Run:</b> 0:00");
                            else newButton.Lock(arena);
                        }
                    }
                }

                // Check if save is unlocked, and if so set time
                else if (arena.unlockByDefault || SaveSystem.IsArenaUnlocked(arena.InternalID))
                {
                    if (SaveSystem.saveData.arenaTimes.ContainsKey(arena.InternalID))
                    {
                        newButton.Set(arena, "<b>Best Run:</b> " + Formatter.Time
                            (SaveSystem.saveData.arenaTimes[arena.InternalID]));
                    }
                    else newButton.Set(arena, "<b>Best Run:</b> 0:00");
                }

                // If arena not unlocked, show it as locked
                else newButton.Lock(arena);
            }

            // Set arenas generated flag to true
            arenasGenerated = true;
        }
    }

    public void Update()
    {
        foreach (ArenaButton button in limitedTimeArenas)
        {
            button.UpdateTimer();
        }
    }

    public void SetPanel(ArenaData arena)
    {
        // Set the new arena data
        Gamemode.arena = arena;

        // Reset the panel colors
        arenaIcon.sprite = arena.unlockedIcon;
        panelBorder.color = arena.buttonColor;
        panelBackground.color = arena.darkColor;

        // Set arena information
        arenaName.text = arena.name.ToUpper();
        arenaDesc.text = arena.shortDesc;
        arenaTime.text = "<b>BEST RUN:</b> " + Formatter.Time(SaveSystem.GetBestTime(arena.InternalID));

        // Set arena boss info
        if (arena.boss != null)
        {
            arenaBoss.SetActive(true);
            noBoss.SetActive(false);

            bossName.text = arena.boss.name.ToUpper();
            bossName.color = arena.lightColor;
            bossIcon.sprite = arena.boss.icon;
            bossIcon.color = arena.buttonColor;
        }
        else
        {
            arenaBoss.SetActive(false);
            noBoss.SetActive(true);
        }

        // Set arena difficulty
        if (arena.limitedTimeArena)
        {
            arenaName.text += " <size=18><color=#" + ColorUtility.ToHtmlStringRGB(limitedTimeArenaColor)
                + ">LIMITED TIME ARENA";
        }
        else if (difficultyColors.ContainsKey(arena.difficulty))
        {
            arenaName.text += " <size=18><color=#" + ColorUtility.ToHtmlStringRGB(difficultyColors[arena.difficulty])
                + ">" + arena.difficulty.ToString().ToUpper();
        }

        // Check if nightmare
        if (arena.difficulty == Difficulty.Nightmare)
        {
            // Play nightmare sound
            AudioPlayer.Play(nightmareArenaSound, false, 1f, 1f, true, 2f);

            // Set glitch effect
            Effects.ToggleMenuGlitchEffect(true);
        }
        else Effects.ToggleMenuGlitchEffect(false);

        // Set arena objective
        objective.text = arena.achievementObjective.ToUpper();
        if (arena.IsAchievementUnlocked())
        {
            objectiveIcon.sprite = completeObjectiveIcon;
            objectiveIcon.color = completeObjectiveColor;
        }
        else
        {
            objectiveIcon.sprite = incompleteObjectiveIcon;
            objectiveIcon.color = incompleteObjectiveColor;
        }

        // Iterate through blacklist slots
        if (arena.blacklistCards.Count > 0)
        {
            // Remove previous blacklist cards
            RemoveBlacklistCards();

            // Set no blacklist object to false
            if (blacklistEmpty.activeSelf)
                blacklistEmpty.SetActive(false);

            // Iterate through and create cards
            foreach(CardData card in arena.blacklistCards)
            {
                BlacklistCard newCard = Instantiate(blacklistCard, cardList);
                activeBlacklistCards.Add(newCard.gameObject);
                newCard.Set(card);
            }
        }
        else
        {
            // Remove previous blacklist cards
            RemoveBlacklistCards();

            // Set no blacklist object to true
            if (!blacklistEmpty.activeSelf)
                blacklistEmpty.SetActive(true);
        }

        // Set menu music
        if (MusicPlayer.isMenu)
        {
            Debug.Log("Setting music pitch to " + arena.arenaMenuPitch);
            MusicPlayer.music.pitch = arena.arenaMenuPitch;
        }

        // Change the background
        stagesPanel.Set(arena);
        stagesPanel.stageButtonIndex = 0;
        stagesPanel.SwitchStage(0);
        ChangeBackground(arena);
    }

    public void ChangeBackground(ArenaData arena)
    {
        // Check if stage already active
        if (MenuSpawner.active.menuStage == arena.menuStage) return;

        // Set the arena stage
        MenuSpawner.active.menuStage = arena.menuStage;

        // Wipe the old background
        MenuSpawner.active.WipeEnemies();

        // Set the background
        for (int x = -4; x < 4; x++)
            for (int y = -4; y < 4; y++)
                backgroundTilemap.SetTile(new Vector3Int(x, y, 0), arena.arenaBackground);
     }

    /// <summary>
    /// Removes all instantiated blacklist cards
    /// </summary>
    public void RemoveBlacklistCards()
    {
        // Remove previous blacklist cards
        if (activeBlacklistCards != null)
        {
            for (int i = 0; i < activeBlacklistCards.Count; i++)
            {
                Destroy(activeBlacklistCards[i]);
                activeBlacklistCards.RemoveAt(i);
                i--;
            }
        }
        
        // Create new active list
        activeBlacklistCards = new List<GameObject>();
    }

    public void UpdateArenaListing(BlackmarketData data)
    {
        foreach (ArenaButton button in buttonList)
            if (data.type == BlackmarketData.Type.Arena && data.arena == button.arena)
                button.Set(button.arena, "<b>BEST RUN:</b> " + Formatter.Time(SaveSystem.GetBestTime(button.arena.InternalID)));
    }
}
