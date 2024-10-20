using UnityEngine;

public class Perk : MonoBehaviour
{
    public PerkData perkData;  // Reference to the ScriptableObject storing perk data

    public bool isActive;  // Whether this perk is currently active in gameplay

    // Method to move the perk between places
    public void MoveTo(Transform targetPosition)
    {
        transform.position = targetPosition.position;
        CheckLocation(targetPosition);
    }

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
            // This is where we handle the effect based on the effectKey
            switch (perkData.effectKey)
            {
                case "IncreaseSpeed":
                    // Apply speed boost to the player
                    Debug.Log("Speed increased by SamplePerk!");
                    break;
                case "DoubleDamage":
                    // Apply double damage effect to the player
                    Debug.Log("Player deals double damage with SamplePerk!");
                    break;
                // Add more cases for different effects
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
