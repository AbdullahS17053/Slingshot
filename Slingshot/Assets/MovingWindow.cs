using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWindow : MonoBehaviour
{

    public float maxLeftPoint;
    public float maxRightPoint;
    public float speed;
    public Vector3 initialDirection = Vector3.right;
    private Vector3 direction;
    private WindowLevel level;
    void Start()
    {
        level = GameObject.FindGameObjectWithTag("GameManager").GetComponent<WindowLevel>();
        maxLeftPoint = -1.07f;
        maxRightPoint = 0.88f;
        speed = 0.3f;
        direction = initialDirection;
    }

    void Update()
    {
        if (level.level)
            return;
        transform.Translate(direction * speed * Time.deltaTime);

        if (transform.position.x >= maxRightPoint)
        {

            direction = Vector3.left;
        }
        else if (transform.position.x <= maxLeftPoint) { 
        
            direction = Vector3.right;
        }
        
        
    }
}
