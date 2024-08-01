using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static System.Net.Mime.MediaTypeNames;

public class CustomSpeeds : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed;
    private float acceleration = 1.25f; // Change this value to control the rate of acceleration
    public float rotationSpeed = 100f;
    public bool xRot = false;
    public bool yRot = false;
    public bool zRot = true;
    public float fadeDuration = 1.0f;

    [Header("Launch Settings")]
    public float launchAngle = 90f;  // Angle in degrees
    private float launchSpeed = 3f;  // Initial speed
    public float deleteDelay = 5f;
    public bool inFilled = true;

    [Header("Direction Settings")]
    public bool flyRight = true;     // Boolean to fly right
    public bool flyLeft = false;     // Boolean to fly left

    [Header("Window Layers")]
    public LayerMask window1Layer;
    public LayerMask window2Layer;
    public LayerMask window3Layer;
    public LayerMask window4Layer;
    public LayerMask window5Layer;

    [Header("Explosion Settings")]
    [SerializeField] private GameObject _replacement;
    public float explosionForceX = 15f; // Force magnitude in the x direction
    public float explosionForceY = 10f; // Force magnitude in the y direction
    public Transform cube1;      // First cube for x-position randomization
    public Transform cube2;      // Second cube for x-position randomization
    public Transform cube3;      // First cube for z-position randomization
    public Transform cube4;      // Second cube for z-position randomization


    private Rigidbody rb;
    public bool launched = false;
    private MeshRenderer meshRenderer;
    Color startColor;
    Color endColor;

    public GameObject FloatingText;
    private int currRow;
    private bool hasCollided = false;
    private ScoreCounter ScoreScript;
    private Lives life;
    private Lives diamond;
    public bool pan = false;

    private List<string> goodWords;
    private List<string> badWords;

    private bool hasBeenTeleported = false;
    public GameManager gameManager;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        // Randomly decide whether to fly right or left
        bool direction = Random.value > 0.5f;
        flyRight = direction;
        flyLeft = !direction;


        goodWords = new List<string> { "Yummy", "Delicious", "Tasty", "Juicy", "Sweet" };
        badWords = new List<string> { "Yuck", "Gross", "Eww", "Disgusting", "Nasty" };


        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found.");
        }

        if (meshRenderer != null && meshRenderer.materials.Length > 2)
        {
            // Start the fade coroutine for the third material
            startColor = meshRenderer.materials[2].color;
            endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
            StartCoroutine(FadeMaterialAlpha(meshRenderer.materials[2]));
        }

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (pan || launched)
            return;
        if (xRot)
            transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
        else if (yRot)
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        else
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        //Debug.Log(meshRenderer.materials[2].color);
        // Set the Rigidbody's velocity to the desired value
        if (rb != null && !launched)
        {
            //speed += acceleration * Time.deltaTime;
            rb.velocity = new Vector3(0, -speed / 10f, 0);
        }

        ScoreScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreCounter>();
        life = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Lives>();
    }

    public void inPan(bool changeColor)
    {
        rb.velocity = new Vector3(0, 0, 0);
        rb.useGravity = true;

        GameObject textObject = Instantiate(FloatingText, transform.position, Quaternion.identity);

        // Get the TextMeshProUGUI component
        TMP_Text textComponent = textObject.GetComponent<TMP_Text>();

        if (textComponent == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on the prefab.");
            return;
        }

        // Set the size and color based on the context
        textComponent.fontSize = 36; // Adjust font size as needed

        string randomWord;
        if (changeColor)
        {
            textComponent.color = Color.red;
            randomWord = badWords[Random.Range(0, badWords.Count)];
            ScoreScript.SubtractScore(20);
        }
        else
        {
            randomWord = goodWords[Random.Range(0, goodWords.Count)];
            ScoreScript.AddScore(20);
        }

        textComponent.SetText(randomWord);
        Destroy(textComponent, 2f);
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
        gameObject.tag = "Heart";
        rb.velocity = launchDirection * launchSpeed;
        StartCoroutine(DeleteAfterDelay());

    }

    private void OnTriggerEnter(Collider other)
    {
        currRow = other.gameObject.layer;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile") && !launched)
        {
            int foodlayer = this.gameObject.layer; //check layer of good foood or bad food
            ShowText(currRow, foodlayer);
            launched = true;
            rotationSpeed *= 15f;
            var replacement = Instantiate(_replacement,transform.parent);
            replacement.transform.position = transform.position;
            replacement.transform.rotation = transform.rotation;
            var rbs = replacement.GetComponentsInChildren<Rigidbody>();
            foreach (var rb in rbs)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionZ;
                // Calculate the direction from the explosion to the object in world space
                Vector3 force;
                // Calculate the force to apply in the x and y directions only
                if (rb == rbs[0]) // First Rigidbody
                {
                    force = new Vector3(
                        -explosionForceX, // Negative x direction
                        explosionForceY,
                        0 // No force in z direction
                    );
                }
                else if (rb == rbs[1]) // Second Rigidbody
                {
                    force = new Vector3(
                        explosionForceX, // Positive x direction
                        explosionForceY,
                        0 // No force in z direction
                    );
                }
                else
                {
                    // For any other Rigidbodies, if applicable
                    force = new Vector3(
                        explosionForceX,
                        explosionForceY,
                        0
                    );
                }

                // Apply the calculated force
                rb.AddForce(force, ForceMode.Impulse);

                // Enable gravity
                rb.useGravity = true;
            }


            //Destroy(gameObject);
            transform.SetParent(null);
            rb.useGravity = true;

            GameObject[] cubesX = GameObject.FindGameObjectsWithTag("cubeX");
            GameObject[] cubesZ = GameObject.FindGameObjectsWithTag("cubeZ");

            // Check if the correct number of cubes were found
            if (cubesX.Length < 2 || cubesZ.Length < 2)
            {
                Debug.LogError("Not enough cubes found with specified tags.");
                return;
            }

            // Assign the transforms of the found cubes
            cube1 = cubesX[0].transform;
            cube2 = cubesX[1].transform;
            cube3 = cubesZ[0].transform;
            cube4 = cubesZ[1].transform;

            float randomX = Random.Range(cube1.position.x, cube2.position.x);

            // Randomize the z-position between cube3 and cube4
            float randomZ = Random.Range(cube3.position.z, cube4.position.z);

            // Use the y-position from cube1 (assuming all cubes have the same y-position)
            float yPosition = cube1.position.y;

            // Set the Food object's position to the random values
            transform.position = new Vector3(randomX, yPosition, randomZ);
            rb.velocity = new Vector3(0,0,0);
            Destroy(collision.gameObject);
        }

    }

    public void ShowText(int layer, int foodlayer)
    {



        GameObject textObject = Instantiate(FloatingText, transform.position, Quaternion.identity);

        // Get the TMP_Text component
        TMP_Text textComponent = textObject.GetComponent<TMP_Text>();

        if (textComponent == null)
        {
            Debug.LogError("TMP_Text component not found on the prefab.");
            return;
        }

        textComponent.fontSize = 4.0f; // Adjust the size as necessary

        int badFood = LayerMask.NameToLayer("BadFood");
        int goodFood = LayerMask.NameToLayer("GoodFood");

        if (layer == Mathf.RoundToInt(Mathf.Log(window1Layer.value, 2)))
        {
            if (foodlayer == goodFood)
            {
                textComponent.text = "500";
                ScoreScript.AddScore(500);
            }
            
        }
        else if (layer == Mathf.RoundToInt(Mathf.Log(window2Layer.value, 2)))
        {
            if (foodlayer == goodFood)
            {
                textComponent.text = "400";
                ScoreScript.AddScore(400);
            }
        }
        else if (layer == Mathf.RoundToInt(Mathf.Log(window3Layer.value, 2)))
        {
            if (foodlayer == goodFood)
            {
                textComponent.text = "300";
                ScoreScript.AddScore(300);
            }
        }
        else if (layer == Mathf.RoundToInt(Mathf.Log(window4Layer.value, 2)))
        {
            if (foodlayer == goodFood)
            {
                textComponent.text = "200";
                ScoreScript.AddScore(200);
            }
        }
        else if (layer == Mathf.RoundToInt(Mathf.Log(window5Layer.value, 2)))
        {
            if (foodlayer == goodFood)
            {
                textComponent.text = "100";
                ScoreScript.AddScore(100);
            }
        }

        // Destroy the text object after a short delay
        Destroy(textObject, 1f);
    }
    IEnumerator DeleteAfterDelay()
    {
        yield return new WaitForSeconds(deleteDelay);
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
        if (inFilled)
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



    public void Teleport(Vector3 position, Quaternion rotation) {

        if (hasBeenTeleported) return;

        transform.position = position;
        Physics.SyncTransforms();

        hasBeenTeleported = true;
    }
  

}



