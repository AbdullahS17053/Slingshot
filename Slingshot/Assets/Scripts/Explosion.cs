using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void explode(float value,Vector3 pos)
    {
        this.GetComponent<Rigidbody>().AddExplosionForce(value, pos, 2, 2, ForceMode.VelocityChange);
        Debug.Log("ah");
    }
}
