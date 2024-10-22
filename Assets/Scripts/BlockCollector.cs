using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollector : MonoBehaviour
{
    // Prefab of the block to be instantiated
    public GameObject blockPrefab;

    // Number of blocks to spawn
    public int numberOfBlocksY = 5;
    public int numberOfBlocksX = 5;

    // Spacing between the blocks
    public float blockSpacingX = 23f;
    public float blockSpacingY = 46f;

    // Manually define the starting position in code
    private Vector3 startingPosition = new Vector3(1333, -17, 0); // Example starting position at (0, 0, 0)

    // Function to spawn blocks
    public void SpawnBlocks()
    {
        // Destroy any previous blocks if necessary
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Loop through and spawn blocks
        for (int y = 0; y < numberOfBlocksY; y++)
        for (int x = 0; x < numberOfBlocksX; x++)
        {
            // Calculate the position for the next block
            Vector3 spawnPosition = startingPosition - new Vector3(x * blockSpacingX, -y * blockSpacingY, 0);

            // Instantiate the block at the calculated position
            GameObject newBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);

            // Set the block as a child of the BlockCollector object
            newBlock.transform.SetParent(transform);

            // Optionally, give the new block a unique name
            newBlock.name = "Block";
        }
    }

    // Optional: Call the SpawnBlocks function automatically when the game starts
    void Start()
    {
        SpawnBlocks();
    }
}
