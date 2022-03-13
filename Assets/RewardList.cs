using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardList : MonoBehaviour
{
    // Reward entry 
    public Reward reward;
    public Transform list;
    public Sprite normalLevel;
    public Sprite specialLevel;
    public Image levelIcon, button;
    public TextMeshProUGUI levelText, unlockName, nextUnlock;
    public ProgressBar unlockProgress;
    private float r = 1, g = 0, b = 0;
    private bool redIncrease = false, greenIncrease = true, 
        blueIncrease = true, red = false, green = false, blue = true;
    public float maxColorSpeed = 0.01f;
    public bool useRainbow = false;

    // Generate all rewards
    public void Start()
    {
        int currentLevel = 0;
        
        // Iterate through sorted list and reate rewards
        foreach(LevelData rank in Levels.ranks)
        {
            // Create new reward
            Reward newReward = Instantiate(reward, Vector2.zero, Quaternion.identity);
            newReward.transform.SetParent(list);
            newReward.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            // Set reward
            if (currentLevel != 0 && (currentLevel + 1) % 5 == 0) 
                newReward.Set(rank, currentLevel + 1, SaveSystem.saveData.level > currentLevel, specialLevel);
            else newReward.Set(rank, currentLevel + 1, SaveSystem.saveData.level > currentLevel, normalLevel);
            currentLevel += 1;
        }

        // Set progress
        SetProgress();
    }

    // If at max, use rainbows
    public void Update()
    {
        if (!useRainbow) return;

        if (red)
        {
            if (redIncrease)
            {
                r += maxColorSpeed * Time.deltaTime;
                if (r >= 1)
                {
                    red = false;
                    green = true;
                    greenIncrease = false;
                }
            }
            else 
            {
                r -= maxColorSpeed * Time.deltaTime;
                if (r <= 0.7f)
                {
                    red = false;
                    green = true;
                    greenIncrease = true;
                }
            }
        }
        else if (green)
        {
            if (greenIncrease)
            {
                g += maxColorSpeed * Time.deltaTime;
                if (r > 0) r -= maxColorSpeed * Time.deltaTime;
                if (g >= 1)
                {
                    green = false;
                    blue = true;
                    blueIncrease = false;
                }
            }
            else
            {
                g -= maxColorSpeed * Time.deltaTime;
                if (g <= 0)
                {
                    green = false;
                    blue = true;
                    blueIncrease = true;
                }
            }
        }
        else if (blue)
        {
            if (blueIncrease)
            {
                b += maxColorSpeed * Time.deltaTime;
                if (b >= 1)
                {
                    blue = false;
                    red = true;
                    redIncrease = false;
                }
            }
            else
            {
                b -= maxColorSpeed * Time.deltaTime;
                if (b <= 0)
                {
                    blue = false;
                    red = true;
                    redIncrease = true;
                }
            }
        }

        Color color = new Color(r, g, b);
        levelIcon.color = color;
        unlockName.color = color;
        nextUnlock.color = color;
        button.color = color;
        unlockProgress.loadingBar.color = color;
    }

    // Set progress information
    public void SetProgress()
    {
        int currentLevel = SaveSystem.saveData.level;
        if (currentLevel < Levels.ranks.Count)
        {
            Color color = Levels.ranks[currentLevel].GetColor();
            levelText.text = currentLevel.ToString();
            levelIcon.color = color;
            nextUnlock.color = color;
            unlockName.text = Levels.ranks[currentLevel].GetName();
            button.color = color;
            unlockProgress.loadingBar.color = color;
            unlockProgress.currentPercent = (SaveSystem.saveData.xp / 
                Levels.ranks[currentLevel].xpRequirement) * 100;
            unlockProgress.UpdateUI();
        }
        else
        {
            useRainbow = true;
            levelText.text = "50";
            unlockName.text = "LEVEL MAX";
            unlockProgress.currentPercent = 100;
        }
    }
}
