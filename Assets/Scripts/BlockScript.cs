using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockScript : MonoBehaviour
{
    // Public block health variable
    public int blockHealth = 2;

    // Reference to the TextMeshPro component for displaying health
    private TextMeshPro healthText;

    // Start is called before the first frame update
    void Start()
    {
        // Find the TextMeshPro component on the block (assuming it's a child of the block)
        healthText = GetComponentInChildren<TextMeshPro>();

        if (healthText != null)
        {
            // Initialize the health display
            UpdateHealthDisplay();
        }
        else
        {
            Debug.LogWarning("TextMeshPro component not found on block.");
        }
    }

    // Function to apply damage to the block
    public void TakeDamage(int damage)
    {
        // Reduce block health
        blockHealth -= damage;

        // Update health display after taking damage
        UpdateHealthDisplay();

        // Check if block health has reached zero or below
        if (blockHealth <= 0)
        {
            DestroyBlock();
        }
    }

    // Function to update the health text
    private void UpdateHealthDisplay()
    {
        if (healthText != null)
        {
            // Display the current health value on the block
            healthText.text = blockHealth.ToString();
        }
    }

    // Function to destroy the block
    private void DestroyBlock()
    {
        // Add any additional logic here (e.g., sound effects, animations, etc.)
        Destroy(gameObject); // Destroys the block GameObject
    }
}
