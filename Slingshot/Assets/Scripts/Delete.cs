using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete : MonoBehaviour
{
    public int perHitHealth = 10;
    private Lives life;
    private int food;
    private WindowLevel windowLevel;
    void Start()
    {
        windowLevel = GameObject.FindGameObjectWithTag("GameManager").GetComponent<WindowLevel>();
        life = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Lives>();
        food = LayerMask.NameToLayer("GoodFood");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("WindowObject") && !collision.gameObject.GetComponent<CustomSpeeds>().launched)
        {
            if (collision.gameObject.layer == food) {

                if (windowLevel.GetLevelName() != "Blue Windows")
                {
                    life.RemoveHealth(perHitHealth);
                }
                Destroy(collision.gameObject);
            }
            Destroy(collision.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
       
    }
}
