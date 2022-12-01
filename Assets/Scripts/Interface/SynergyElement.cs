using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// I am not happy with the way this works
/// </summary>

public class SynergyElement : MonoBehaviour
{
    // Start is called before the first frame update
    public SynergyProgress basicSynergyOne;
    public SynergyProgress basicSynergyTwo;
    public SynergyProgress advancedSynergy;
    [HideInInspector] public SynergyData synergyData;
    
    // Overarching synergy variables
    public TextMeshProUGUI treeName;

    // Setup the synergy
    public void Set(SynergyData tree)
    {
        synergyData = tree;
        basicSynergyOne.Set(tree.synergyOne);
        basicSynergyTwo.Set(tree.synergyTwo);
        advancedSynergy.Set(tree);
        treeName.text = tree.name.ToUpper();
    }

    // Update the synergies
    public void UpdateSynergies()
    {
        if (synergyData != null && synergyData != null)
        {
            basicSynergyOne.Set(synergyData.synergyOne);
            basicSynergyTwo.Set(synergyData.synergyTwo);
            advancedSynergy.Set(synergyData);
        }
    }
}
