using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSpeeds : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed;
    public float rotationSpeed = 100f;

    [Header("Launch Settings")]
    public float launchAngle = 45f;  // Angle in degrees
    private float launchSpeed = 5f;  // Initial speed
    public float deleteDelay = 5f;

    [Header("Direction Settings")]
    public bool flyRight = true;     // Boolean to fly right
    public bool flyLeft = false;     // Boolean to fly left

    private Rigidbody rb;
    private bool launched = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
    }

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
     
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
}
