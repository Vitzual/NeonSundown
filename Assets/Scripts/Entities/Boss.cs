using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // Reference the main enemy script
    protected Enemy enemy;
    protected Transform target;

    // On start, get reference to enemy script
    public void Start()
    {
        enemy = GetComponent<Enemy>();
        target = EnemyHandler.active.player;
    }

    // Control turrets
    public void Update()
    {
        
    }
}
