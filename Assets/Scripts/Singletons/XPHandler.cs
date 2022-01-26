using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPHandler : MonoBehaviour
{
    // Active instance
    public static XPHandler active;

    // XP mover class
    public class XPInstance
    {
        public XPInstance(Transform transform, Vector2 startPos, float speed = 10f, float timer = 15f)
        {
            this.transform = transform;
            this.startPos = startPos;
            this.timer = timer;
            this.speed = speed;

            isStarting = true;
            isMoving = false;
        }

        public Transform transform;
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
    public Transform player;
    public float startSpeed = 5f;
    public float startDistance = 10f;
    public float speedFatigueModifier = 0.1f;
    public float speedIncreaseModifier = 0.1f;
    public float targetDistanceCheck = 1f;

    // On start set active
    public void Start() { active = this; }

    // Move normal enemies
    public void FixedUpdate()
    {
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
                    xpList[a].transform.position = Vector2.MoveTowards(xpList[a].transform.position, player.position, step);
                    xpList[a].speed += speedIncreaseModifier;

                    // Check distance
                    if (Vector2.Distance(xpList[a].transform.position, player.position) < targetDistanceCheck)
                    {
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
                    {
                        Destroy(xpList[a].transform.gameObject);
                        xpList.RemoveAt(a);
                        a--;
                    }
                }
            }
            else
            {
                xpList.RemoveAt(a);
                a--;
            }
        }
    }

    // Spawn XP
    public void Spawn(Vector2 startPos, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            XP newXp = Instantiate(xpObject, startPos, Quaternion.identity);
            newXp.Setup(new Vector2(startPos.x + Random.Range(-startDistance, startDistance),
                startPos.y + Random.Range(-startDistance, startDistance)));
        }
    }

    // Register mover
    public XPInstance Register(Transform transform, Vector2 startPos)
    {
        XPInstance newXP = new XPInstance(transform, startPos);
        xpList.Add(newXP);
        return newXP;
    }
}
