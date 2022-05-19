using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPHandler : MonoBehaviour
{
    // Active instance
    public static XPHandler active;
    public Image rewardIcon;
    public TextMeshProUGUI reward, xpRequirement;
    public GameObject rewardObject;
    public Material deadXP;
    public CrystalData redCrystal;

    // List of active movers
    private List<XP> activeList = new List<XP>();
    private List<XP> inactiveList = new List<XP>();
    
    // Target
    public XP xpObject;
    public AudioClip xpSound;
    public Ship player;
    public float startSpeed = 5f;
    public float startDistance = 10f;
    public float speedFatigueModifier = 0.1f;
    public float speedIncreaseModifier = 0.1f;
    public float targetDistanceCheck = 1f;
    public float xpValue = 1;
    private bool xpHealing = false;

    // On start set active
    public void Start() 
    {
        active = this;
        UpdateRewards();
    }
    
    // Move normal enemies
    public void FixedUpdate()
    {
        // Check if something is open
        if (Dealer.isOpen) return;

        // Move enemies each frame towards their target
        for (int a = 0; a < activeList.Count; a++)
        {
            // If moving, aim towards player
            if (activeList[a].isMoving)
            {
                // Move towards the target
                float step = activeList[a].speed * Time.deltaTime;
                activeList[a].transform.position = Vector2.MoveTowards(activeList[a].transform.position, player.transform.position, step);
                activeList[a].speed += speedIncreaseModifier;

                // Check distance
                if (Vector2.Distance(activeList[a].transform.position, player.transform.position) < targetDistanceCheck)
                {
                    player.AddXP(activeList[a].value);
                    RuntimeStats.totalXP += activeList[a].value;
                    if (SaveSystem.saveData.level < Levels.ranks.Count)
                        xpRequirement.text = Formatter.Round(SaveSystem.saveData.xp, 0) + " / " +
                            Levels.ranks[SaveSystem.saveData.level].xpRequirement + "xp";
                    if (xpHealing) Ship.Heal(0.01f * activeList[a].value);
                    activeList[a].gameObject.SetActive(false);
                    inactiveList.Add(activeList[a]);
                    activeList.RemoveAt(a);
                    a--;
                }
            }

            // Check if starting
            else if (activeList[a].isStarting)
            {
                // Move towards the target
                float step = activeList[a].speed * Time.deltaTime;
                activeList[a].transform.position = Vector2.MoveTowards(activeList[a].transform.position, activeList[a].startPos, step);
                activeList[a].speed -= speedFatigueModifier;

                // Check distance
                if (Vector2.Distance(activeList[a].transform.position, activeList[a].startPos)
                    < targetDistanceCheck || activeList[a].speed <= 0f)
                {
                    activeList[a].isMoving = true;
                    activeList[a].isStarting = false;
                    activeList[a].speed = 5f;
                    activeList[a].trail.enabled = true;
                }
            }
        }
    }

    // Update the interface rewards
    public void UpdateRewards()
    {
        if (Levels.ranks == null)
            Levels.GenerateRanks();

        if (SaveSystem.saveData.level < Levels.ranks.Count)
        {
            reward.text = Levels.ranks[SaveSystem.saveData.level].GetName().ToUpper();
            Color color = Levels.ranks[SaveSystem.saveData.level].GetColor();
            rewardIcon.sprite = Levels.ranks[SaveSystem.saveData.level].GetIcon();
            if (Levels.ranks[SaveSystem.saveData.level].IsColored()) rewardIcon.color = color;
            else rewardIcon.color = Color.white;
            xpRequirement.text = Formatter.Round(SaveSystem.saveData.xp) + " / " + 
                Levels.ranks[SaveSystem.saveData.level].xpRequirement + "xp";
            xpRequirement.color = color;
        }
        else
        {
            reward.gameObject.SetActive(false);
            xpRequirement.gameObject.SetActive(false);
            rewardObject.SetActive(false);
        }
    }

    // Spawn XP
    public void Spawn(Vector2 startPos, int amount, Crystal crystal = null)
    {
        // Check if XP available in list
        if (inactiveList.Count > 0)
        {
            if (!Settings.compoundXP)
            {
                // Amount to spawn
                int amountToSpawn = amount;

                for (int i = 0; i < amountToSpawn; i++)
                {
                    XP newXP = inactiveList[0];
                    newXP.transform.position = startPos;
                    newXP.Setup(new Vector2(startPos.x + Random.Range(-startDistance, startDistance),
                                startPos.y + Random.Range(-startDistance, startDistance)), xpValue);
                    newXP.gameObject.SetActive(true);
                    activeList.Add(newXP);
                    inactiveList.RemoveAt(0);
                    amount -= 1;
                    if (inactiveList.Count <= 0) break;
                }
            }
            else
            {
                XP newXP = inactiveList[0];
                newXP.transform.position = startPos;
                newXP.Setup(new Vector2(startPos.x + Random.Range(-startDistance, startDistance),
                    startPos.y + Random.Range(-startDistance, startDistance)), xpValue * amount);
                newXP.gameObject.SetActive(true);
                activeList.Add(newXP);
                inactiveList.RemoveAt(0);
                amount = 0;
            }
        }

        if (amount > 0)
        {
            if (!Settings.compoundXP)
            {
                for (int i = 0; i < amount; i++)
                {
                    XP newXp = Instantiate(xpObject, startPos, Quaternion.identity);
                    newXp.Setup(new Vector2(startPos.x + Random.Range(-startDistance, startDistance),
                        startPos.y + Random.Range(-startDistance, startDistance)), xpValue);
                    activeList.Add(newXp);
                }
            }
            else
            {
                XP newXp = Instantiate(xpObject, startPos, Quaternion.identity);
                newXp.Setup(new Vector2(startPos.x + Random.Range(-startDistance, startDistance),
                    startPos.y + Random.Range(-startDistance, startDistance)), xpValue * amount);
                activeList.Add(newXp);
            }
        }

        // If crystal spawn set to true, spawn
        if (crystal != null)
        {
            // Check if synergy is available
            Crystal newCrystal = Instantiate(crystal, startPos, Quaternion.identity).GetComponent<Crystal>();
            if (SynergyHandler.synergyCrystalOverride > 0)
            {
                SynergyHandler.synergyCrystalOverride -= 1;
                newCrystal.Setup(50, redCrystal);
            }
            else newCrystal.Setup(50);

            newCrystal.SetSpeed();
        }
    }

    // Enable XP healing
    public void EnableXPHealing()
    {
        xpHealing = true;
    }
}
