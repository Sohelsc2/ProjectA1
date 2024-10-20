using System.Collections.Generic;
using UnityEngine;

public class PerkManager : MonoBehaviour
{
    public GameObject perkPrefab;  // Prefab for the perk
    public List<Slot> activeSlots; // List of slots in the active area (ensure it's ordered)

    public PerkData[] availablePerks;  // Available perks (ScriptableObjects)

    void Start()
    {
        // Example: instantiate perks in storage at the start
        foreach (var perkData in availablePerks)
        {
            // Create a new GameObject named "MyObject"
            GameObject myObject = new GameObject("MyObject");
            // Get the Transform component of the GameObject (all GameObjects have a Transform by default)
            Transform myTransform = myObject.transform;
            // Set the position of the GameObject to (5, 0, 0)
            myTransform.position = new Vector3(5, 0, 0);
            // Set the rotation of the GameObject to (0, 45, 0) degrees
            myTransform.rotation = Quaternion.Euler(0, 0, 0);
            // Set the scale of the GameObject to (2, 2, 2)
            myTransform.localScale = new Vector3(2, 2, 2);
            CreatePerk(perkData, myTransform);
        }
    }

    // Create a perk in the specified location (e.g., storage or active area)
public void CreatePerk(PerkData perkData, Transform parent)
{
    GameObject perkObject = Instantiate(perkPrefab, parent.position, Quaternion.identity, parent);
    Perk perkComponent = perkObject.GetComponent<Perk>();

    perkComponent.perkData = perkData;  // Assign data
    perkComponent.isActive = false;     // Perks are inactive by default

    // Set up the PerkVisual component
    PerkVisual perkVisual = perkObject.GetComponent<PerkVisual>();
    if (perkVisual != null)
    {
        perkVisual.perk = perkComponent; // Link perk data to visual script
    }
}


    // Method to start the game and apply active perks in order
    public void StartGame()
    {
        foreach (Slot slot in activeSlots)
        {
            Perk perkInSlot = slot.GetComponentInChildren<Perk>();

            if (perkInSlot != null && perkInSlot.isActive)
            {
                perkInSlot.ApplyEffect();  // Apply the effect of the perk in this slot
            }
        }
    }
}
