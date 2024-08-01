using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class CoinSlider : MonoBehaviour
{
    private Slider _slider;
    public int value = 5;
    public AudioSource empty;
    private Animator anim;
    private bool isEmpty = false;
    bool played = false;

    public float lerpDuration = 0.2f; // Duration for the lerp

    void Start()
    {
        _slider = GetComponent<Slider>();
        anim = GetComponent<Animator>();
        _slider.value = value;
    }

    void Update()
    {

        if (_slider.value == 0 && !played)
        {
            isEmpty = true;
            anim.SetBool("isFull", isEmpty);
            empty.Play();
            played = true;

        }
        else if (_slider.value > 0) {


            isEmpty = false;
            anim.SetBool("isFull", false);
            played = false;
        }

    }

    public void HealthDecreaseSlider()
    {
        if (_slider.value > 0)
        {
            StartCoroutine(LerpSliderValue(_slider.value, _slider.value - 1, lerpDuration));
        }
    }

    public void HealthIncreaseSlider()
    {
        StartCoroutine(LerpSliderValue(_slider.value, _slider.value + 1, lerpDuration));
    }

    public void SetSliderBar(int v) {

        _slider.value = v;
    }
    public void ResetHealth()
    {
        _slider.value = value;
    }

    public int GetHealth()
    {
        return (int)_slider.value;
    }

    private IEnumerator LerpSliderValue(float startValue, float endValue, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _slider.value = Mathf.Lerp(startValue, endValue, elapsed / duration);
            yield return null;
        }

        _slider.value = endValue;
    }
}
