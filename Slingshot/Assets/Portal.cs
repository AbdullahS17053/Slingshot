using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    [SerializeField] Transform destination;

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WindowObject")) {

            other.GetComponent<CustomSpeeds>().Teleport(destination.position, destination.rotation);
        }
    }
}
