using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartFade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WindowObject"))
        {
            other.gameObject.GetComponent<CustomSpeeds>().inFilled = true;

        }
    }
}
