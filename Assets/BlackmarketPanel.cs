using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlackmarketPanel : MonoBehaviour
{
    // Progress tracker
    [System.Serializable]
    public class Progress
    {
        public BlackmarketData.Type type;
        public TextMeshProUGUI amount;
        public ProgressBar bar;
    }

    // Progress bars
    public List<Progress> progress;

    // Labs stage
    public ShipData defaultShip;
    public ArenaData labs;

    // Reference to blackmarket
    public Blackmarket blackmarket;

    // Panel components 
    public TextMeshProUGUI title, desc, amount, buy, 
        test, redCrystals, greenCrystals, blueCrystals;
    public Image icon, crystal, border, background,
        buyButton, testButton;
    public GameObject notSelectedView, selectedView;

    // Color settings
    public CrystalData blueCrystal, redCrystal, greenCrystal;

    // Data holder
    private BlackmarketData data;
    private bool alreadyPurchased = false, isPreviewing = false;
    private int totalAmountOfItems = 0;

    // Equip sound
    public AudioClip equipSound, buySound;
    private AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Events.active.onBlackmarketItemClicked += SetItem;
        UpdateProgress();
    }

    public void UpdateProgress()
    {
        // Reset total amount
        totalAmountOfItems = 0;

        // Dictionary of types
        Dictionary<BlackmarketData.Type, int> totalAmount = new Dictionary<BlackmarketData.Type, int>();
        Dictionary<BlackmarketData.Type, int> progressAmount = new Dictionary<BlackmarketData.Type, int>();

        // Iterate through all 
        foreach (BlackmarketData data in Scriptables.blackmarketItems)
        {
            if (totalAmount.ContainsKey(data.type))
            {
                if (!data.unlockByDefault) totalAmount[data.type] += 1;
                if (SaveSystem.IsBlackmarketItemUnlocked(data.InternalID))
                    progressAmount[data.type] += 1;
            }
            else
            {
                if (!data.unlockByDefault) totalAmount.Add(data.type, 1);
                else totalAmount.Add(data.type, 0);
                progressAmount.Add(data.type, 0);
                if (SaveSystem.IsBlackmarketItemUnlocked(data.InternalID))
                    progressAmount[data.type] += 1;
            }
        }

        // Set progress bars
        foreach (KeyValuePair<BlackmarketData.Type, int> amount in totalAmount)
        {
            foreach (Progress pro in progress)
            {
                if (pro.type == amount.Key)
                {
                    pro.amount.text = progressAmount[amount.Key] + "/" + amount.Value;
                    pro.bar.maxValue = amount.Value;
                    pro.bar.currentPercent = progressAmount[amount.Key];
                    pro.bar.UpdateUI();
                    break;
                }
            }

            totalAmountOfItems += amount.Value;
        }
    }

    public void ResetPanel()
    {
        // Set selected view
        notSelectedView.gameObject.SetActive(true);
        selectedView.gameObject.SetActive(false);
        blackmarket.UpdateListings();
        CancelPreview();
    }

    public void SetItem(BlackmarketData data)
    {
        // Cancel preview
        CancelPreview();

        // Set data object
        this.data = data;

        // Check if already purchased
        alreadyPurchased = data.unlockByDefault || SaveSystem.IsBlackmarketItemUnlocked(data.InternalID);

        // If already purchased, display
        if (alreadyPurchased)
        {
            if (data.type == BlackmarketData.Type.Card)
            {
                buy.text = "OWNED";
            }
            else
            {
                // Set button thing
                bool isEquipped = false;
                switch (data.type)
                {
                    case BlackmarketData.Type.Ship:
                        isEquipped = data.ship == Gamemode.shipData;
                        break;
                    case BlackmarketData.Type.Arena:
                        isEquipped = data.arena == Gamemode.arena;
                        break;
                    case BlackmarketData.Type.Audio:
                        isEquipped = data.audio == MusicPlayer.GetMusicData();
                        break;
                }

                // Check if equipped
                if (isEquipped) buy.text = "EQUIPPED";
                else buy.text = "EQUIP";
            }
        }
        else buy.text = "PURCHASE";

        // Set selected view
        notSelectedView.gameObject.SetActive(false);
        selectedView.gameObject.SetActive(true);

        // Set text
        title.text = data.name.ToUpper();
        desc.text = data.desc;
        amount.text = data.amountRequired + " CRYSTALS";
        icon.sprite = data.icon;

        // Set colors
        border.color = data.lightColor;
        background.color = data.darkColor;
        title.color = data.lightColor;
        buyButton.color = data.lightColor;
        testButton.color = data.lightColor;
        if (data.useLightColorOnIcon)
            icon.color = data.lightColor;

        // Set crystal color
        crystal.color = data.crystal.lightColor;

        // Set button thing
        switch (data.type)
        {
            case BlackmarketData.Type.Ship:
                test.text = "TRY IN LABS";
                break;
            case BlackmarketData.Type.Arena:
                test.text = "PLAY MUSIC";
                break;
            case BlackmarketData.Type.Card:
                test.text = "TRY IN LABS";
                break;
            case BlackmarketData.Type.Audio:
                test.text = "PLAY MUSIC";
                break;
        }
    }

    public void BuyItem()
    {
        // Check if already purchased
        if (alreadyPurchased)
        {
            // Depending on type, unlock new content
            if (EquipItem(data)) ResetPanel();
            return;
        }

        // If not already purchased, unlock and reset panels
        if (!SaveSystem.IsBlackmarketItemUnlocked(data.InternalID) &&
            SaveSystem.GetCrystalAmount(data.crystal.InternalID) >= data.amountRequired)
        {
            // Depending on type, unlock new content
            switch (data.type)
            {
                case BlackmarketData.Type.Ship:
                    if (data.ship != null && !SaveSystem.IsShipUnlocked(data.ship.InternalID))
                        SaveSystem.AddShipUnlock(data.ship.InternalID);
                    else Debug.Log(data.name + " is already unlocked!");
                    break;
                case BlackmarketData.Type.Arena:
                    if (data.arena != null && !SaveSystem.IsArenaUnlocked(data.arena.InternalID))
                        SaveSystem.AddArenaUnlock(data.arena.InternalID);
                    else Debug.Log(data.name + " is already unlocked!");
                    break;
                case BlackmarketData.Type.Card:
                    if (data.card != null && !SaveSystem.IsCardUnlocked(data.card.InternalID))
                        SaveSystem.AddCardUnlock(data.card.InternalID);
                    else Debug.Log(data.name + " is already unlocked!");
                    break;
                case BlackmarketData.Type.Audio:
                    if (data.audio != null && !SaveSystem.IsAudioModUnlocked(data.audio.InternalID))
                        SaveSystem.AddAudioMod(data.audio.InternalID);
                    else Debug.Log(data.name + " is already unlocked!");
                    break;
            }

            // Add blackmarket internal ID
            SaveSystem.AddBlackmarketItem(data.InternalID);

            // Remove crystals from player
            SaveSystem.AddCrystal(data.crystal.InternalID, -data.amountRequired);
            
            // Update the save
            SaveSystem.UpdateSave();

            // Play equip sound
            if (audioSource != null)
            {
                audioSource.volume = Settings.sound;
                audioSource.clip = buySound;
                audioSource.Play();
            }

            // Equip the new item
            EquipItem(data, false);

            // Reset the panel
            ResetPanel();
            UpdateProgress();
            UpdateResources();

            // Call on buy event
            Events.active.BlackmarketItemBought(data);
        }
    }

    // Depending on item, preview it
    public void PreviewItem()
    {
        // Cancel preview
        CancelPreview();

        // Set button thing
        switch (data.type)
        {
            case BlackmarketData.Type.Ship:
                Gamemode.shipData = data.ship;
                Gamemode.arena = labs;
                SceneManager.LoadScene("Main");
                break;
            case BlackmarketData.Type.Arena:
                MusicPlayer.PlaySong(data.arena.arenaMusic);
                isPreviewing = true;
                break;
            case BlackmarketData.Type.Card:
                Gamemode.shipData = defaultShip;
                Gamemode.arena = labs;
                Gamemode.startingCards.Add(data.card);
                SceneManager.LoadScene("Main");
                break;
            case BlackmarketData.Type.Audio:
                if (data.audio == MusicPlayer.GetMusicData()) return;
                MusicPlayer.music.pitch = 1f;
                MusicPlayer.PlaySong(data.audio.audio);
                isPreviewing = true;
                break;
        }
    }

    public void CancelPreview()
    {
        if (isPreviewing)
        {
            isPreviewing = false;
            MusicPlayer.music.pitch = Gamemode.arena.arenaMenuPitch;
            MusicPlayer.ResetSong();
        }
    }

    public bool EquipItem(BlackmarketData data, bool playSound = true)
    {
        // Depending on type, unlock new content
        switch (data.type)
        {
            case BlackmarketData.Type.Ship:
                if (data.ship == Gamemode.shipData) return false;
                Events.active.SetupShip(data.ship);
                break;
            case BlackmarketData.Type.Arena:
                if (data.arena == Gamemode.arena) return false;
                Events.active.ArenaButtonClicked(data.arena);
                break;
            case BlackmarketData.Type.Audio:
                if (data.audio == MusicPlayer.GetMusicData()) return false;
                MusicPlayer.SetMusicData(data.audio);
                break;
            default:
                return false;
        }

        // Play equip sound
        if (playSound && audioSource != null)
        {
            audioSource.volume = Settings.sound;
            audioSource.clip = equipSound;
            audioSource.Play();
        }

        // Return true
        return true;
    }

    public int GetTotalAmountOfItems()
    {
        return totalAmountOfItems;
    }

    // Update the crystal amounts
    public void UpdateResources()
    {
        // Update blue crystals
        int amount;
        if (SaveSystem.saveData.crystals.ContainsKey(blueCrystal.InternalID))
        {
            amount = SaveSystem.saveData.crystals[blueCrystal.InternalID];
            if (amount > 999) blueCrystals.text = "999";
            else blueCrystals.text = amount.ToString();
        }
        else blueCrystals.text = "0";

        // Update green crystals
        if (SaveSystem.saveData.crystals.ContainsKey(greenCrystal.InternalID))
        {
            amount = SaveSystem.saveData.crystals[greenCrystal.InternalID];
            if (amount > 999) greenCrystals.text = "999";
            else greenCrystals.text = amount.ToString();
        }
        else greenCrystals.text = "0";

        // Update red crystals
        if (SaveSystem.saveData.crystals.ContainsKey(redCrystal.InternalID))
        {
            amount = SaveSystem.saveData.crystals[redCrystal.InternalID];
            if (amount > 999) redCrystals.text = "999";
            else redCrystals.text = amount.ToString();
        }
        else redCrystals.text = "0";
    }
}
