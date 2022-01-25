using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Tracking
    public float timer;
    public float cooldown;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        cooldown -= Time.deltaTime;

        foreach(EnemyData enemy in )
    }
}
