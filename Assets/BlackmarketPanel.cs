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
    public TextMeshProUGUI title, desc, amount,
        buy, test;
    public Image icon, crystal, border, background,
        buyButton, testButton;
    public GameObject notSelectedView, selectedView;

    // Color settings
    public Color blueCrystal, redCrystal, greenCrystal;

    // Data holder
    private BlackmarketData data;
    private bool alreadyPurchased = false, isPreviewing = false;

    public void Start()
    {
        Events.active.onBlackmarketItemClicked += SetItem;
        UpdateProgress();
    }

    public void UpdateProgress()
    {
        // Dictionary of types
        Dictionary<BlackmarketData.Type, int> totalAmount = new Dictionary<BlackmarketData.Type, int>();
        Dictionary<BlackmarketData.Type, int> progressAmount = new Dictionary<BlackmarketData.Type, int>();

        // Iterate through all 
        foreach (BlackmarketData data in Scriptables.blackmarketItems)
        {
            if (totalAmount.ContainsKey(data.type))
            {
                totalAmount[data.type] += 1;
                if (SaveSystem.IsBlackmarketItemUnlocked(data.InternalID))
                    progressAmount[data.type] += 1;
            }
            else
            {
                totalAmount.Add(data.type, 1);
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
                    break;
                }
            }
        }
    }

    public void ResetPanel()
    {
        // Set selected view
        notSelectedView.gameObject.SetActive(true);
        selectedView.gameObject.SetActive(false);
        blackmarket.UpdateListings();
        if (isPreviewing) CancelPreview();
        UpdateProgress();
    }

    public void SetItem(BlackmarketData data)
    {
        // Cancel preview
        if (isPreviewing) CancelPreview();

        // Set data object
        this.data = data;

        // Check if already purchased
        alreadyPurchased = SaveSystem.IsBlackmarketItemUnlocked(data.InternalID);

        // If already purchased, display
        if (alreadyPurchased) buy.text = "ALREADY PURCHASED";
        else buy.text = "PURCHASE";

        // Set selected view
        notSelectedView.gameObject.SetActive(false);
        selectedView.gameObject.SetActive(true);

        // Set text
        title.text = data.name.ToUpper();
        desc.text = data.desc.ToUpper();
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
        switch (data.crystal)
        {
            case CrystalType.blue:
                crystal.color = blueCrystal;
                break;
            case CrystalType.green:
                crystal.color = greenCrystal;
                break;
            case CrystalType.red:
                crystal.color = redCrystal;
                break;
        }

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
        if (alreadyPurchased) return;

        // If not already purchased, unlock and reset panels
        if (!SaveSystem.IsBlackmarketItemUnlocked(data.InternalID))
        {
            // Depending on type, unlock new content
            switch (data.type)
            {
                case BlackmarketData.Type.Ship:
                    if (data.ship != null && !SaveSystem.IsShipUnlocked(data.ship.InternalID))
                        SaveSystem.AddShipUnlock(data.ship.InternalID);
                    break;
                case BlackmarketData.Type.Arena:
                    if (data.arena != null && !SaveSystem.IsArenaUnlocked(data.arena.InternalID))
                        SaveSystem.AddArenaUnlock(data.arena.InternalID);
                    break;
                case BlackmarketData.Type.Card:
                    if (data.card != null && !SaveSystem.IsCardUnlocked(data.card.InternalID))
                        SaveSystem.AddCardUnlock(data.card.InternalID);
                    break;
                case BlackmarketData.Type.Audio:
                    if (data.audio != null && !SaveSystem.IsAudioModUnlocked(data.audio.InternalID))
                        SaveSystem.AddAudioMod(data.audio.InternalID);
                    break;
            }

            // Add blackmarket internal ID
            SaveSystem.AddBlackmarketItem(data.InternalID);
        }

        // Reset the panel
        ResetPanel();
    }

    // Depending on item, preview it
    public void PreviewItem()
    {
        // Cancel preview
        CancelPreview();
        isPreviewing = true;

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
                break;
            case BlackmarketData.Type.Card:
                Gamemode.shipData = defaultShip;
                Gamemode.arena = labs;
                Gamemode.startingCards.Add(data.card);
                SceneManager.LoadScene("Main");
                break;
            case BlackmarketData.Type.Audio:
                MusicPlayer.PlaySong(data.audio.audio);
                break;
        }
    }

    public void CancelPreview()
    {
        MusicPlayer.ResetSong();
    }
}
