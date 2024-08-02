using UnityEngine;

public class WallBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a knife with the tag "Projectile"
        if (other.CompareTag("Projectile"))
        {
            StickKnife(other);
        }
    }

    private void StickKnife(Collider knife)
    {
        // Get the Rigidbody of the knife
        Rigidbody knifeRigidbody = knife.GetComponent<Rigidbody>();

        if (knifeRigidbody != null)
        {
            // Stop the knife's movement and rotation
            knifeRigidbody.velocity = Vector3.zero;
            knifeRigidbody.angularVelocity = Vector3.zero;
            knifeRigidbody.isKinematic = true; // Make the Rigidbody unaffected by physics
            knifeRigidbody.constraints = RigidbodyConstraints.FreezePosition;

            // Position the knife at the collision point
            //Vector3 collisionPoint = knife.ClosestPoint(transform.position);

            //// Adjust the knife's position to appear embedded in the wall
            //knife.transform.position = collisionPoint;
            //knife.transform.rotation = Quaternion.LookRotation(-transform.forward);

            //// Optionally, offset the knife's position slightly to appear embedded
            //knife.transform.position += knife.transform.forward * 0.1f; // Adjust the value as needed
        }
    }
}
