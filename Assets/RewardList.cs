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

        // Create sorted list
        LevelData[] levels = new LevelData[Levels.ranks.Count];

        // Sort level data
        foreach (LevelData rank in Levels.ranks)
            levels[int.Parse(rank.name) - 1] = rank;
        
        // Iterate through sorted list and reate rewards
        foreach(LevelData rank in levels)
        {
            // Create new reward
            Reward newReward = Instantiate(reward, Vector2.zero, Quaternion.identity);
            newReward.transform.SetParent(list);
            newReward.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            // Set reward
            if (currentLevel != 0 && (currentLevel + 1) % 5 == 0) 
                newReward.Set(rank, currentLevel + 1, Levels.level > currentLevel, specialLevel);
            else newReward.Set(rank, currentLevel + 1, Levels.level > currentLevel, normalLevel);
            currentLevel += 1;
        }
    }
}
