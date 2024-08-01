using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackFade : MonoBehaviour
{
    // Start is called before the first frame update

    public float fadeDuration = 5f;
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
            CustomSpeeds customSpeeds = other.gameObject.GetComponent<CustomSpeeds>();

            // Check if the component exists
            if (customSpeeds != null)
            {
                // If the component exists, perform the operations
                if (customSpeeds.inFilled)
                {
                    // Start the coroutine defined in the CustomSpeeds component
                    StartCoroutine(customSpeeds.isSeeThru());
                }
            }
            else
            {
                //Debug.Log("CustomSpeeds component not found on the GameObject.");
            }
        }
    }

}
