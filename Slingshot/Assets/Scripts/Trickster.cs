using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static System.Net.Mime.MediaTypeNames;

public class Trickster : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed;
    private float acceleration = 1.25f; // Change this value to control the rate of acceleration
    public float rotationSpeed = 100f;
    public bool xRot = false;
    public bool yRot = false;
    public bool zRot = true;
    public float fadeDuration = 1.0f;

    [Header("Trickster")]
    public float health = 30f;
    public float hitValue = 10f;
    public GameObject[] colorWindows;
    GameObject targetWindow;
    GameObject currentWindow;
    private List<GameObject> selectedWindows; // Stores the original open windows
    private List<GameObject> instantiatedWindows; // Stores the instantiated color windows

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
    private float lerpDuration = 0.5f;
    private float bigDuration = 0.25f;// Duration for lerping to and from zero
    private float increaseAmount = 1f; // Amount to increase scale before lerping to zero

    private Vector3 originalScale;
    private Vector3 increasedScale;
    private bool lerping = false;

    private bool hasBeenTeleported = false;
    public GameManager gameManager;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();

        goodWords = new List<string> { "Yummy", "Delicious", "Tasty", "Juicy", "Sweet" };
        badWords = new List<string> { "Yuck", "Gross", "Eww", "Disgusting", "Nasty" };

        ScoreScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreCounter>();
        life = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Lives>();
        originalScale = transform.localScale;
        increasedScale = originalScale + new Vector3(increaseAmount, increaseAmount, increaseAmount);

        StartCoroutine(InitialDelay());
    }

    public void Move()
    {
        // Find all open windows
        GameObject[] openWindows = GameObject.FindGameObjectsWithTag("Window");

        // Ensure there is at least one open window
        if (openWindows.Length == 0)
        {
            //Debug.LogWarning("No open windows to move to!");
            return;
        }

        // Randomly select an open window
        GameObject selectedWindow = openWindows[Random.Range(0, openWindows.Length)];

        currentWindow = selectedWindow;

        // Move this GameObject's position to the selected window's position
        transform.position = selectedWindow.transform.position;

        //Debug.Log($"Moved to window at position: {transform.position}");
    }

    public void spawnWindows()
    {
        GameObject[] openWindows = GameObject.FindGameObjectsWithTag("Window");

        // Check if there are enough open windows to choose from
        if (openWindows.Length == 0)
        {
            //Debug.LogWarning("No open windows found!");
            return;
        }

        openWindows = openWindows.Where(window => window != currentWindow).ToArray();

        // Determine the number of windows to color (ensure it's > 0 and <= colorWindows.Length)
        int numberToColor = Mathf.Min(openWindows.Length, colorWindows.Length);

        // Randomly select open windows
        selectedWindows = new List<GameObject>();
        while (selectedWindows.Count < numberToColor)
        {
            GameObject randomWindow = openWindows[Random.Range(0, openWindows.Length)];
            if (!selectedWindows.Contains(randomWindow))
            {
                selectedWindows.Add(randomWindow);
            }
        }

        // Instantiate color windows at the selected open window positions
        instantiatedWindows = new List<GameObject>();
        int index = 0;
        foreach (GameObject window in selectedWindows)
        {
            Vector3 position = window.transform.position;
            Quaternion rotation = window.transform.rotation;

            // Instantiate a random color window
            GameObject randomColorWindow = colorWindows[index];
            index++;
            GameObject instantiatedWindow = Instantiate(randomColorWindow, position, rotation);
            instantiatedWindow.transform.SetParent(window.transform.parent);
            instantiatedWindow.transform.localScale = window.transform.localScale;
            instantiatedWindows.Add(instantiatedWindow);

            // Disable the open window
            window.SetActive(false);
        }

        // Randomly pick one of the instantiated windows to be the target window
        targetWindow = instantiatedWindows[Random.Range(0, instantiatedWindows.Count)];

        // Instantiate the target window again at its position
        GameObject curr = Instantiate(targetWindow, transform.position, transform.rotation);
        curr.transform.SetParent(currentWindow.transform.parent);
        curr.transform.localScale = currentWindow.transform.localScale;
        curr.transform.rotation = currentWindow.transform.rotation;
        instantiatedWindows.Add(curr);
        currentWindow.SetActive(false);

        targetWindow.AddComponent<TricksterHelper>();
        targetWindow.GetComponent<TricksterHelper>().parent = this.gameObject;

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void Hit()
    {
        health -= hitValue;
        
        foreach (GameObject window in selectedWindows)
        {
            window.SetActive(true);
        }
        currentWindow.SetActive(true);
        // Clear the list of selected windows
        selectedWindows.Clear();

        // Delete all instantiated color windows
        foreach (GameObject window in instantiatedWindows)
        {
            Destroy(window);
        }

        // Clear the list of instantiated windows
        instantiatedWindows.Clear();

        if(health > 0)
        {
            // Start the scale lerping coroutine
            StartCoroutine(LerpToZeroScaleRoutine());
            //meshRenderer.enabled = false;
            StartCoroutine(RemoveWindowAfterDelay());
        }
        else
        {
            death();
        }
    }

    IEnumerator RemoveWindowAfterDelay()
    {
        yield return new WaitForSeconds(lerpDuration + bigDuration);
        Move();
        StartCoroutine(LerpToNormalScaleRoutine());
        yield return new WaitForSeconds(lerpDuration + bigDuration);
        spawnWindows();
    }

    IEnumerator InitialDelay()
    {
        Move();
        StartCoroutine(LerpToNormalScaleRoutine());
        yield return new WaitForSeconds(lerpDuration + bigDuration);
        spawnWindows();
    }

    IEnumerator LerpToZeroScaleRoutine()
    {
        lerping = true;
        // Lerp to slightly increased scale
        yield return StartCoroutine(LerpScale(transform.localScale, increasedScale, bigDuration));

        // Lerp to zero
        yield return StartCoroutine(LerpScale(increasedScale, Vector3.zero, lerpDuration));
      
    }

    IEnumerator LerpToNormalScaleRoutine()
    {

        // Lerp back to slightly increased scale
        yield return StartCoroutine(LerpScale(Vector3.zero, increasedScale, lerpDuration));

        // Lerp back to original scale
        yield return StartCoroutine(LerpScale(increasedScale, originalScale, bigDuration));
        lerping = false;
    }

    IEnumerator LerpScale(Vector3 startScale, Vector3 endScale, float duration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            // Calculate the lerp value
            float lerpValue = Mathf.Clamp01(timeElapsed / duration);

            // Lerp the scale
            transform.localScale = Vector3.Lerp(startScale, endScale, lerpValue);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is exactly the endScale
        transform.localScale = endScale;
    }




    public void death()
    {
        int foodlayer = this.gameObject.layer; //check layer of good foood or bad food
        ShowText(currRow, foodlayer);
        launched = true;
        rotationSpeed *= 15f;
        var replacement = Instantiate(_replacement, transform.parent);
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
            //Debug.LogError("Not enough cubes found with specified tags.");
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
        rb.velocity = new Vector3(0, 0, 0);
        this.gameObject.tag = "WindowObject";
        this.gameObject.GetComponent<Collider>().isTrigger = false;
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
            //Debug.LogError("TextMeshProUGUI component not found on the prefab.");
            return;
        }

        // Set the size and color based on the context
        textComponent.fontSize = 2.5f; // Adjust font size as needed

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

    private void OnTriggerEnter(Collider other)
    {
        currRow = other.gameObject.layer;
    }

    public void ShowText(int layer, int foodlayer)
    {



        GameObject textObject = Instantiate(FloatingText, transform.position, Quaternion.identity);

        // Get the TMP_Text component
        TMP_Text textComponent = textObject.GetComponent<TMP_Text>();
        textComponent.fontSize = 2;

        if (textComponent == null)
        {
            //Debug.LogError("TMP_Text component not found on the prefab.");
            return;
        }


        textComponent.text = "Boss! 100!";
        ScoreScript.AddScore(100);

        // Destroy the text object after a short delay
        Destroy(textObject, 1f);
    }

    public void Teleport(Vector3 position, Quaternion rotation)
    {

        if (hasBeenTeleported) return;

        transform.position = position;
        Physics.SyncTransforms();

        hasBeenTeleported = true;
    }

}