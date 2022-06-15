using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    // Active instance
    public static LineDrawer active;
    private List<Transform> laserObjects;
    public Laser laserPrefab;
    public float laserSpeed = 2f;
    public bool debug = false;

    // Get active instance
    public void Awake()
    {
        active = this;
        laserObjects = new List<Transform>();
    }
}
