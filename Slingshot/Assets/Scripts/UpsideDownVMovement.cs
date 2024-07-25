using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UpsideDownVMovement : MonoBehaviour
{
    public float X = 2f;
    public float Y = 6f; // Initial upward velocity
    private Rigidbody rb;
    
    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

        // Apply the initial velocity upwards
        rb.velocity = new Vector3(X, Y, 0);
    }
}
