using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject perkButtonPrefab; // Reference to the PerkButton prefab
    [SerializeField] private Transform perkGrid; // Reference to the PerkGrid
    [SerializeField] private Button rerollButton; // Reference to the reroll button
    [SerializeField] private List<PerkData> availablePerks; // List of available PerkData assets

    [SerializeField] private StorageManager storageManager; // Reference to the StorageManager

    private void Start()
    {
        // Set up the reroll button listener
        rerollButton.onClick.AddListener(RerollPerks);
        GenerateRandomPerks();
    }

    private void GenerateRandomPerks()
    {
        // Clear previous perk buttons
        foreach (Transform child in perkGrid)
        {
            Destroy(child.gameObject);
        }

        // Generate 2 perks of each type
        CreatePerkButton(GetRandomPerkOfType(ShapeType.Triangle));
        CreatePerkButton(GetRandomPerkOfType(ShapeType.Triangle));
        CreatePerkButton(GetRandomPerkOfType(ShapeType.Circle));
        CreatePerkButton(GetRandomPerkOfType(ShapeType.Circle));
        CreatePerkButton(GetRandomPerkOfType(ShapeType.Square));
        CreatePerkButton(GetRandomPerkOfType(ShapeType.Square));
    }

    private PerkData GetRandomPerkOfType(ShapeType shapeType)
    {
        // Filter the available perks by shape type and return a random one
        List<PerkData> filteredPerks = availablePerks.FindAll(perk => perk.shapeType == shapeType);
        if (filteredPerks.Count > 0)
        {
            return filteredPerks[Random.Range(0, filteredPerks.Count)];
        }
        return null; // Handle the case where there are no perks of that type
    }

    private void CreatePerkButton(PerkData perkData)
    {
        if (perkData == null) return; // Avoid creating a button if no perk data is available

        GameObject perkButtonObj = Instantiate(perkButtonPrefab, perkGrid);
        PerkButton perkButton = perkButtonObj.GetComponent<PerkButton>();
        perkButton.Initialize(perkData); // Pass the PerkData object to the PerkButton

        // Add a listener for when the player buys or clicks on the perk button
        perkButtonObj.GetComponent<Button>().onClick.AddListener(() => OnPerkButtonClick(perkButtonObj, perkData));
    }

    private void OnPerkButtonClick(GameObject perkButtonObj, PerkData perkData)
    {
        // Try to add the perk to the appropriate storage
        bool addedToStorage = storageManager.TryAddPerkToStorage(perkData, perkButtonObj);

        if (addedToStorage)
        {
            // You can handle successful purchase logic here (e.g., deduct currency, etc.)
            Debug.Log("Perk added to storage successfully.");
        }
        else
        {
            Debug.Log("Storage is full for this perk type.");
        }
    }

    private void RerollPerks()
    {
        GenerateRandomPerks();
    }
}
