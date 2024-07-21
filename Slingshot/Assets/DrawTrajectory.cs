using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTrajectory : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    [Range(10, 100)]
    private int lineSegmentCount = 50;

    [SerializeField]
    private LayerMask collisionMask;

    private List<Vector3> linePoints = new List<Vector3>();

    #region Singleton

    public static DrawTrajectory Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public void UpdateTrajectory(Vector3 forceVector, Rigidbody rigidBody, Vector3 startPoint)
    {
        Vector3 velocity = (forceVector / rigidBody.mass) * Time.fixedDeltaTime;
        float flightDuration = (2 * velocity.y) / Physics.gravity.y;
        float stepTime = flightDuration / lineSegmentCount;

        linePoints.Clear();

        Vector3 previousPoint = startPoint;
        linePoints.Add(previousPoint);

        bool col = false;

        for (int i = 0; i < lineSegmentCount; i++)
        {

            if (col) break;

            float stepTimePassed = stepTime * i;
            Vector3 movementVector = new Vector3(
                velocity.x * stepTimePassed,
                velocity.y * stepTimePassed - 0.5f * Physics.gravity.y * stepTimePassed * stepTimePassed,
                velocity.z * stepTimePassed
            );

            Vector3 newPoint = -movementVector + startPoint;

            RaycastHit hit;
            if (Physics.Raycast(previousPoint, newPoint - previousPoint, out hit, Vector3.Distance(previousPoint, newPoint), collisionMask))
            {
                linePoints.Add(hit.point);
                col = true;
            }
            else {

                linePoints.Add(newPoint);
                previousPoint = newPoint;
            }

            

        }

        if (!col) ExtendTrajectoryBelow();

        lineRenderer.positionCount = linePoints.Count;
        lineRenderer.SetPositions(linePoints.ToArray());
    }

    private void ExtendTrajectoryBelow()
    {
        if (linePoints.Count < 2) return;

        Vector3 lastPoint = linePoints[linePoints.Count - 1];
        Vector3 secondLastPoint = linePoints[linePoints.Count - 2];
        Vector3 direction = lastPoint - secondLastPoint;

        // Extend trajectory below 
        float extensionLength = 5f;  //value
        Vector3 extendedPoint = lastPoint + direction.normalized * extensionLength;

        linePoints.Add(extendedPoint);
    }


    public void HideLine()
    {
        lineRenderer.positionCount = 0;
    }
}
