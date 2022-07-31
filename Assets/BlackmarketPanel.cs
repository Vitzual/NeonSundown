using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    public ArenaData labs;

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
    private bool alreadyPurchased = false;

    public void Start()
    {
        Events.active.onBlackmarketItemClicked += SetItem;
    }

    public void ResetPanel()
    {
        // Set selected view
        notSelectedView.gameObject.SetActive(true);
        selectedView.gameObject.SetActive(false);
    }

    public void SetItem(BlackmarketData data)
    {
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
        amount.text = amount + " CRYSTALS";

        // Set colors
        border.color = data.lightColor;
        background.color = data.darkColor;
        title.color = data.lightColor;

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
    }

    public void PreviewItem()
    {

    }
}
