using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StorageManager : MonoBehaviour
{
    [SerializeField] private Transform triangleStorage;
    [SerializeField] private Transform circleStorage;
    [SerializeField] private Transform squareStorage;

    [SerializeField] private GameObject trianglePerkPrefab; // Storage button prefab for triangle perks
    [SerializeField] private GameObject circlePerkPrefab;   // Storage button prefab for circle perks
    [SerializeField] private GameObject squarePerkPrefab;   // Storage button prefab for square perks

    private const int MaxPerksInStorage = 6; // Maximum of 6 perks per storage

    // This method tries to add the perk to storage, and returns whether it succeeded or not
    public bool TryAddPerkToStorage(PerkData perkData, GameObject shopButton)
    {
        Transform targetStorageGrid = null;
        GameObject perkPrefab = null;

        // Decide which storage and which prefab to use based on shape type
        switch (perkData.shapeType)
        {
            case ShapeType.Triangle:
                targetStorageGrid = triangleStorage.Find("Grid"); // Assumes your Grid is named "Grid"
                perkPrefab = trianglePerkPrefab;
                break;
            case ShapeType.Circle:
                targetStorageGrid = circleStorage.Find("Grid"); // Assumes your Grid is named "Grid"
                perkPrefab = circlePerkPrefab;
                break;
            case ShapeType.Square:
                targetStorageGrid = squareStorage.Find("Grid"); // Assumes your Grid is named "Grid"
                perkPrefab = squarePerkPrefab;
                break;
        }

        // Check if the storage grid has space
        if (targetStorageGrid != null && targetStorageGrid.childCount < MaxPerksInStorage)
        {
            // Instantiate and add the perk to the appropriate storage grid
            GameObject perkButtonObj = Instantiate(perkPrefab, targetStorageGrid);
            PerkButtonStorage perkButton = perkButtonObj.GetComponent<PerkButtonStorage>();
            if (perkButton == null)
            {
                Debug.LogError("PerkButtonStorage component is missing on the storage button prefab.");
                return false; // Fail if the script is missing
            }

            perkButton.Initialize(perkData); // Initialize with perk data
            perkButtonObj.GetComponent<Button>().onClick.AddListener(perkButton.OnClick); // Add click listener

            return true; // Success
        }
        else
        {
            // Storage is full, trigger the red button behavior on the shop button
            StartCoroutine(ShowStorageFullWarning(shopButton));
            return false; // Failed
        }
    }

    // Coroutine to turn the shop button red temporarily
    private IEnumerator ShowStorageFullWarning(GameObject shopButton)
    {
        Button buttonComponent = shopButton.GetComponent<Button>();
        Image buttonImage = shopButton.GetComponent<Image>();
        Color originalColor = buttonImage.color;

        // Disable button interaction
        buttonComponent.interactable = false;

        // Turn the button red
        buttonImage.color = Color.red;

        // Wait for 1 second (adjust the duration as needed)
        yield return new WaitForSeconds(1.0f);

        // Revert back to the original color
        buttonImage.color = originalColor;

        // Re-enable button interaction
        buttonComponent.interactable = true;
    }

    // Call this function to clear the storage if needed
    public void ClearStorage()
    {
        ClearStorageOfType(triangleStorage.Find("Grid"));
        ClearStorageOfType(circleStorage.Find("Grid"));
        ClearStorageOfType(squareStorage.Find("Grid"));
    }

    // Helper method to clear individual storage grids
    private void ClearStorageOfType(Transform storageGrid)
    {
        foreach (Transform child in storageGrid)
        {
            Destroy(child.gameObject);
        }
    }
}
