using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DragAndShoot : MonoBehaviour
{
    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;
    public float forceMultiplier = 3;
    public float maxLeftDrag = -200f;  
    public float maxRightDrag = 200f;  
    public float maxUpDrag = 0f; // no upward drag

    private Rigidbody rb;

    private bool isShoot;

    public delegate void ShootAction();
    public event ShootAction OnShoot;


    public CinemachineVirtualCamera vcam;
    public float camMoveSpeed = 1f;
    private Vector3 initialCameraPosition;

    // Constraints for camera movement
    public float maxCameraX = 1f;
    public float minCameraX = 0f;
    //public float maxCameraY = 1f;
    //public float minCameraY = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        vcam = GameObject.FindGameObjectWithTag("vcam").GetComponent<CinemachineVirtualCamera>();
    }

    private void OnMouseDown()
    {
        mousePressDownPos = Input.mousePosition;
        initialCameraPosition = vcam.transform.position;

    }

    private void OnMouseDrag()
    {
        if (isShoot) return;

        Vector3 forceInit = (Input.mousePosition - mousePressDownPos);
        forceInit.x = Mathf.Clamp(forceInit.x, maxLeftDrag, maxRightDrag);
        forceInit.y = Mathf.Clamp(forceInit.y, float.NegativeInfinity, maxUpDrag);

        Vector3 forceV = new Vector3(-forceInit.x, -forceInit.y, -forceInit.y) * forceMultiplier;

        DrawTrajectory.Instance.UpdateTrajectory(forceV, rb, transform.position);

        MoveCamera(forceInit);
    }

    private void OnMouseUp()
    {
        if (isShoot) return;

        mouseReleasePos = Input.mousePosition;
        Vector3 force = (mouseReleasePos - mousePressDownPos);
        force.x = Mathf.Clamp(force.x, maxLeftDrag, maxRightDrag);
        force.y = Mathf.Clamp(force.y, float.NegativeInfinity, maxUpDrag);
        Shoot(force);

        DrawTrajectory.Instance.HideLine();
    }

    void Shoot(Vector3 force)
    {
        rb.AddForce(new Vector3(-force.x, -force.y, -force.y) * forceMultiplier);
        isShoot = true;

        OnShoot?.Invoke();


    }




    void MoveCamera(Vector3 dragInput) {

        // Calculate the new camera position based on the initial position and drag input
        float cameraMovement = dragInput.x * camMoveSpeed * Time.deltaTime;
        float newCameraPositionX = initialCameraPosition.x + cameraMovement;

        // Clamp the new camera position within the allowed range
        newCameraPositionX = Mathf.Clamp(newCameraPositionX, minCameraX, maxCameraX);

        // Apply the new camera position
        vcam.transform.position = new Vector3(newCameraPositionX, vcam.transform.position.y, vcam.transform.position.z);


    }
}
