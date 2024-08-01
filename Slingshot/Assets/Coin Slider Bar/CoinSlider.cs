using System.Collections;
using System.Collections.Generic;
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

    private Coroutine lerpCoroutine;
    private float targetValue;

    void Start()
    {
        _slider = GetComponent<Slider>();
        anim = GetComponent<Animator>();
        _slider.value = value;
        targetValue = value;
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
        else if (_slider.value > 0)
        {
            isEmpty = false;
            anim.SetBool("isFull", false);
            played = false;
        }
    }

    public void HealthDecreaseSlider()
    {
        if (_slider.value > 0)
        {
            targetValue = Mathf.Max(0, targetValue - 1);
            if (lerpCoroutine != null)
            {
                StopCoroutine(lerpCoroutine);
            }
            lerpCoroutine = StartCoroutine(LerpSliderValue(_slider.value, targetValue, lerpDuration));
        }
    }

    public void HealthIncreaseSlider()
    {
        targetValue = Mathf.Min(_slider.maxValue, targetValue + 1);
        if (lerpCoroutine != null)
        {
            StopCoroutine(lerpCoroutine);
        }
        lerpCoroutine = StartCoroutine(LerpSliderValue(_slider.value, targetValue, lerpDuration));
    }

    public void ResetHealth()
    {
        _slider.value = value;
    }

    public int GetHealth()
    {
        return value;
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
