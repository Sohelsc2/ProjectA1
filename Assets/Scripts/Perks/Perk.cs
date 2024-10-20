using UnityEngine;

public class Perk : MonoBehaviour
{
    public PerkData perkData;  // Reference to the ScriptableObject storing perk data
    public bool isActive;      // Whether this perk is currently active in gameplay

    // Method to move the perk between places
    public void MoveTo(Transform targetPosition)
    {
        // Check if the slot allows the perk's shape type
        if (IsValidSlot(targetPosition))
        {
            // Move the perk to the new slot visually
            transform.position = targetPosition.position;

            // Set whether it's active or not based on where it is placed
            CheckLocation(targetPosition);
        }
        else
        {
            Debug.Log("Invalid slot for this perk shape!");
        }
    }

    // Check if the target slot matches the perk's shape type
    private bool IsValidSlot(Transform targetPosition)
    {
        Slot slot = targetPosition.GetComponent<Slot>();
        if (slot != null && slot.allowedShape == perkData.shapeType)
        {
            return true;
        }
        return false;
    }

    // Check where the perk is moved to (storage, active place, or trash)
    private void CheckLocation(Transform newLocation)
    {
        if (newLocation.CompareTag("ActivePlace"))
        {
            isActive = true;
        }
        else if (newLocation.CompareTag("StoragePlace"))
        {
            isActive = false;
        }
        else if (newLocation.CompareTag("TrashPlace"))
        {
            DestroyPerk();
        }
    }

    public void ApplyEffect()
    {
        if (isActive)
        {
            // Handle the perk's effect based on the effectKey
            switch (perkData.effectKey)
            {
                case "IncreaseSpeed":
                    Debug.Log("Speed increased!");
                    break;
                case "DoubleDamage":
                    Debug.Log("Double damage activated!");
                    break;
                // Add more effect cases as needed
                default:
                    Debug.Log("Unknown effect: " + perkData.effectKey);
                    break;
            }
        }
    }

    public void DestroyPerk()
    {
        Destroy(gameObject);
    }
}
