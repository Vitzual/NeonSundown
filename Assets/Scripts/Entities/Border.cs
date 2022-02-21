using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject theWall;

    public void Start()
    {
        if (Gamemode.arena.useWall)
            EnableTheWall();
    }

    public void EnableTheWall()
    {
        Transform wall = Instantiate(theWall, Vector2.zero, Quaternion.identity).GetComponent<Transform>();
        wall.SetParent(transform);
    }
}
