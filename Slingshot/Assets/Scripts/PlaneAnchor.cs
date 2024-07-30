using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneAnchor : MonoBehaviour
{

    public LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        line.positionCount = 2;
        line.SetPosition(0, transform.position);
    }

    private void OnMouseDrag()
    {
        Debug.Log("Hello");
        line.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
