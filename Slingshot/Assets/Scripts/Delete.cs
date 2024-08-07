using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete : MonoBehaviour
{
    public int perHitHealth = 10;
    private Lives life;
    private int food;
    private WindowLevel windowLevel;
    private MainSpawner mainSpawner;
    void Start()
    {
        windowLevel = GameObject.FindGameObjectWithTag("GameManager").GetComponent<WindowLevel>();
        life = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Lives>();
        mainSpawner = FindObjectOfType<MainSpawner>();
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
            if (collision.gameObject.layer == food)
            {

             
                if (windowLevel.GetLevelName() == "Blue Windows" && mainSpawner.isPattern) // meaning if pattern is started in blue windows
                {
                    Destroy(collision.gameObject);
                }
                else {

                    CameraShake.Instance.ShakeCamera(1.2f, 5f, 0.5f);
                    life.RemoveHealth(perHitHealth);
                }
              
            }
            Destroy(collision.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
       
    }
}
