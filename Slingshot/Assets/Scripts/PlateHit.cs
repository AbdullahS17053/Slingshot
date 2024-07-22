using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateHit : MonoBehaviour
{
    // Start is called before the first frame update

    public PlateController parentPlate;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            parentPlate.OutlineHit();
            Destroy(gameObject);
            //Destroy(collision.gameObject);
        }
    }
}
