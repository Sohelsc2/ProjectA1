using UnityEngine;
using System.Collections;
public class SphereController : MonoBehaviour
{
    public float speed = 5f;  // Speed of the sphere
    private Rigidbody rb;
    public bool fightHasStarted = false;

    private Vector3 direction;
    public float damage = 1f;
    public int numberOfDamage=2;
    // Control the randomness in reflection
    public float reflectionAngleVariation = 5f; // Angle variation in degrees
    //SpeedLust perk
    public bool speedLust = false;
    public float speedLustFactor = 1;
    //TemporarySpeed Perk:
    public bool temporarySpeed = false;
    public float temporarySpeedFactor = 1;    
    public float temporarySpeedDuration = 0; 
    //WildMovement
    public bool wildMovement = false;
    public float wildMovementInterval = 1;
    //PowerOfChaos
    public bool powerOfChaos = false;
    public float powerOfChaosFactor = 1;
    //PowerOfSpeed
    public bool powerOfSpeed = false;
    //SplashDamage
    public bool splashDamage = false;
    public float splashDamageRadius = 100f;
    public float splashDamageFactor = 1f;

public void ApplySplashDamage()
{
    if(!splashDamage){return;}
    StartCoroutine(transform.GetComponent<BallVisuals>().SplashEffect());
    // Find all colliders within the radius
    Collider[] hitColliders = Physics.OverlapSphere(transform.position, splashDamageRadius);

    // Loop through all objects that were hit
    foreach (Collider hitCollider in hitColliders)
    {
        // Check if the object has a health component (or any other condition)
        BlockScript block = hitCollider.GetComponent<BlockScript>();
        
        if (block != null)
        {
            // Apply damage to the object
            block.TakeDamage(splashDamageFactor*damage);
        }
    }

    // Optionally, you can add a visual or sound effect here to indicate the explosion
}
    void Start()
    {
        // Get the Rigidbody component for applying forces
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // Set the initial direction of movement to a random direction in the X and Y plane
        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;  
        rb.velocity = direction * speed;
    }
    public void IncreaseSpeed(float increaseFactor, bool apply){
        speed *= increaseFactor;
        if(powerOfSpeed && apply){
            damage *= increaseFactor;
        }
    }
    public void DecreaseSpeed(float decreaseFactor){
        speed /= decreaseFactor;
    }
    public IEnumerator TemporaryBoostSpeed()
    {
        if (temporarySpeed)
        {
            if (temporarySpeedFactor > 0){
                // Increase the speed
                IncreaseSpeed(temporarySpeedFactor, true);
                UpdateVelocity();
                // Wait for the duration (3 seconds in this case)
                yield return new WaitForSeconds(temporarySpeedDuration);
                if (this == null || gameObject == null)
                    {
                        // Exit the coroutine if the object or its script is destroyed
                        yield break;
                    }
                // Reset speed to the original value
                DecreaseSpeed(temporarySpeedFactor);
                UpdateVelocity();
            }
            else
            {
                float originalSpeed = speed;
                speed = 0;
                UpdateVelocity();
                yield return new WaitForSeconds(temporarySpeedDuration);
                // Reset speed to the original value
                if (this == null || gameObject == null)
                    {
                        // Exit the coroutine if the object or its script is destroyed
                        yield break;
                    }
                speed = originalSpeed;
                yield return null;
                UpdateVelocity();
            }
        }
    }
        public IEnumerator ChangeDirectionRoutine()
    {
        
        while (wildMovement)
        {
        if (this == null || gameObject == null)
        {
            // Exit the coroutine if the object or its script is destroyed
            yield break;
        }
            direction = AddRandomAngle(direction, 45);
            // Update the direction and apply the new velocity
            direction = direction.normalized;
            rb.velocity = direction * speed;
            // Wait for 0.2 seconds before changing the direction
            yield return new WaitForSeconds(wildMovementInterval); 
        }
    }
    void Update()
    {
        // Ensure the Z coordinate of the sphere remains 0
        Vector3 position = transform.position;
        position.z = 0; // Set Z to 0
        transform.position = position; // Update the position
        UpdateVelocity();

    }
    private void SpeedLust(){
        if(speedLust){
            IncreaseSpeed(speedLustFactor, fightHasStarted);
            UpdateVelocity();
        }
    }
    private void ReflectBall(Collision collision){
            // Reflect the direction vector based on the normal of the surface hit
            Vector3 reflectedDirection = Vector3.Reflect(direction, collision.contacts[0].normal);

            // Apply a slight random variation to the reflection
            reflectedDirection = AddRandomAngle(reflectedDirection, reflectionAngleVariation);

            // Update the direction and apply the new velocity
            direction = reflectedDirection.normalized;
            rb.velocity = direction * speed;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            ReflectBall(collision);
        }
        else if (collision.gameObject.CompareTag("Block"))
        {
            // Handle collision with a block
            BlockScript block = collision.gameObject.GetComponent<BlockScript>();

            if (block != null)
            {
                // Decrease the block's health
                StartCoroutine(DealDamage(block));
                SpeedLust();
                ReflectBall(collision);
            }
        }
    }
    private IEnumerator DealDamage(BlockScript block)
    {
    if (numberOfDamage > 1)
    {
        block.TakeDamage(damage);
        ApplySplashDamage();
        numberOfDamage--;
    }
    else
    {
        block.TakeDamage(damage);
        ApplySplashDamage();
        Destroy(gameObject);
        yield return null; // Wait for a frame
    }
}

private void Death(){
    transform.GetComponent<BallVisuals>().OnDeathEffect();
    Destroy(gameObject);
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
        PowerOfChaosFunction();
        return newDirection.normalized; // Return normalized direction
    }
    private void PowerOfChaosFunction(){
        if(powerOfChaos && fightHasStarted){
            damage *= powerOfChaosFactor;
        }
    }
    private void UpdateVelocity(){
        if (rb.velocity.magnitude > 0){
            direction = rb.velocity.normalized;
            direction = new Vector3(direction.x, direction.y, 0);
        }
        rb.velocity = direction * speed;
    }
}
