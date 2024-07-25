using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class CustomSpeeds : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed;
    private float acceleration = 1.25f; // Change this value to control the rate of acceleration
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

    public GameObject FloatingText;
    private Vector3 initialContactPosition;
    private bool hasCollided = false;
    private ScoreCounter ScoreScript;
    private Lives life;

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
            //rb.useGravity = false;
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
            speed += acceleration * Time.deltaTime;
            rb.velocity = new Vector3(0, -speed/10f, 0);
        }

        ScoreScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreCounter>();
        life = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Lives>();
        if (gameObject.transform.position.y < 0.23f && !gameObject.CompareTag("Heart")) {
            life.LifeLost();
            Destroy(gameObject);
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
            Destroy(gameObject , 0.1f);
        }
        else if (flyLeft)
        {
            launchDirection.x = -Mathf.Abs(launchDirection.x); // Ensure negative x direction
            Destroy(gameObject, 0.1f);
        }

        // Apply the launch velocity
        rb.useGravity = true;
        gameObject.GetComponent<Collider>().enabled = false;
        rb.velocity = launchDirection * launchSpeed;
        StartCoroutine(DeleteAfterDelay());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile") && this.gameObject.CompareTag("Heart") && !launched)
        {
            Debug.Log("Heart fuck ball");
            life.AddLife();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Projectile") && !launched)
        {
            initialContactPosition = transform.position;
            ShowText(initialContactPosition);
            Launch();
            launched = true;
            rotationSpeed *= 15f;
            Destroy(collision.gameObject);
        }
        
    }

    public void ShowText(Vector3 position) {

        

        GameObject text =  Instantiate(FloatingText, position, Quaternion.identity);
        text.GetComponent<TextMesh>().characterSize = 0.08f;
        if (position.y > 1.30f)
        {
            text.GetComponent<TextMesh>().text = "50";
            ScoreScript.AddScore(50);
        }
        else if (position.y > 1.09f && position.y < 1.30f)
        {

            text.GetComponent<TextMesh>().text = "40";
            ScoreScript.AddScore(40);
        }
        else if (position.y > 0.82f && position.y < 1.09f)
        {

            text.GetComponent<TextMesh>().text = "30";
            ScoreScript.AddScore(30);
        }
        else if (position.y > 0.54f && position.y < 0.82f)
        {

            text.GetComponent<TextMesh>().text = "20";
            ScoreScript.AddScore(20);

        }
        else if (position.y > 0.225f && position.y < 0.54f)
        {
            text.GetComponent<TextMesh>().text = "10";
            ScoreScript.AddScore(10);

        }
        

        Destroy(text, 1f);
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
                //Debug.Log(inFilled);
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

