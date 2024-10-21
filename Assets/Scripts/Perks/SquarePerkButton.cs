using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class SquarePerkButton : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public PerkData perkData; // Store the perk data
    [SerializeField] private TextMeshProUGUI perkText; // Reference to the TextMeshPro component
    [SerializeField] private GameObject tooltip; // Reference to the tooltip GameObject
    [SerializeField] private float hoverDuration = 1f; // Duration to show tooltip
    private float hoverTimer = 0f; // Timer for hover
    private bool isHovering = false; // Is the mouse hovering
    public GameObject storageGrid; // The grid under triangle storage
    [SerializeField] private GameObject perkPrefab; // Reference to the tooltip GameObject

    // Store the original position
    private Vector3 originalPosition;
private void CreateNewPerk(Transform dropZone)
{
    // Check if the drop zone has a HorizontalLayoutGroup (or any other logic)
    if (dropZone != null)
    {
        if (perkPrefab != null)
        {
            // Instantiate the new perk
            GameObject newPerk = Instantiate(perkPrefab, dropZone);

            // Get the component of the new perk button (TrianglePerkButton)
            SquarePerkButton newPerkButton = newPerk.GetComponent<SquarePerkButton>();

            if (newPerkButton != null)
            {
                // Initialize it with the same perk data
                newPerkButton.Initialize(perkData);
            }

            // Optionally: Set position if you want a specific location within the drop zone
            // newPerk.transform.localPosition = Vector3.zero; // Set to the center of the drop zone or adjust as needed

            // Optionally: Destroy the current perk after creating the new one
            Destroy(gameObject); // This destroys the current perk button
        }
        else
        {
            Debug.LogWarning("Perk prefab not found!");
        }
    }
}

    public void Initialize(PerkData data)
    {
        perkData = data;
        UpdateButtonUI();
        originalPosition = transform.localPosition; // Save the original position
        storageGrid = transform.parent.gameObject; 
    }

    private void UpdateButtonUI()
    {
        if (perkText != null && perkData != null)
        {
            perkText.text = perkData.perkName; // Set the perk name
        }
    }

    private void Update()
    {
        if (isHovering)
        {
            hoverTimer += Time.deltaTime;
            if (hoverTimer >= hoverDuration)
            {
                ShowTooltip();
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Start dragging logic
        //transform.SetAsLastSibling(); // Bring the button to the front
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update position based on drag
        transform.position = eventData.position;
    }

public void OnEndDrag(PointerEventData eventData)
{
    // Check if it's over a valid storage area
    Transform dropZone = GetDropZone(eventData.position);
    if (dropZone != null)
    {
        CreateNewPerk(dropZone);
    }
    else
    {
        // If not dropped in a valid area, revert to the original position
        // also "reset" the Grid by de- and reactivating it to restore the proper location
        transform.localPosition = originalPosition;
        storageGrid.GetComponent<HorizontalLayoutGroup>().enabled = false;
        storageGrid.GetComponent<HorizontalLayoutGroup>().enabled = true;
        
    }

    // Reset hover state and hide tooltip
    hoverTimer = 0f; // Reset the hover timer
    isHovering = false;
    HideTooltip(); // Hide the tooltip on drag end
}

private bool HasChildren(RectTransform rectTransform)
{
    return rectTransform.childCount > 0; // Returns true if there are children, false otherwise
}
private Transform GetDropZone(Vector3 mousePosition)
{
    // Loop through all objects with PerkType
    PerkType[] perkTypes = FindObjectsOfType<PerkType>();

    foreach (PerkType perkType in perkTypes)
    {
        // Check if this perk type matches the shape of the dragged perk
        if (perkType.shapeType != this.perkData.shapeType) // Assuming perkData is accessible
            continue;

        // Get the RectTransform of the current perkType object
        RectTransform rectTransform = perkType.GetComponent<RectTransform>();

        // Check if the mouse position is within the bounds of this RectTransform
        if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition))
        {
            if (HasChildren(rectTransform))
                {
                    // You can handle logic here for when the drop zone has children
                }else{
                    return rectTransform; // Return the matching drop zone
                }
        }
    }
    // Loop through all objects with PerkType
    TrashBin[] trashBins = FindObjectsOfType<TrashBin>();
    foreach (TrashBin trashBin in trashBins)
    {
        // Get the RectTransform of the current perkType object
        RectTransform rectTransform = trashBin.GetComponent<RectTransform>();

        // Check if the mouse position is within the bounds of this RectTransform
        if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition))
        {
            Destroy(gameObject); // This destroys the current perk button
        }
    }
    return null; // No valid drop zone found
}




public ShapeType GetShapeType()
{
    return perkData.shapeType; // Assuming perkData has a shapeType property
}

    private void ShowTooltip()
    {
        if (tooltip != null && perkData != null)
        {
            tooltip.SetActive(true);
            tooltip.GetComponentInChildren<TextMeshProUGUI>().text = $"{perkData.perkName}\n{perkData.description}";

            // Set the tooltip's position to be centered at the top of the screen
            RectTransform tooltipRect = tooltip.GetComponent<RectTransform>();
            float screenHeight = Screen.height;
            float desiredY = screenHeight * 0.9f;
            tooltipRect.position = new Vector3(Screen.width / 2, desiredY, 0);
        }
    }

    private void HideTooltip()
    {
        if (tooltip != null)
        {
            tooltip.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true; // Start the hover state
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false; // End the hover state
        hoverTimer = 0f; // Reset hover timer
        HideTooltip(); // Hide tooltip when not hovering
    }

    public void OnClick()
    {
        // Logic to handle what happens when this button is clicked
        Debug.Log($"Clicked on perk: {perkData.perkName}");
        
        // Here, you can call a method in StorageManager or whatever you need
    }
}
