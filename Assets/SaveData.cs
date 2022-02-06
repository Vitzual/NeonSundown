using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public SaveData(int xpCrystals, int bloodCrystals, int lifeCrystals)
    {
        this.xpCrystals = xpCrystals;
        this.bloodCrystals = bloodCrystals;
        this.lifeCrystals = lifeCrystals;
    }

    public int xpCrystals;
    public int bloodCrystals;
    public int lifeCrystals;
}
