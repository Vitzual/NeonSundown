using UnityEngine;

public class RewardList : MonoBehaviour
{
    // Reward entry 
    public Reward reward;
    public Transform list;
    public Sprite normalLevel;
    public Sprite specialLevel;

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
    }
}
