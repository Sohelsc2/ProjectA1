using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SphereCollector : MonoBehaviour
{
    [SerializeField] private GameObject spherePrefab; // Assign the prefab in the inspector
    public Vector3 targetPosition; // Define the target position in the inspector

    private void Start()
    {
        InitializeSphere();
    }
    public void OnFightStart(){
                // Find all GameObjects with the tag "Sphere"
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

        // Loop through the found spheres and destroy them
        foreach (GameObject ball in balls)
        {
            StartCoroutine(ball.GetComponent<SphereController>().TemporaryBoostSpeed());
        }
        
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
        public void MoveBallsToTarget()
    {
        StartCoroutine(MoveBallsCoroutine());
    }


private IEnumerator MoveBallsCoroutine()
{
    // Find all GameObjects with the tag "Sphere"
    GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

    // Loop through each ball and move them one by one
    foreach (GameObject ball in balls)
    {
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Get the current speed of the ball
            float speed = rb.velocity.magnitude;

            // Set the direction to positive x and add a random angle in the y direction
            Vector3 direction = new Vector3(1, 0, 0); // Positive x direction
            float randomAngle = Random.Range(-180f, 180f); // Random angle in degrees
            ball.transform.position = targetPosition;
            float angleInRadians = randomAngle * Mathf.Deg2Rad;

            // Calculate the new direction using the random angle
            direction = new Vector3(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians), 0); // Only modify x and y

            // Set the new velocity while maintaining the speed
            rb.velocity = direction.normalized * speed;

            // Normalize the direction and set the new velocity
        }

        // Wait
        yield return new WaitForSeconds(0.001f);
    }
}

}
