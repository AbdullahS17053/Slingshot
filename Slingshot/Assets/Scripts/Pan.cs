using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : MonoBehaviour
{
    public float fadeDuration = 5f;
    public WindowLevel level;
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
            //Debug.Log("pan");
            if (other.gameObject.GetComponent<CustomSpeeds>().inFilled)
                StartCoroutine(other.gameObject.GetComponent<CustomSpeeds>().isSeeThru());
            other.gameObject.GetComponent<CustomSpeeds>().pan = true;

            int badFood = LayerMask.NameToLayer("BadFood");
            bool ChangeColor = false;
            if (other.gameObject.layer == badFood)
            {
                ChangeColor = true;
            }

            other.gameObject.GetComponent<CustomSpeeds>().inPan(ChangeColor);
            level.SetFiresToBoost();
        }
    }
}
