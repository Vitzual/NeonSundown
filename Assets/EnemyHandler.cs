using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    // Active instance
    public static EnemyHandler active;

    // Contains all active enemies in the scene
    public List<Enemy> enemies = new List<Enemy>();

    // Player transform for targetting
    public Transform player;

    // On awake set active instance
    public void Awake() { active = this; }

    // Start method
    public void Start() { active = this; }

    // Handles enemy movement every frame
    public void Update() { MoveEnemies(); }

    // Move normal enemies
    public virtual void MoveEnemies()
    {
        // Move enemies each frame towards their target
        for (int a = 0; a < enemies.Count; a++)
        {
            if (enemies[a] != null)
            {
                enemies[a].Move();
            }
            else
            {
                enemies.RemoveAt(a);
                a--;
            }
        }
    }
    // Create a new active enemy instance
    public void CreateEnemy(EnemyData enemyData, Vector2 position)
    {
        // Create the tile
        GameObject lastObj = Instantiate(enemyData.obj, position, Quaternion.identity);
        lastObj.name = enemyData.name;

        // Attempt to set enemy variant
        Enemy enemy = lastObj.GetComponent<Enemy>();
        enemy.Setup(enemyData);

        // Add to enemies list
        enemies.Add(enemy);
    }
}