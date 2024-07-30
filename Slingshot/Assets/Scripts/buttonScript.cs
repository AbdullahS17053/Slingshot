using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonScript : MonoBehaviour
{
    public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void playSound()
    {
        audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
