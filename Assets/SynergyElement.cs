using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyElement : MonoBehaviour
{
    // Start is called before the first frame update
    public SynergyProgress topSynergy;
    public SynergyProgress bottomSynergy;
    public SynergyProgress rightSynergy;
    public SynergyData synergyData;

    // Setup the synergy
    public void Set(SynergyData synergy)
    {
        synergyData = synergy;
        topSynergy.Set(synergy.synergyOne);
        bottomSynergy.Set(synergy.synergyTwo);
        rightSynergy.Set(synergy);
    }

    // Update the synergies
    public void UpdateSynergies()
    {
        topSynergy.Set(synergyData.synergyOne);
        bottomSynergy.Set(synergyData.synergyTwo);
        rightSynergy.Set(synergyData);
    }
}
