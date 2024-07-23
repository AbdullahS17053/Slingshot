using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DragAndShoot : MonoBehaviour
{
    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;
    public float forceMultiplier = 3;
    public float maxLeftDrag = -200f;
    public float maxRightDrag = 200f;
    public float maxUpDrag = 0f; // no upward drag
    public float rotationSpeed = 0.1f;
    public float xMultiplier = 1f;
    public float yMultiplier = 1f;
    public float zMultiplier = 1f;
    public Rigidbody rb;
    public Transform pos;

    private bool isShoot;

    public delegate void ShootAction();
    public event ShootAction OnShoot;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

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

        Vector3 forceV = new Vector3(-forceInit.x * xMultiplier, -forceInit.y * yMultiplier, -forceInit.y + zMultiplier) * forceMultiplier;

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
        rb.AddForce(new Vector3(-force.x * xMultiplier, -force.y * yMultiplier, -force.y + zMultiplier) * forceMultiplier);
        //rb.AddForce(new Vector3(-force.x * xMultiplier, -force.y * yMultiplier, -force.y * zMultiplier) * forceMultiplier);
        rb.useGravity = true;
        isShoot = true;

        OnShoot?.Invoke();
        DrawTrajectory.Instance.turnOff();
    }

    public void setBullet(GameObject obj)
    {
        rb = obj.GetComponent<Rigidbody>();
        pos = obj.transform;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.constraints = RigidbodyConstraints.FreezePosition;
    }
}
