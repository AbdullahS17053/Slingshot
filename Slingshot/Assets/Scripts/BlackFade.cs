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
            if(other.gameObject.GetComponent<CustomSpeeds>().inFilled)
                StartCoroutine(other.gameObject.GetComponent<CustomSpeeds>().isSeeThru());
        }
    }

}
