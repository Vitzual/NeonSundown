using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeStats : MonoBehaviour
{
    // Start is called before the first frame update
    public static int enemiesDestroyed, bulletsFired, cardsChosen, synergiesCreated;
    public static float totalXP, damageGiven, damageTaken;

    // Reset stats
    public static void ResetStats()
    {
        totalXP = 0;
        enemiesDestroyed = 0;
        bulletsFired = 0;
        damageGiven = 0;
        damageTaken = 0;
        cardsChosen = 0;
        synergiesCreated = 0;
    }
}
