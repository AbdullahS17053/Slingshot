using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTrajectory : MonoBehaviour
{
    private GameObject lookAtTarget;

    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    [Range(10, 100)]
    private int lineSegmentCount = 50;

    [SerializeField]
    private LayerMask collisionMask;

    private List<Vector3> linePoints = new List<Vector3>();
    private List<Vector3> fullTrajectoryPoints = new List<Vector3>();

    #region Singleton

    public static DrawTrajectory Instance;

    private CinemachineVirtualCamera ballVcam;
    private CinemachineVirtualCamera trajectoryVcam;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    private void Start()
    {
        ballVcam = GameObject.FindGameObjectWithTag("vcam").GetComponent<CinemachineVirtualCamera>();
        trajectoryVcam = GameObject.FindGameObjectWithTag("trajectoryVcam").GetComponent<CinemachineVirtualCamera>();
        lookAtTarget = new GameObject("TrajectoryLookAtTarget");
    }

    public void UpdateTrajectory(Vector3 forceVector, Rigidbody rigidBody, Vector3 startPoint)
    {
        Vector3 velocity = (forceVector / rigidBody.mass) * Time.fixedDeltaTime;
        float flightDuration = (2 * velocity.y) / Physics.gravity.y;
        float stepTime = flightDuration / lineSegmentCount;

        linePoints.Clear();
        fullTrajectoryPoints.Clear();

        Vector3 previousPoint = startPoint;
        linePoints.Add(previousPoint);
        fullTrajectoryPoints.Add(previousPoint);

        bool col = false;

        for (int i = 0; i < lineSegmentCount; i++)
        {
            float stepTimePassed = stepTime * i;
            Vector3 movementVector = new Vector3(
                velocity.x * stepTimePassed,
                velocity.y * stepTimePassed - 0.5f * Physics.gravity.y * stepTimePassed * stepTimePassed,
                velocity.z * stepTimePassed
            );

            Vector3 newPoint = -movementVector + startPoint;
            fullTrajectoryPoints.Add(newPoint);

            if (col) continue; // continue to add points to fullTrajectoryPoints but not to linePoints

            RaycastHit hit;
            if (Physics.Raycast(previousPoint, newPoint - previousPoint, out hit, Vector3.Distance(previousPoint, newPoint), collisionMask))
            {
                linePoints.Add(hit.point);
                col = true;
            }
            else
            {
                linePoints.Add(newPoint);
                previousPoint = newPoint;
            }
        }

        if (!col) { } //ExtendTrajectoryBelow();

        lineRenderer.positionCount = linePoints.Count;
        lineRenderer.SetPositions(linePoints.ToArray());
        UpdateTrajectoryCamera();
    }

    private void ExtendTrajectoryBelow()
    {
        if (linePoints.Count < 2) return;

        Vector3 lastPoint = linePoints[linePoints.Count - 1];
        Vector3 secondLastPoint = linePoints[linePoints.Count - 2];
        Vector3 direction = lastPoint - secondLastPoint;

        int pointsCount = 5; // Number of points to add

        for (int i = 0; i < pointsCount; i++)
        {
            Vector3 newPoint = lastPoint + direction.normalized * (i + 1);
            linePoints.Add(newPoint);
            fullTrajectoryPoints.Add(newPoint);
        }
    }

    private void UpdateTrajectoryCamera()
    {
        if (fullTrajectoryPoints.Count > 0)
        {
            Vector3 middlePoint = fullTrajectoryPoints[fullTrajectoryPoints.Count / 2]; 
            lookAtTarget.transform.position = middlePoint;

            trajectoryVcam.LookAt = lookAtTarget.transform;

            trajectoryVcam.Priority = 20;
            ballVcam.Priority = 10;
        }
    }

    public void HideLine()
    {
        lineRenderer.positionCount = 0;
        trajectoryVcam.LookAt = null;

        trajectoryVcam.Priority = 10;
        ballVcam.Priority = 20;
    }
}
