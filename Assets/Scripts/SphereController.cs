using UnityEngine;

public class SphereController : MonoBehaviour
{
    public float speed = 5f;  // Speed of the sphere
    private Rigidbody rb;

    private Vector3 direction;

    // Control the randomness in reflection
    public float reflectionAngleVariation = 5f; // Angle variation in degrees

    void Start()
    {
        // Get the Rigidbody component for applying forces
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // Set the initial direction of movement to a random direction in the X and Y plane
        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;  
        rb.velocity = direction * speed;
    }

    void Update()
    {
        // Ensure the Z coordinate of the sphere remains 0
        Vector3 position = transform.position;
        position.z = 0; // Set Z to 0
        transform.position = position; // Update the position
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Reflect the direction vector based on the normal of the surface hit
            Vector3 reflectedDirection = Vector3.Reflect(direction, collision.contacts[0].normal);

            // Apply a slight random variation to the reflection
            reflectedDirection = AddRandomAngle(reflectedDirection, reflectionAngleVariation);

            // Update the direction and apply the new velocity
            direction = reflectedDirection.normalized;
            rb.velocity = direction * speed;
        }
    }

    // Function to add a random angle variation to the reflection direction
    private Vector3 AddRandomAngle(Vector3 originalDirection, float maxAngleVariation)
    {
        // Generate a random angle between -maxAngleVariation and +maxAngleVariation
        float randomAngle = Random.Range(-maxAngleVariation, maxAngleVariation);

        // Calculate the new direction by rotating around the Z-axis only
        float cosAngle = Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        float sinAngle = Mathf.Sin(randomAngle * Mathf.Deg2Rad);

        // Create the new direction in the X-Y plane
        Vector3 newDirection = new Vector3(
            originalDirection.x * cosAngle - originalDirection.y * sinAngle,
            originalDirection.x * sinAngle + originalDirection.y * cosAngle,
            0 // Ensure Z is always 0
        );

        return newDirection.normalized; // Return normalized direction
    }
}
