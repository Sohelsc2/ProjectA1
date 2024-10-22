using UnityEngine;
using System.Collections.Generic;

public class BlockCollector : MonoBehaviour
{
    // Prefab of the block to be instantiated
    public GameObject blockPrefab;

    // Total number of blocks to spawn
    public int totalBlocksToSpawn = 10;

    // Define two areas (bottom-left and top-right corners)
    public Vector3 area1Min = new Vector3(680, 110, 0); // Bottom-left corner of area 1
    public Vector3 area1Max = new Vector3(1320, 180, 0);   // Top-right corner of area 1

    public Vector3 area2Min = new Vector3(680, -30, 0); // Bottom-left corner of area 2
    public Vector3 area2Max = new Vector3(1320, 40, 0);   // Top-right corner of area 2

    // Function to spawn blocks
    public void SpawnBlocks()
    {
        // Destroy any previous blocks if necessary
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // List to hold the positions of spawned blocks
        List<Vector3> spawnedPositions = new List<Vector3>();

        // Loop until the desired number of blocks are spawned
        while (spawnedPositions.Count < totalBlocksToSpawn)
        {
            int attempts = 0; // Attempt counter

            // Attempt to find a valid position
            while (attempts < 100)
            {
                // Choose a random area
                float randomValue = Random.value;
                Vector3 randomAreaMin = randomValue > 0.5f ? area1Min : area2Min;
                Vector3 randomAreaMax = randomValue > 0.5f ? area1Max : area2Max;

                // Generate a random position within the selected area
                Vector3 randomPosition = new Vector3(
                    Random.Range(randomAreaMin.x, randomAreaMax.x),
                    Random.Range(randomAreaMin.y, randomAreaMax.y),
                    0
                );

                // Check if the position is valid (not overlapping with existing blocks)
                if (IsPositionValid(randomPosition, spawnedPositions))
                {
                    // Instantiate the block at the valid random position
                    GameObject newBlock = Instantiate(blockPrefab, randomPosition, Quaternion.Euler(0, 0, 90));
                    AddLineRendererToBlock(newBlock);
                    // Set the block as a child of the BlockCollector object
                    newBlock.transform.SetParent(transform);

                    // Optionally, give the new block a unique name
                    newBlock.name = "Block_" + spawnedPositions.Count;

                    // Add the position to the list of spawned positions
                    spawnedPositions.Add(randomPosition);
                    break; // Exit the attempt loop if a block was successfully spawned
                }

                attempts++; // Increment the attempt counter
            }

            // If 100 attempts were made and no valid position was found, exit the spawning process
            if (attempts >= 100)
            {
                Debug.LogWarning("Could not find a valid spawn position after 100 attempts.");
                break; // Exit the while loop
            }
        }
    }
    private void AddLineRendererToBlock(GameObject block)
{
    if (!block.TryGetComponent(out LineRenderer lineRenderer))
    {
        lineRenderer = block.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 1f; // Set the width of the line
        lineRenderer.endWidth = 1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Create a material for the line
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }
}
    // Check if the new position is valid (not too close to existing blocks)
    private bool IsPositionValid(Vector3 position, List<Vector3> existingPositions)
    {
        float minDistance = 50.0f; // Minimum distance to prevent overlap (adjust as needed)

        foreach (Vector3 existingPosition in existingPositions)
        {
            // Check the distance to existing positions
            if (Vector3.Distance(position, existingPosition) < minDistance)
            {
                return false; // Position is too close to an existing block
            }
        }
        return true; // Position is valid
    }

    // Optional: Call the SpawnBlocks function automatically when the game starts
    void Start()
    {
        SpawnBlocks();
    }
}
