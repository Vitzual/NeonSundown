using Michsky.UI.ModernUIPack;
using System.Collections;
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

    // Background tilebase
    public Tilemap backgroundTilemap;

    // Card and chip slots
    public BlacklistCard blacklistCard;
    public SerializableDictionary<Difficulty, Color> difficultyColors;
    public AudioClip nightmareArenaSound;
    private List<GameObject> activeBlacklistCards;

    // Objective sprite options
    public Color incompleteObjectiveColor, completeObjectiveColor;
    public Sprite incompleteObjectiveIcon, completeObjectiveIcon;

    // Image components
    public Image arenaIcon, panelBackground, panelBorder, objectiveIcon;

    // Text components
    public TextMeshProUGUI arenaName, arenaDesc, arenaTime, objective;

    // Game object components
    public GameObject blacklistEmpty, chipsLocked;

    // Private internal flag
    private bool arenasGenerated = false, chipsActivated = false;

    // Subscribe to the arena button event
    public void Start()
    {
        // Setup events
        Events.active.onArenaButtonClicked += SetPanel;

        // Check if arenas already generated
        if (!arenasGenerated)
        {
            // Setup arenas
            List<ArenaButton> buttonList = new List<ArenaButton>();

            // Iterate through all arenas
            foreach (ArenaData arena in Scriptables.arenas)
            {
                // Create arena buttons
                ArenaButton newButton = Instantiate(arenaButton, Vector2.zero, Quaternion.identity);
                newButton.transform.SetParent(arenaList);
                RectTransform rect = newButton.GetComponent<RectTransform>();
                rect.localScale = new Vector3(1, 1, 1);
                buttonList.Add(newButton);

                // Check if save is unlocked, and if so set time
                if (arena.unlockByDefault || SaveSystem.IsArenaUnlocked(arena.InternalID))
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

            // I'll come back to this later. Unity is being a dummy and
            // not giving two shits about setting the sibling index

            // Iterate through all generated buttons and set order
            foreach (ArenaButton button in buttonList)
                button.gameObject.transform.SetSiblingIndex(button.arena.order);

            // Iterate through all generated buttons and set order
            foreach (ArenaButton button in buttonList)
                button.gameObject.transform.SetSiblingIndex(button.arena.order);

            // Iterate through all generated buttons and set order
            foreach (ArenaButton button in buttonList)
                button.gameObject.transform.SetSiblingIndex(button.arena.order);

            // Set arenas generated flag to true
            arenasGenerated = true;
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

        // Set arena difficulty
        if (difficultyColors.ContainsKey(arena.difficulty))
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
}
