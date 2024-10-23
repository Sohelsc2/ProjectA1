using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockScript : MonoBehaviour
{
    // Public block health variable
    public float blockHealth = 2f;
    private List<BlockScript> allBlocks = new List<BlockScript>();

    // Reference to the TextMeshPro component for displaying health
    private TextMeshPro healthText;
    public bool lifeLink = false;
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

// Coroutine to wait for one frame before populating the list
private IEnumerator RepopulateBlocksList()
{
    // Wait for one frame to ensure any destroyed blocks are removed
    yield return null;

    // Find all remaining blocks in the scene
    allBlocks = new List<BlockScript>(FindObjectsOfType<BlockScript>());
    CreateLifeLinkVisuals(allBlocks);
}

// Call this function to repopulate the blocks list
public void UpdateBlockList()
{
    StartCoroutine(RepopulateBlocksList());
    
}
    // Function to apply damage to the block
public void TakeDamage(float damage)
{
    StartCoroutine(TakeDamageCoroutine(damage));
}

private IEnumerator TakeDamageCoroutine(float damage)
{
    // Call UpdateBlockList and wait for it to finish
    yield return StartCoroutine(RepopulateBlocksList());

    // After UpdateBlockList is done, continue with damage logic
    if (lifeLink && allBlocks.Count > 0)
    {
        // Divide the damage equally among all blocks
        float dividedDamage = damage / allBlocks.Count;

        foreach (BlockScript block in allBlocks)
        {
            block.ApplyDamage(dividedDamage);
        }
    }
    else
    {
        ApplyDamage(damage);
    }
}
private void ApplyDamage(float damage)
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
        // Display the current health value based on its value
        if (blockHealth > 10)
        {
            healthText.text = blockHealth.ToString("F0");
        }
        else
        {
            healthText.text = blockHealth.ToString("F1");
        }
    }
}

    // Function to destroy the block
    private void DestroyBlock()
    {
        // Add any additional logic here (e.g., sound effects, animations, etc.)
        Destroy(gameObject); // Destroys the block GameObject
    }




public void CreateLifeLinkVisuals(List<BlockScript> allBlocksLocal)
{

    // List to store blocks that have lifeLink = true
    List<BlockScript> lifeLinkedBlocks = new List<BlockScript>();

    // Populate the list with blocks that have lifeLink = true
    foreach (BlockScript block in allBlocksLocal)
    {
        if (block.lifeLink)
        {
            lifeLinkedBlocks.Add(block);
        }
    }

    // Create a single chain of connections
    for (int i = 0; i < lifeLinkedBlocks.Count - 1; i++)
    {
        // Draw a line between the current block and the next block
        DrawLineBetweenBlocks(lifeLinkedBlocks[i].gameObject, lifeLinkedBlocks[i + 1].gameObject);
    }
}

// Draw a line between two blocks
private void DrawLineBetweenBlocks(GameObject block1, GameObject block2)
{
    LineRenderer lineRenderer = block1.GetComponent<LineRenderer>();

    if (lineRenderer == null) return; // Make sure the block has a LineRenderer

    // Set up the LineRenderer to draw a line between the two blocks
    lineRenderer.positionCount = 2;
    lineRenderer.SetPosition(0, block1.transform.position);
    lineRenderer.SetPosition(1, block2.transform.position);
}



}
