using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stage", menuName = "Arena/Stage")]
public class StageData : IdentifiableScriptableObject
{
    [System.Serializable]
    public class Enemy
    {
        public EnemyData data;
        public Variant variant;
        public float cooldown;
        public int amount = 1;
    }

    public new string name;
    public bool isMenuStage;
    public bool enemyScaling;
    public List<Enemy> enemies;
    public float time;

    // Runtime display variables
    private string runtime = "";

    // Called by scriptable loader on setup
    public float CalcTotal(float runningTime)
    {
        float endTime = runningTime + time;

        if (enemyScaling) runtime = Formatter.Time(runningTime) + " - INFINITE";
        else runtime = Formatter.Time(runningTime) + " - " + Formatter.Time(endTime);

        return endTime;
    }

    // Returns a string of the stage time
    public string GetTime()
    {
        return runtime;
    }
}
