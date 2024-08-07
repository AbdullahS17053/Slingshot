using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTrajectory : MonoBehaviour
{
    private GameObject lookAtTarget;
    //public GameObject line;
    //public GameObject bigLine;

    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    [Range(10, 100)]
    private int lineSegmentCount = 50;

    [SerializeField]
    private LayerMask collisionMask;
    public bool hasHit = false;
    public Vector3 windowHit;

    private List<Vector3> linePoints = new List<Vector3>();
    private List<Vector3> fullTrajectoryPoints = new List<Vector3>();

    #region Singleton

    public static DrawTrajectory Instance;

    private CinemachineVirtualCamera ballVcam;
    private CinemachineVirtualCamera trajectoryVcam;
    private GameObject hitObject = null;
    bool glow = false;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    private void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        ballVcam = GameObject.FindGameObjectWithTag("vcam").GetComponent<CinemachineVirtualCamera>();
    }


    public void UpdateTrajectory(Vector3 forceVector, Rigidbody rigidBody, Vector3 startPoint)
    {
        GameObject[] lines = GameObject.FindGameObjectsWithTag("line");

        // Iterate through each object and destroy it
        foreach (GameObject line in lines)
        {
            Destroy(line);
        }
        Vector3 velocity = (forceVector / rigidBody.mass) * Time.fixedDeltaTime;
        float flightDuration = (2 * velocity.y) / Physics.gravity.y;
        float stepTime = flightDuration / lineSegmentCount;

        linePoints.Clear();
        fullTrajectoryPoints.Clear();

        Vector3 previousPoint = startPoint;
        linePoints.Add(previousPoint);
        //Instantiate(line,previousPoint, Quaternion.identity);
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
            //Instantiate(line, newPoint, Quaternion.identity);


            if (col) continue; // continue to add points to fullTrajectoryPoints but not to linePoints

            RaycastHit hit;
            if (Physics.Raycast(previousPoint, newPoint - previousPoint, out hit, Vector3.Distance(previousPoint, newPoint), collisionMask))
            {
                //Debug.Log(windowHit);
                windowHit = hit.point;
                if (glow && hitObject !=  hit.collider.gameObject)
                {
                    hitObject.GetComponent<GlowWindow>().SetOff();
                    glow = false;
                }
                if(hit.collider.gameObject.CompareTag("Window") || hit.collider.gameObject.CompareTag("Plate"))
                {

                    
                    
                    hitObject = hit.collider.gameObject;
                    hitObject.GetComponent<GlowWindow>().SetOn();
                    glow = true;
                }
                linePoints.Add(hit.point);
                //if(i > 1)
                //    Instantiate(bigLine, hit.point, Quaternion.identity);
                col = true;
            }
            else
            {
                //Debug.Log(windowHit);
                linePoints.Add(newPoint);
                //if (i > 1)
                //    Instantiate(line, newPoint, Quaternion.identity);
                windowHit = newPoint;
                previousPoint = newPoint;
            }
        }

        if (!col) ExtendTrajectoryBelow();

        lineRenderer.positionCount = linePoints.Count;
        lineRenderer.SetPositions(linePoints.ToArray());
    }

    public Vector3 GetLastPoint()
    {
        return linePoints[linePoints.Count - 1];
    }

    private void ExtendTrajectoryBelow()
    {
        if (linePoints.Count < 2) return;

        Vector3 lastPoint = linePoints[linePoints.Count - 1];
        Vector3 secondLastPoint = linePoints[linePoints.Count - 2];
        Vector3 direction = lastPoint - secondLastPoint;

        float extensionLength = 5f;  
        Vector3 extendedPoint = lastPoint + direction.normalized * extensionLength;

        linePoints.Add(extendedPoint);
        //Instantiate(line, extendedPoint, Quaternion.identity);
        fullTrajectoryPoints.Add(extendedPoint);
    }

    public void HideLine()
    {
        lineRenderer.positionCount = 0;
        //trajectoryVcam.LookAt = null;

        //trajectoryVcam.Priority = 10;
        //ballVcam.Priority = 20;
        hasHit = false;
        Destroy(lookAtTarget);

        GameObject[] lines = GameObject.FindGameObjectsWithTag("line");

        // Iterate through each object and destroy it
        foreach (GameObject line in lines)
        {
            Destroy(line);
        }
    }

    public void turnOff()
    {
        if (hitObject != null)
        { 
            hitObject.GetComponent<GlowWindow>().SetOff();
            glow = false;
            hitObject = null;
        }
    }
}
