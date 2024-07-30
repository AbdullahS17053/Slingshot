using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWithMouse : MonoBehaviour
{
    public LineRenderer line;
    private Vector3 previousPosition;
    
    // Start is called before the first frame update
    private void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 1;
        previousPosition = transform.position;
    }
    [SerializeField]
    private float minDistance = 0.1f;

    public void startLine(Vector2 position)
    {
        line.positionCount = 1;
        line.SetPosition(0, position);
    }

    public void UpdateLine()
    {
        if(Input.GetMouseButton(0))
        {
            Vector3 CurrPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CurrPosition.z = 0f;

            if(Vector3.Distance(CurrPosition,previousPosition) > minDistance)
            {
                if(previousPosition == transform.position)
                {
                    line.SetPosition(0, CurrPosition);
                }
                else
                {
                    line.positionCount++;
                    line.SetPosition(line.positionCount - 1, CurrPosition);
                }

                previousPosition = CurrPosition;
            }
        }
    }
}
