using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinSlider : MonoBehaviour
{
    private Slider coinSlider;
    private float value = 0;
    public AudioSource full;
    private Animator anim;
    private bool isFull = false;
    bool played = false;

    // Start is called before the first frame update
    void Start()
    {
        coinSlider = GetComponent<Slider>();
        anim = GetComponent<Animator>();
        coinSlider.value = value;
    }

    // Update is called once per frame
    void Update()
    {
        coinSlider.value = Mathf.Lerp(coinSlider.value, value, Time.deltaTime * 3f);

        if (value >= coinSlider.maxValue)
        {
            isFull = true;
        }

        anim.SetBool("isFull", isFull);
        if(isFull && !played)
        {
            full.Play();
            played = true;
        }
    }

    public void CollectCoin()
    {
        value++;
    }

    public void PerformAction()
    {
        full.Stop();
        //called when slider is full and then it is pressed
        Debug.Log("Slider pressed");
        PlaneMovement.poweredUp = true;
        value = 0;
        isFull = false;
    }

    private void OnMouseDown()
    {
        //Debug.Log("Slider pressed");
        if (isFull)
        {
            PerformAction();
        }
    }
}
