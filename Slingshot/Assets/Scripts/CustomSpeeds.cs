using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSpeeds : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
    public float rotationSpeed = 100f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();


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
        //transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
     
        // Set the Rigidbody's velocity to the desired value
        if (rb != null)
        {
            rb.velocity = new Vector3(0, -speed/10f, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
