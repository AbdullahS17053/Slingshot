using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthButton : MonoBehaviour
{
    public Image juiceBar;
    private Lives life;
    
    // Start is called before the first frame update
    void Start()
    {
        juiceBar = GameObject.FindGameObjectWithTag("juiceBar").GetComponent<Image>();
        life = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Lives>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void healthRefill()
    {
        if (juiceBar.fillAmount > 0.3)
            life.AddHealth(30);

    }
}
