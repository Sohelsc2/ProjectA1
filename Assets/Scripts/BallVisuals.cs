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
        Destroy(shatterEffect, shatterDuration); // Automatically destroy the effect after its lifetime
    }

    private GameObject CreateShatterEffect()
    {
        // Create a new GameObject to hold the Particle System
        GameObject shatterEffectObject = new GameObject("ShatterEffect");

        // Position the shatter effect where the ball was
        shatterEffectObject.transform.position = transform.position;

        // Add a Particle System component
        ParticleSystem particleSystem = shatterEffectObject.AddComponent<ParticleSystem>();

        // Configure the particle system
        var mainModule = particleSystem.main;
        mainModule.duration = shatterDuration;       // How long the particle system runs
        mainModule.startLifetime = shatterDuration;  // How long each particle lives
        mainModule.startSpeed = 2.0f;                // Initial speed of the particles
        mainModule.startSize = 0.1f;                 // Size of the particles (adjust as needed)
        mainModule.gravityModifier = 1.0f;           // Gravity effect on particles, to make them fall
        mainModule.maxParticles = particleCount;     // Number of particles to emit

        // Set the material for the particle system to use the ball's material
        var rendererModule = particleSystem.GetComponent<ParticleSystemRenderer>();
        rendererModule.material = ballMaterial; // Assuming the ball material is already assigned

        // Configure the emission to emit a burst of particles
        var emissionModule = particleSystem.emission;
        emissionModule.rateOverTime = 0; // No continuous emission
        emissionModule.SetBursts(new ParticleSystem.Burst[] {
            new ParticleSystem.Burst(0f, particleCount) // Emit all particles in one burst
        });

        // Configure the particle shape to explode outwards from a sphere
        var shapeModule = particleSystem.shape;
        shapeModule.shapeType = ParticleSystemShapeType.Sphere;
        shapeModule.radius = 0.5f; // Adjust to match the size of the ball

        // Add some randomness to the velocity of the particles
        var velocityOverLifetimeModule = particleSystem.velocityOverLifetime;
        velocityOverLifetimeModule.enabled = true;
        velocityOverLifetimeModule.space = ParticleSystemSimulationSpace.Local;
        velocityOverLifetimeModule.y = new ParticleSystem.MinMaxCurve(-2f, -5f); // Particles fall downwards (-y)

        // Start the particle system
        particleSystem.Play();

        return shatterEffectObject;
    }
}
