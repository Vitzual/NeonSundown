using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Entities/Enemy")]
public class EnemyData : IdentifiableScriptableObject
{
    [BoxGroup("Enemy Info")]
    public new string name;
    [BoxGroup("Enemy Info"), TextArea]
    public string desc;
    [BoxGroup("Enemy Info")]
    public GameObject obj;

    [BoxGroup("Enemy Stats"), SerializeField]
    protected List<VariantData> variantStats;
    public Dictionary<Variant, VariantData> variants;

    // Generate the variants stat dictionary handler
    // Needs to be run in or der to get stats
    public void GenerateStats()
    {
        // Check if one variant defined
        if (variantStats.Count == 0)
        {
            Debug.Log(name + " has no variants defined and will not work!");
            return;
        }

        // Generate stats
        Debug.Log("Generating " + name + " scriptable stat data...");
        variants = new Dictionary<Variant, VariantData>();
        foreach (VariantData variant in variantStats)
            variants.Add(variant.variant, variant);
        Debug.Log("Successfully generated " + name + " with " + variantStats.Count + " stat entries!");
    }
}
