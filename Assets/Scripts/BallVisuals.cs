using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Animations;
using System.Collections.Generic;
using System.Collections;
public class BallVisuals : MonoBehaviour
{
    public float splashDamageRadius; // The radius of the splash damage
    private Material transparentMaterial; // The transparent material for the sphere

    void Start()
    {
        splashDamageRadius=transform.GetComponent<SphereController>().splashDamageRadius;
        // Initialize and create the transparent material
        CreateTransparentMaterial();
    }
void Update(){
}
    // Call this function when you want to show the splash damage radius (e.g., when aiming)
public IEnumerator SplashEffect()
{
    // Create a new sphere for each splash effect
    GameObject newRadiusIndicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    newRadiusIndicator.transform.position = transform.position; // Set to the same position as the object

    // Disable the sphere's collider as it's only for visualization
    Destroy(newRadiusIndicator.GetComponent<SphereCollider>());

    // Assign the transparent material to the sphere's renderer
    newRadiusIndicator.GetComponent<Renderer>().material = transparentMaterial;

    // Set initial scale to zero (no visible sphere initially)
    newRadiusIndicator.transform.localScale = Vector3.zero;

    float elapsedTime = 0f;
    float duration = 0.2f; // Time to expand the radius over 1 second
    Vector3 targetScale = Vector3.one * splashDamageRadius * 2; // Target scale based on splash radius

    // Gradually increase the scale of the sphere over 1 second
    while (elapsedTime < duration)
    {
        float progress = elapsedTime / duration;
        newRadiusIndicator.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, progress);

        elapsedTime += Time.deltaTime;
        yield return null; // Wait for the next frame
    }

    // Ensure the final scale is exactly the target scale
    newRadiusIndicator.transform.localScale = targetScale;

    // Wait for another 0.1 seconds before destroying the effect
    yield return new WaitForSeconds(0.03f);

    // Destroy the sphere to remove the effect
    Destroy(newRadiusIndicator);
}



    // Create a transparent material for the sphere
    private void CreateTransparentMaterial()
    {
        // Create a new material with a transparent shader
        transparentMaterial = new Material(Shader.Find("Standard"));
        
        // Set the color with an alpha value to make it transparent (RGBA format)
        transparentMaterial.color = new Color(1f, 0f, 0f, 0.3f); // Red color with 30% opacity
        
        // Enable transparency mode on the material
        transparentMaterial.SetFloat("_Mode", 3); // Set rendering mode to transparent
        transparentMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        transparentMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        transparentMaterial.SetInt("_ZWrite", 0);
        transparentMaterial.DisableKeyword("_ALPHATEST_ON");
        transparentMaterial.EnableKeyword("_ALPHABLEND_ON");
        transparentMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        transparentMaterial.renderQueue = 3000;
    }


    //on death effect
    public Material ballMaterial; // Assign the ball's material here in the Inspector if needed.
    public float shatterDuration = 2.0f; // How long the shattering particles will last.
    public int particleCount = 100; // Number of shattering pieces.

    // Call this function on the ball's death
    public void OnDeathEffect()
    {
        GameObject shatterEffect = CreateShatterEffect();
        StartCoroutine(DestroyEffectAfterDuration(shatterEffect, shatterDuration));
    }

    private GameObject CreateShatterEffect()
    {
        // Create a new GameObject for the particle system.
        GameObject shatterEffectObject = new GameObject("ShatterEffect");

        // Position the shatter effect at the ball's location.
        shatterEffectObject.transform.position = transform.position;

        // Add a Particle System component.
        ParticleSystem particleSystem = shatterEffectObject.AddComponent<ParticleSystem>();

        // Stop the particle system before setting properties.
        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // Configure the particle system.
        var mainModule = particleSystem.main;
        mainModule.duration = shatterDuration;       // Set the duration.
        mainModule.startLifetime = shatterDuration;  // Lifetime of each particle.
        mainModule.startSpeed = 2.0f;                // Initial speed of particles.
        mainModule.startSize = 1f;                 // Size of each particle.
        mainModule.gravityModifier = 1.0f;           // Apply gravity to particles.
        mainModule.maxParticles = particleCount;     // Number of particles.
        mainModule.loop = false;                     // No looping.

        // Set the material for the particle system.
        var rendererModule = particleSystem.GetComponent<ParticleSystemRenderer>();
        rendererModule.material = ballMaterial;

        // Configure the emission for a burst of particles.
        var emissionModule = particleSystem.emission;
        emissionModule.rateOverTime = 0; // No continuous emission.
        emissionModule.SetBursts(new ParticleSystem.Burst[] {
            new ParticleSystem.Burst(0f, particleCount) // Emit all particles in one burst.
        });

        // Configure the particle shape to explode outward from a sphere.
        var shapeModule = particleSystem.shape;
        shapeModule.shapeType = ParticleSystemShapeType.Sphere;
        shapeModule.radius = 0.5f; // Adjust the radius to match the ball size.

        // Configure velocity over lifetime so particles fall downward (-y direction).
        var velocityOverLifetimeModule = particleSystem.velocityOverLifetime;
        velocityOverLifetimeModule.enabled = true;
        velocityOverLifetimeModule.space = ParticleSystemSimulationSpace.Local;

        // Set downward velocity using MinMaxCurve for all axes.
        velocityOverLifetimeModule.x = new ParticleSystem.MinMaxCurve(0f, 1f);  // Slight horizontal movement.
        velocityOverLifetimeModule.y = new ParticleSystem.MinMaxCurve(-5f, -2f); // Downward velocity.
        velocityOverLifetimeModule.z = new ParticleSystem.MinMaxCurve(0f, 1f);  // Slight forward/backward movement.

        // Start the particle system.
        particleSystem.Play();

        return shatterEffectObject;
    }

    // Coroutine to wait and destroy the effect after its duration.
    private IEnumerator DestroyEffectAfterDuration(GameObject effectObject, float duration)
    {
        // Wait for the effect's duration.
        yield return new WaitForSeconds(duration);

        // Stop and clear the particle system.
        ParticleSystem particleSystem = effectObject.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        // Destroy the particle system GameObject.
        Destroy(effectObject);
    }
}
