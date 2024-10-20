using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StorageManager : MonoBehaviour
{
    [SerializeField] private Transform triangleGrid; // The grid under triangle storage
    [SerializeField] private Transform circleGrid;   // The grid under circle storage
    [SerializeField] private Transform squareGrid;   // The grid under square storage

    [SerializeField] private GameObject trianglePerkPrefab; // Storage button prefab for triangle perks
    [SerializeField] private GameObject circlePerkPrefab;   // Storage button prefab for circle perks
    [SerializeField] private GameObject squarePerkPrefab;   // Storage button prefab for square perks

    private const int MaxPerksInStorage = 6; // Maximum of 6 perks per storage

    // This method tries to add the perk to storage, and returns whether it succeeded or not
    public bool TryAddPerkToStorage(PerkData perkData, GameObject shopButton)
    {
        if (perkData == null){
            Debug.LogWarning("perkData is null in tryaddperktostorage.");
        }
        Transform targetGrid = null; // This will hold the grid to which we want to add the perk button
        GameObject perkPrefab = null;

        // Decide which grid and which prefab to use based on shape type
        switch (perkData.shapeType)
        {
            case ShapeType.Triangle:
                targetGrid = triangleGrid;
                perkPrefab = trianglePerkPrefab;
                break;
            case ShapeType.Circle:
                targetGrid = circleGrid;
                perkPrefab = circlePerkPrefab;
                break;
            case ShapeType.Square:
                targetGrid = squareGrid;
                perkPrefab = squarePerkPrefab;
                break;
        }

        // Check if the grid has space
        if (targetGrid != null && targetGrid.childCount < MaxPerksInStorage)
        {
            // Instantiate and add the perk to the appropriate grid
            GameObject perkButtonObj = Instantiate(perkPrefab, targetGrid);

            // Initialize with the appropriate button script based on shape type
            switch (perkData.shapeType)
            {
                case ShapeType.Triangle:
                    //TrianglePerkButton triangleButton = perkButtonObj.GetComponent<TrianglePerkButton>();
                    //triangleButton.Initialize(perkData); // Initialize with perk data
                    //perkButtonObj.GetComponent<Button>().onClick.AddListener(triangleButton.OnClick); // Add click listener
                    TrianglePerkButton triangleButton = perkButtonObj.GetComponent<TrianglePerkButton>();
                            if (perkData == null){
                            Debug.LogWarning("perkData is null in tryaddperktostorage2.");
                            }
                    triangleButton.Initialize(perkData); // Pass the PerkData object to the TrianglePerkButton
                    perkButtonObj.GetComponent<Button>().onClick.AddListener(triangleButton.OnClick); // Add the click listener
                break;

                case ShapeType.Circle:
                    CirclePerkButton circleButton = perkButtonObj.GetComponent<CirclePerkButton>();
                    circleButton.Initialize(perkData);
                    perkButtonObj.GetComponent<Button>().onClick.AddListener(circleButton.OnClick);
                    break;

                case ShapeType.Square:
                    SquarePerkButton squareButton = perkButtonObj.GetComponent<SquarePerkButton>();
                    squareButton.Initialize(perkData);
                    perkButtonObj.GetComponent<Button>().onClick.AddListener(squareButton.OnClick);
                    break;
            }

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
    Button button = shopButton.GetComponent<Button>(); // Get the Button component
    Image buttonImage = button.GetComponent<Image>(); // Get the Image component
    Color originalColor = buttonImage.color;

    // Turn the button red and disable it
    buttonImage.color = Color.red;
    button.interactable = false;

    // Wait for 1 second (adjust the duration as needed)
    yield return new WaitForSeconds(1.0f);

    // Revert back to the original color and re-enable the button
    buttonImage.color = originalColor;
    button.interactable = true;
}

    // Call this function to clear the storage if needed
    public void ClearStorage()
    {
        ClearStorageOfType(triangleGrid);
        ClearStorageOfType(circleGrid);
        ClearStorageOfType(squareGrid);
    }

    // Helper method to clear individual storage
    private void ClearStorageOfType(Transform grid)
    {
        foreach (Transform child in grid)
        {
            Destroy(child.gameObject);
        }
    }
}
