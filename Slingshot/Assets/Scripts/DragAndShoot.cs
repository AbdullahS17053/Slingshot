using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DragAndShoot : MonoBehaviour
{
    private Vector3 mousePressDownPos;
    private Vector3 mousePressDragPos;
    private Vector3 mouseReleasePos;
    private Vector3 originalScale;

    public float forceMultiplier = 3;
    public float maxLeftDrag = -200f;
    public float maxRightDrag = 200f;
    public float maxUpDrag = 0f; // no upward drag
    public float rotationSpeed = -5000f;
    public float additionalAngle = 0f;
    public float xMultiplier = 1f;
    public float yMultiplier = 1f;
    public float zMultiplier = 1f;
    public Rigidbody rb;
    public Transform pos;
    public Collider aim;
    public Collider shoot;

    private bool isShoot;

    public delegate void ShootAction();
    public event ShootAction OnShoot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalScale = transform.localScale; // Store the original scale
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        // Ensure the Rigidbody constraints allow for rotation
        rb.constraints = RigidbodyConstraints.None;
    }

    private void OnMouseDown()
    {
        mousePressDownPos = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        if (isShoot) return;

        Vector3 directionToTarget = DrawTrajectory.Instance.windowHit - transform.position;
        if (DrawTrajectory.Instance.windowHit != Vector3.zero && directionToTarget != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            Vector3 eulerRotation = lookRotation.eulerAngles;
            eulerRotation.x += additionalAngle;
            Quaternion adjustedRotation = Quaternion.Euler(eulerRotation);
            transform.rotation = adjustedRotation;
        }

        Vector3 forceInit = (mousePressDownPos - Input.mousePosition);
        forceInit.x = Mathf.Clamp(forceInit.x, maxLeftDrag, maxRightDrag);
        forceInit.y = Mathf.Clamp(forceInit.y, float.NegativeInfinity, maxUpDrag);

        Vector3 forceV = new Vector3(-forceInit.x * xMultiplier, -forceInit.y * yMultiplier, -forceInit.y + zMultiplier) * forceMultiplier;

        DrawTrajectory.Instance.UpdateTrajectory(forceV, rb, transform.position);

        transform.localScale = originalScale; // Reapply the original scale
    }

    private void OnMouseUp()
    {
        if (isShoot) return;

        mouseReleasePos = Input.mousePosition;
        Vector3 force = (mousePressDownPos - mouseReleasePos);
        force.x = Mathf.Clamp(force.x, maxLeftDrag, maxRightDrag);
        force.y = Mathf.Clamp(force.y, float.NegativeInfinity, maxUpDrag);
        Shoot(force);

        DrawTrajectory.Instance.HideLine();
    }

    void Shoot(Vector3 force)
    {
        aim.enabled = false;
        shoot.enabled = true;
        rb.AddForce(new Vector3(-force.x * xMultiplier, -force.y * yMultiplier, -force.y + zMultiplier) * forceMultiplier);

        Vector3 directionToTarget = DrawTrajectory.Instance.windowHit - transform.position;
        if (DrawTrajectory.Instance.windowHit != Vector3.zero && directionToTarget != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            Vector3 eulerRotation = lookRotation.eulerAngles;
            eulerRotation.x += additionalAngle + 30f;
            Quaternion adjustedRotation = Quaternion.Euler(eulerRotation);
            StartCoroutine(SmoothRotateTo(adjustedRotation));
        }

        rb.useGravity = true;
        //rb.angularVelocity = new Vector3(0, 0, rotationSpeed * Mathf.Deg2Rad); // Set angular velocity for z-axis rotation

        isShoot = true;
        OnShoot?.Invoke();
        DrawTrajectory.Instance.turnOff();
    }

    public void setBullet(GameObject obj)
    {
        rb = obj.GetComponent<Rigidbody>();
        pos = obj.transform;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.None; // Ensure no constraints on Rigidbody
        originalScale = transform.localScale; // Update the original scale if setting a new bullet
    }

    private IEnumerator SmoothRotateTo(Quaternion targetRotation)
    {
        float duration = 0.5f; // Time to complete the rotation, adjust as needed
        float timeElapsed = 0f;

        Quaternion startRotation = transform.rotation;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float lerpValue = timeElapsed / duration;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, lerpValue);
            yield return null;
        }

        transform.rotation = targetRotation; // Ensure the final rotation is exact
    }
}
