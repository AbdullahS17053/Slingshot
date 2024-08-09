using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
//using UnityEngine.WSA;

public class PatternHelper : MonoBehaviour
{
    [Header("Movement Settings")]
    private float acceleration = 1.25f; // Change this value to control the rate of acceleration
    public float rotationSpeed = 100f;
    public bool xRot = false;
    public bool yRot = false;
    public bool zRot = true;
    private bool launched = false;

    [Header("Explosion Settings")]
    [SerializeField] private GameObject _replacement;
    public float explosionForceX = 15f; // Force magnitude in the x direction
    public float explosionForceY = 10f; // Force magnitude in the y direction
    public Transform cube1;      // First cube for x-position randomization
    public Transform cube2;      // Second cube for x-position randomization
    public Transform cube3;      // First cube for z-position randomization
    public Transform cube4;      // Second cube for z-position randomization

    [Header("Score Settings")]
    public GameObject FloatingText;
    private ScoreCounter ScoreScript;
    public GameObject parent;


    [Header("Sounds")]
    [SerializeField] AudioClip choiceSound;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioSource soundSource;
    public bool soundOn = false;

    private Rigidbody rb;

    // Start is called before the first frame update
    private float lerpDuration = 0.5f;
    private float bigDuration = 0.25f;// Duration for lerping to and from zero
    private float increaseAmount = 1f; // Amount to increase scale before lerping to zero

    private Vector3 originalScale;
    private Vector3 increasedScale;
    private bool lerping = false;
    private List<string> correctHitWords = new List<string>
    {
        "Wow!",
        "Nice!",
        "Great!",
        "Yay!",
        "Cool!",
        "Bingo!",
        "Sweet!",
        "Yum!",
        "Boom!",
        "Yes!"
    };

    private List<string> wrongHitWords = new List<string>
{
    "Oops!",
    "Miss!",
    "Nope!",
    "Ouch!",
    "Darn!",
    "Uh-oh!",
    "Bummer!",
    "Yikes!",
    "Whoops!",
    "Nah!"
};
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ScoreScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreCounter>();
        //originalScale = transform.localScale;
        //increasedScale = originalScale + new Vector3(increaseAmount, increaseAmount, increaseAmount);
    }

    // Update is called once per frame
    void Update()
    {
        if (!parent.GetComponent<Pattern>().playing)
            return;
        if (xRot)
            transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
        else if (yRot)
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        else
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        if (launched)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.velocity = Vector3.zero;
            StartCoroutine(LerpToNormalScaleRoutine());
            launched = false;
        }
    }

    public void spawn()
    {
        originalScale = transform.localScale;
        increasedScale = originalScale + new Vector3(increaseAmount, increaseAmount, increaseAmount);
        StartCoroutine(LerpToNormalScaleRoutine());
    }

    public void selected()
    {
        StartCoroutine(selectedRoutine());
        //while (lerping);
    }

    IEnumerator selectedRoutine()
    {
        lerping = true;

        yield return StartCoroutine(LerpScale(originalScale, increasedScale, lerpDuration));
        yield return StartCoroutine(LerpScale(increasedScale,originalScale, lerpDuration));
        
        lerping = false;
       
    }

    IEnumerator LerpToNormalScaleRoutine()
    {
        lerping = true;

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
    public void explode()
    {
        int foodlayer = this.gameObject.layer; //check layer of good foood or bad food
        //rotationSpeed *= 15f;
        var replacement = Instantiate(_replacement, transform.parent);
        Destroy(replacement, 10f);
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
        soundSource.clip = hurtSound;
        soundSource.Play();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!parent.GetComponent<Pattern>().playing)
            return;
        if (collision.gameObject.CompareTag("Projectile"))
        {
            launched = true;
            explode();

            parent.GetComponent<Pattern>().childHit(this.gameObject);
        }
    }

    public void ShowText(bool correctPattern)
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
        if( correctPattern)
        {
            string randomWord = correctHitWords[UnityEngine.Random.Range(0, correctHitWords.Count)];

            textComponent.text = randomWord + "50!";
            ScoreScript.AddScore(50);
        }
        else
        {
            string randomWord = wrongHitWords[UnityEngine.Random.Range(0, correctHitWords.Count)];

            textComponent.text = randomWord;
        }


        // Destroy the text object after a short delay
        Destroy(textObject, 1f);
    }
}
