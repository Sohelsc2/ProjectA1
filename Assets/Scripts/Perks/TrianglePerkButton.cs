using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class TrianglePerkButton : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private PerkData perkData; // Store the perk data
    [SerializeField] private TextMeshProUGUI perkText; // Reference to the TextMeshPro component
    [SerializeField] private GameObject tooltip; // Reference to the tooltip GameObject
    [SerializeField] private float hoverDuration = 1f; // Duration to show tooltip
    private float hoverTimer = 0f; // Timer for hover
    private bool isHovering = false; // Is the mouse hovering
    public GameObject storageGrid; // The grid under triangle storage

    // Store the original position
    private Vector3 originalPosition;

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
        // Snap to the position where it's dropped within the storage area
        RectTransform dropZoneRect = dropZone.GetComponent<RectTransform>();
        Vector3 localPoint;
        // Convert screen position to local point in the drop zone
        RectTransformUtility.ScreenPointToLocalPointInRectangle(dropZoneRect, eventData.position, null, out Vector2 localPoint2D);
        localPoint = new Vector3(localPoint2D.x, localPoint2D.y, 0); // Convert Vector2 to Vector3

        // Set the position to the new local point
        transform.localPosition = localPoint;
    }
    else
    {
        // If not dropped in a valid area, revert to the original position
        // also "reset" the Grid by de- and reactivating it to restore the proper location
        transform.localPosition = originalPosition;
        storageGrid.GetComponent<GridLayoutGroup>().enabled = false;
        storageGrid.GetComponent<GridLayoutGroup>().enabled = true;
        
    }

    // Reset hover state and hide tooltip
    hoverTimer = 0f; // Reset the hover timer
    isHovering = false;
    HideTooltip(); // Hide the tooltip on drag end
}


    private Transform GetDropZone(Vector3 position)
    {
        // You can add logic here to determine if the position is inside the specific storage areas
        // For now, return null to avoid placing the perk anywhere
        // Implement actual drop zone detection based on your layout
        // For example:
        // if (Physics2D.OverlapPoint(position)) return someStorageTransform;

        return null; // Replace with logic to check if the position is within a valid storage area
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
