using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSpeeds : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed;
    public float rotationSpeed = 100f;
    public float fadeDuration = 1.0f;

    [Header("Launch Settings")]
    public float launchAngle = 45f;  // Angle in degrees
    private float launchSpeed = 5f;  // Initial speed
    public float deleteDelay = 5f;
    public bool inFilled = true;

    [Header("Direction Settings")]
    public bool flyRight = true;     // Boolean to fly right
    public bool flyLeft = false;     // Boolean to fly left

    private Rigidbody rb;
    private bool launched = false;
    private MeshRenderer meshRenderer;
    Color startColor;
    Color endColor;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        // Randomly decide whether to fly right or left
        bool direction = Random.value > 0.5f;
        flyRight = direction;
        flyLeft = !direction;


        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found.");
        }
        else
        {
            // Disable the default gravity
            rb.useGravity = false;
        }

        if (meshRenderer != null && meshRenderer.materials.Length > 2)
        {
            // Start the fade coroutine for the third material
            startColor = meshRenderer.materials[2].color;
            endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
            StartCoroutine(FadeMaterialAlpha(meshRenderer.materials[2]));
        }
    }

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        //Debug.Log(meshRenderer.materials[2].color);
        // Set the Rigidbody's velocity to the desired value
        if (rb != null && !launched)
        {
            rb.velocity = new Vector3(0, -speed/10f, 0);
        }
    }

    public void Launch()
    {
        // Calculate the launch direction
        float angleInRadians = launchAngle * Mathf.Deg2Rad;
        Vector3 launchDirection = new Vector3(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians), 0);

        // Adjust direction based on the booleans
        if (flyRight)
        {
            launchDirection.x = Mathf.Abs(launchDirection.x); // Ensure positive x direction
        }
        else if (flyLeft)
        {
            launchDirection.x = -Mathf.Abs(launchDirection.x); // Ensure negative x direction
        }

        // Apply the launch velocity
        rb.useGravity = true;
        gameObject.GetComponent<Collider>().enabled = false;
        rb.velocity = launchDirection * launchSpeed;
        StartCoroutine(DeleteAfterDelay());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile") && !launched)
        {
            //Destroy(gameObject);
            Launch();
            launched = true;
            rotationSpeed *= 15f;
            Destroy(collision.gameObject);
        }
    }

    IEnumerator DeleteAfterDelay()
    {
        yield return new WaitForSeconds(deleteDelay);
        Destroy(gameObject);
    }

    private IEnumerator FadeMaterialAlpha(Material material)
    {
        // Ensure the material has a color property
        if (material.HasProperty("_Color"))
        {

            while (true)
            {
                Debug.Log(inFilled);
                // Fade out (alpha to 0)
                yield return StartCoroutine(FadeTo(material, endColor, fadeDuration));

                // Fade in (alpha to original)
                yield return StartCoroutine(FadeTo(material, startColor, fadeDuration));
            }
        }
        else
        {
            Debug.LogWarning("Material does not have a _Color property.");
        }
    }

    private IEnumerator FadeTo(Material material, Color targetColor, float duration)
    {
        Color startColor = material.color;
        float timeElapsed = 0f;

        while (timeElapsed < duration && inFilled)
        {
            timeElapsed += Time.deltaTime;
            float lerpValue = Mathf.Clamp01(timeElapsed / duration);
            material.color = Color.Lerp(startColor, targetColor, lerpValue);
            yield return null;
        }

        // Ensure the final color is exactly the target color
        if(inFilled)
            material.color = targetColor;
    }

    private IEnumerator Clear(Material material, Color targetColor, float duration)
    {
        Color startColor = material.color;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float lerpValue = Mathf.Clamp01(timeElapsed / duration);
            material.color = Color.Lerp(startColor, targetColor, lerpValue);
            yield return null;
        }

        // Ensure the final color is exactly the target color
        material.color = targetColor;
    }

    public IEnumerator isSeeThru()
    {
        inFilled = false;
        yield return StartCoroutine(Clear((meshRenderer.materials[2]), endColor, fadeDuration));
    }
}
