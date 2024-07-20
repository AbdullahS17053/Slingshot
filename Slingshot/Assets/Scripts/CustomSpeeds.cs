using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSpeeds : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;

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
        // Set the Rigidbody's velocity to the desired value
        if (rb != null)
        {
            rb.velocity = new Vector3(0, -speed/10f, 0);
        }
    }
}
