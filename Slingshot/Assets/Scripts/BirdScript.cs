using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public AudioSource audi;

    void playSound()
    {
        audi.Play();
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("playSound", 0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
