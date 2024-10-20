using UnityEngine;

public class PerkManager : MonoBehaviour
{
    public GameObject perkPrefab;  // Assign the prefab that represents a perk visually
    public Transform storageArea;  // The area where perks are stored
    public Transform activeArea;   // The area where perks become active

    public PerkData[] availablePerks;  // Array of available perks (drag and drop ScriptableObjects here)

    void Start()
    {
        // Example: instantiate perks in storage at the start of the game
        foreach (var perkData in availablePerks)
        {
            CreatePerk(perkData, storageArea);
        }
    }

    // Create a perk in the specified location (e.g., storage or active area)
    public void CreatePerk(PerkData perkData, Transform parent)
    {
        GameObject perkObject = Instantiate(perkPrefab, parent.position, Quaternion.identity, parent);
        Perk perkComponent = perkObject.GetComponent<Perk>();

        perkComponent.perkData = perkData;  // Assign the ScriptableObject data to the Perk
        perkComponent.isActive = false;     // Perks are inactive by default in storage
    }
}
