using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SphereCollector : MonoBehaviour
{
    [SerializeField] private GameObject spherePrefab; // Assign the prefab in the inspector

    private void Start()
    {
        InitializeSphere();
    }

    public void InitializeSphere()
    {
        // Destroy all spheres with the tag "Sphere"
        DestroyAllSpheres();

        // Instantiate a new sphere from the prefab as a child of this GameObject
        GameObject newBall = Instantiate(spherePrefab, GetRandomSpawnPosition(), Quaternion.identity, transform);
    }

    private void DestroyAllSpheres()
    {

    StartCoroutine(DestroyBalls());

    }
    private IEnumerator DestroyBalls()
        {
        // Find all GameObjects with the tag "Sphere"
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

        // Loop through the found spheres and destroy them
        foreach (GameObject ball in balls)
        {
            Destroy(ball);
        }
            // Wait until the end of the frame to ensure it's destroyed
            yield return new WaitForEndOfFrame();
        }
    private Vector3 GetRandomSpawnPosition()
    {
        // Generate a random position for the sphere (customize as needed)
        // For example, to spawn within a defined range:
        float x = Random.Range(-5f, 5f); // Adjust range as needed
        float y = 1f; // Height where the sphere should spawn
        float z = Random.Range(-5f, 5f); // Adjust range as needed

        return new Vector3(x, y, z);
    }
}
