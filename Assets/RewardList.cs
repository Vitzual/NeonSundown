using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardList : MonoBehaviour
{
    // Reward entry 
    public Reward reward;
    public Transform list;

    // Generate all rewards
    public void Start()
    {
        int currentLevel = 0;
        foreach(LevelData rank in Levels.ranks)
        {
            Reward newReward = Instantiate(reward, Vector2.zero, Quaternion.identity);
            newReward.transform.SetParent(list);
            newReward.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            newReward.Set(rank, currentLevel + 1, Levels.level > currentLevel);
            currentLevel += 1;
        }
    }
}
