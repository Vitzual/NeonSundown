using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ArenaPanel : MonoBehaviour
{
    [System.Serializable]
    public class Card
    {
        public Image icon;
        public Image model;
        public TextMeshProUGUI amount;
    }

    // Stages panel
    public StagesPanel stagesPanel;

    // Arena elements
    public ArenaButton arenaButton;
    public Transform arenaList;

    // Background tilebase
    public Tilemap backgroundTilemap;

    // Canvas group
    public Image panelBackgrounder;
    public Image panelBorder;

    // Private internal flag
    private bool arenasGenerated = false;

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

        // Reset the panel
        panelBorder.color = arena.buttonColor;
        panelBackgrounder.color = arena.darkColor;

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
}
