using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TricksterHelper : MonoBehaviour
{

    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
            parent.GetComponent<Trickster>().Hit();
    }

    public void parentDeath()
    {
        
    }
}
