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


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnMouseDown()
    {
        mousePressDownPos = Input.mousePosition;

    }

    private void OnMouseDrag()
    {
        if (isShoot) return;

        Vector3 forceInit = (Input.mousePosition - mousePressDownPos);
        forceInit.x = Mathf.Clamp(forceInit.x, maxLeftDrag, maxRightDrag);
        forceInit.y = Mathf.Clamp(forceInit.y, float.NegativeInfinity, maxUpDrag);

        Vector3 forceV = new Vector3(-forceInit.x, -forceInit.y, -forceInit.y) * forceMultiplier;

        DrawTrajectory.Instance.UpdateTrajectory(forceV, rb, transform.position);

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

}
