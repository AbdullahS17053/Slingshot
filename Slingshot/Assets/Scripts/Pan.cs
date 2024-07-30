using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : MonoBehaviour
{
    public float fadeDuration = 5f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("WindowObject"))
        {
            Debug.Log("pan");
            if (other.gameObject.GetComponent<CustomSpeeds>().inFilled)
                StartCoroutine(other.gameObject.GetComponent<CustomSpeeds>().isSeeThru());
            other.gameObject.GetComponent<CustomSpeeds>().pan = true;
            other.gameObject.GetComponent<CustomSpeeds>().inPan();
        }
    }
}
