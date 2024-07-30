using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
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

    private Rigidbody rb;
    private bool launched = false;
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
        if (pan)
            return;
        if(xRot)
            transform.Rotate(rotationSpeed * Time.deltaTime,0, 0);
        else if (yRot)
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        else
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        //Debug.Log(meshRenderer.materials[2].color);
        // Set the Rigidbody's velocity to the desired value
        if (rb != null && !launched)
        {
            //speed += acceleration * Time.deltaTime;
            rb.velocity = new Vector3(0, -speed/10f, 0);
        }

        ScoreScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreCounter>();
        life = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Lives>();
    }

    public void inPan(bool changeColor)
    {
        rb.velocity = new Vector3(0,0,0);
        rb.useGravity = true;
        GameObject text = Instantiate(FloatingText, transform.position, Quaternion.identity);
        text.GetComponent<TextMesh>().characterSize = 0.1f;

        string randomWord;
        if (changeColor) {
            text.GetComponent<TextMesh>().color = Color.red;
            randomWord = badWords[Random.Range(0, badWords.Count)];
            text.GetComponent<TextMesh>().text = randomWord;
            ScoreScript.SubtractScore(20);
        
        }
        else
        {
            randomWord = goodWords[Random.Range(0, goodWords.Count)];
            text.GetComponent<TextMesh>().text = randomWord;
            ScoreScript.AddScore(20);
        }
        Destroy(text, 2f);
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
        //Debug.Log(currRow);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (((collision.gameObject.CompareTag("Projectile") && this.gameObject.CompareTag("Heart")) || (collision.gameObject.CompareTag("Projectile") && this.gameObject.CompareTag("Diamond"))) && !launched )
        //{
        //    if (this.gameObject.CompareTag("Diamond")) {

        //        life.AddDiamond();
        //        Destroy(collision.gameObject);
        //        Destroy(gameObject);
        //        return;
        //    }
        //    //Debug.Log("Heart fuck ball");
        //    life.AddLife();
        //    Destroy(collision.gameObject);
        //    Destroy(gameObject);
        //}
        if (collision.gameObject.CompareTag("Projectile") && !launched)
        {
            int foodlayer = this.gameObject.layer; //check layer of good foood or bad food
            ShowText(currRow, foodlayer);
            Launch();
            launched = true;
            rotationSpeed *= 15f;
            Destroy(collision.gameObject);
        }
        
    }

    public void ShowText(int layer, int foodlayer)
    {



        GameObject text = Instantiate(FloatingText, transform.position, Quaternion.identity);
        text.GetComponent<TextMesh>().characterSize = 0.08f;
        int badFood = LayerMask.NameToLayer("BadFood");
        int goodFood = LayerMask.NameToLayer("GoodFood");

       

        if (layer == Mathf.RoundToInt(Mathf.Log(window1Layer.value, 2)))
        {
            if (foodlayer == badFood)
            {

                text.GetComponent<TextMesh>().text = "50";
                ScoreScript.AddScore(50);
            }
            else if (foodlayer == goodFood)
            {

                text.GetComponent<TextMesh>().text = "Ouch!";
                text.GetComponent<TextMesh>().color = Color.red;

            }
        }
        else if (layer == Mathf.RoundToInt(Mathf.Log(window2Layer.value, 2)))
        {
            if (foodlayer == badFood)
            {
                text.GetComponent<TextMesh>().text = "40";
                ScoreScript.AddScore(40);
            }
            else if (foodlayer == goodFood)
            {

                text.GetComponent<TextMesh>().text = "Oops!";
                text.GetComponent<TextMesh>().color = Color.red;
            }
        }
        else if (layer == Mathf.RoundToInt(Mathf.Log(window3Layer.value, 2)))
        {
            if (foodlayer == badFood)
            {
                text.GetComponent<TextMesh>().text = "30";
                ScoreScript.AddScore(30);
            }
            else if (foodlayer == goodFood)
            {

                text.GetComponent<TextMesh>().text = "Wrong!";
                text.GetComponent<TextMesh>().color = Color.red;
            }
        }
        else if (layer == Mathf.RoundToInt(Mathf.Log(window4Layer.value, 2)))
        {
            if (foodlayer == badFood)
            {
                text.GetComponent<TextMesh>().text = "20";
                ScoreScript.AddScore(20);
            }
            else if (foodlayer == goodFood)
            {

                text.GetComponent<TextMesh>().text = "Ouch!";
                text.GetComponent<TextMesh>().color = Color.red;
            }
        }
        else if (layer == Mathf.RoundToInt(Mathf.Log(window5Layer.value, 2)))
        {
            if (foodlayer == badFood)
            {
                text.GetComponent<TextMesh>().text = "10";
                ScoreScript.AddScore(10);
            }
            else if (foodlayer == goodFood)
            {

                text.GetComponent<TextMesh>().text = "Fail!";
                text.GetComponent<TextMesh>().color = Color.red;
            }
        }
        Destroy(text, 1f);
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

