using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    // List of damage numbers
    public List<DamageNumber> activeNumbers;
    public List<DamageNumber> inactiveNumbers;

    // Other variables
    public static DamageHandler active;
    public static float critChance = 0.1f;
    public DamageNumber damageNumber;
    private DamageNumber newNumber;

    public void Awake() 
    { 
        active = this;
        critChance = 0.1f;
        activeNumbers = new List<DamageNumber>();
        inactiveNumbers = new List<DamageNumber>();
    }

    public void CreateNumber(Vector2 position, float amount, bool crit)
    {
        // Attempt to use pooled object
        if (inactiveNumbers.Count > 0)
        {
            inactiveNumbers[0].transform.position = position;
            inactiveNumbers[0].gameObject.SetActive(true);
            inactiveNumbers[0].Set(amount, crit);
            activeNumbers.Add(inactiveNumbers[0]);
            inactiveNumbers.RemoveAt(0);
        }
        else
        {
            newNumber = Instantiate(damageNumber, position, Quaternion.identity);
            newNumber.Set(amount, crit);
            activeNumbers.Add(newNumber);
        }
    }

    public void Update()
    {
        for (int i = 0; i < activeNumbers.Count; i++)
        {
            // Check if still active
            if (!activeNumbers[i].isActive)
            {
                inactiveNumbers.Add(activeNumbers[i]);
                activeNumbers.RemoveAt(i);
                i--;
            }
            else activeNumbers[i].Move();
        }
    }
}
