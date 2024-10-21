using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
public class StorageManager : MonoBehaviour
{
    [SerializeField] private Transform triangleGrid; // The main grid under triangle storage
    [SerializeField] private Transform circleGrid;   // The main grid under circle storage
    [SerializeField] private Transform squareGrid;   // The main grid under square storage

    // Sub-grids under each main grid
    [SerializeField] private Transform[] triangleSubGrids; // Array of sub-grids for triangle perks
    [SerializeField] private Transform[] circleSubGrids;   // Array of sub-grids for circle perks
    [SerializeField] private Transform[] squareSubGrids;   // Array of sub-grids for square perks

    [SerializeField] private GameObject trianglePerkPrefab; // Storage button prefab for triangle perks
    [SerializeField] private GameObject circlePerkPrefab;   // Storage button prefab for circle perks
    [SerializeField] private GameObject squarePerkPrefab;   // Storage button prefab for square perks

    private const int MaxPerksInStorage = 1; // Maximum of 6 perks per storage
    public void Start(){
        triangleSubGrids = GetAllChildTransforms(triangleGrid);
        circleSubGrids = GetAllChildTransforms(circleGrid);
        squareSubGrids = GetAllChildTransforms(squareGrid);
    }
    // Function to get all children of a parent GameObject and return them as a list
    public Transform[] GetAllChildTransforms(Transform parentTransform)
    {
        List<Transform> childTransforms = new List<Transform>();

        // Loop through each child of the parent Transform
        foreach (Transform child in parentTransform)
        {
            childTransforms.Add(child); // Add the child Transform to the list
        }

        return childTransforms.ToArray(); // Convert the list to an array and return
    }   
    public Transform GetFirstChildlessTransform(Transform[] transformArray)
    {
        // Iterate through each transform in the array
        foreach (Transform t in transformArray)
        {
            // Check if the transform has no children
            if (t.childCount == 0)
            {
                return t; // Return the first transform without children
            }
        }

        // If no childless transform is found, return null
        return null;
    }

    // This method tries to add the perk to storage, and returns whether it succeeded or not
    public bool TryAddPerkToStorage(PerkData perkData, GameObject shopButton)
    {
        if (perkData == null)
        {
            Debug.LogWarning("perkData is null in tryaddperktostorage.");
            return false; // Exit early if perkData is null
        }

        Transform targetSubGrid = null; // This will hold the sub-grid to which we want to add the perk button
        GameObject perkPrefab = null;

        // Decide which sub-grid and which prefab to use based on shape type
        switch (perkData.shapeType)
        {
            case ShapeType.Triangle:
                targetSubGrid = GetFirstChildlessTransform(triangleSubGrids);
                perkPrefab = trianglePerkPrefab;
                break;
            case ShapeType.Circle:
                targetSubGrid = GetFirstChildlessTransform(circleSubGrids);
                perkPrefab = circlePerkPrefab;
                break;
            case ShapeType.Square:
                targetSubGrid = GetFirstChildlessTransform(squareSubGrids);
                perkPrefab = squarePerkPrefab;
                break;
        }

        // Check if the target sub-grid has space
        if (targetSubGrid != null && targetSubGrid.childCount < MaxPerksInStorage)
        {
            // Instantiate and add the perk to the appropriate sub-grid
            GameObject perkButtonObj = Instantiate(perkPrefab, targetSubGrid);

            // Initialize with the appropriate button script based on shape type
            switch (perkData.shapeType)
            {
                case ShapeType.Triangle:
                    TrianglePerkButton triangleButton = perkButtonObj.GetComponent<TrianglePerkButton>();
                    triangleButton.Initialize(perkData); // Initialize with perk data
                    perkButtonObj.GetComponent<Button>().onClick.AddListener(triangleButton.OnClick);
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
        ClearStorageOfType(triangleSubGrids);
        ClearStorageOfType(circleSubGrids);
        ClearStorageOfType(squareSubGrids);
    }

    // Helper method to clear individual storage
    private void ClearStorageOfType(Transform[] subGrids)
    {
        foreach (Transform grid in subGrids)
        {
            foreach (Transform child in grid)
            {
                Destroy(child.gameObject);
            }
        }
    }

}
