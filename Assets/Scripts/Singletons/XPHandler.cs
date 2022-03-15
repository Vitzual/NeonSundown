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

    // XP mover class
    public class XPInstance
    {
        public XPInstance(float value, SpriteRenderer xpModel, Transform transform, Vector2 startPos, float speed = 10f, float timer = 15f)
        {
            this.value = value;
            this.xpModel = xpModel;
            this.transform = transform;
            this.startPos = startPos;
            this.timer = timer;
            this.speed = speed;

            isStarting = true;
            isMoving = false;
        }

        public float value;
        public Transform transform;
        public SpriteRenderer xpModel;
        public Vector2 startPos;
        public bool isStarting;
        public bool isMoving;
        public float timer;
        public float speed;
    }

    // List of active movers
    private List<XPInstance> xpList = new List<XPInstance>();

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
        for (int a = 0; a < xpList.Count; a++)
        {
            if (xpList[a].transform != null)
            {
                // If moving, aim towards player
                if (xpList[a].isMoving)
                {
                    // Move towards the target
                    float step = xpList[a].speed * Time.deltaTime;
                    xpList[a].transform.position = Vector2.MoveTowards(xpList[a].transform.position, player.transform.position, step);
                    xpList[a].speed += speedIncreaseModifier;

                    // Check distance
                    if (Vector2.Distance(xpList[a].transform.position, player.transform.position) < targetDistanceCheck)
                    {
                        player.AddXP(xpList[a].value);
                        if (SaveSystem.saveData.level < Levels.ranks.Count)
                            xpRequirement.text = Formatter.Round(SaveSystem.saveData.xp) + " / " +
                                Levels.ranks[SaveSystem.saveData.level].xpRequirement + "xp";
                        if (xpHealing) player.Heal(0.05f);
                        AudioPlayer.Play(xpSound, true, 0.8f, 1.2f, false, 1.5f);
                        Destroy(xpList[a].transform.gameObject);
                        xpList.RemoveAt(a);
                        a--;
                    }
                }

                // Check if starting
                else if (xpList[a].isStarting)
                {
                    // Move towards the target
                    float step = xpList[a].speed * Time.deltaTime;
                    xpList[a].transform.position = Vector2.MoveTowards(xpList[a].transform.position, xpList[a].startPos, step);
                    xpList[a].speed -= speedFatigueModifier;

                    // Check distance
                    if (Vector2.Distance(xpList[a].transform.position, xpList[a].startPos)
                        < targetDistanceCheck || xpList[a].speed <= 0f)
                    {
                        xpList[a].isStarting = false;
                        xpList[a].speed = 5f;
                    }
                }

                // Check if moving
                else if (!xpList[a].isMoving)
                {
                    xpList[a].timer -= Time.deltaTime;
                    if (xpList[a].timer < 0f)
                        xpList[a].isMoving = true;
                }
            }
            else
            {
                xpList.RemoveAt(a);
                a--;
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
            reward.text = Levels.ranks[SaveSystem.saveData.level].GetName();
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
        // Spawn XP
        if (!Settings.compoundXP)
        {
            for (int i = 0; i < amount; i++)
            {
                XP newXp = Instantiate(xpObject, startPos, Quaternion.identity);
                newXp.Setup(new Vector2(startPos.x + Random.Range(-startDistance, startDistance),
                    startPos.y + Random.Range(-startDistance, startDistance)), xpValue);
            }
        }
        else
        {
            XP newXp = Instantiate(xpObject, startPos, Quaternion.identity);
            newXp.Setup(new Vector2(startPos.x + Random.Range(-startDistance, startDistance),
                startPos.y + Random.Range(-startDistance, startDistance)), xpValue * amount);
        }

        // If crystal spawn set to true, spawn
        if (crystal != null)
        {
            Crystal newCrystal = Instantiate(crystal, startPos, Quaternion.identity).GetComponent<Crystal>();
            newCrystal.Setup(50);
            newCrystal.SetSpeed();
        }
    }

    // Register mover
    public XPInstance Register(float value, SpriteRenderer xpModel, Transform transform, Vector2 startPos)
    {
        XPInstance newXP = new XPInstance(value, xpModel, transform, startPos);
        xpList.Add(newXP);
        return newXP;
    }

    // Enable XP healing
    public void EnableXPHealing()
    {
        xpValue = xpValue / 2;
        xpHealing = true;
    }
}
