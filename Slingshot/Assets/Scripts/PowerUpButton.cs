using Microlight.MicroBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpButton : MonoBehaviour
{
    [Header("Microbar Prefab")]
    [SerializeField] juiceBar juicebar;
    [SerializeField] MicroBar healthBar;
    private int currValue = 0;
    public int maxValue = 30;
    int oldJuice = 0;

    [Header("Sounds")]
    [SerializeField] AudioClip fullSound;
    [SerializeField] AudioSource soundSource;
    public bool soundOn = false;
    private Lives life;
    
    // Start is called before the first frame update
    void Start()
    {
        juicebar = GameObject.FindGameObjectWithTag("juiceBar").GetComponent<juiceBar>();
        life = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Lives>();
        healthBar.Initialize(maxValue);
    }

    // Update is called once per frame
    void Update()
    {
        if(oldJuice != juicebar.currJuice)
        {
            UpdateValue(juicebar.currJuice);
            oldJuice = juicebar.currJuice;
        }
    }

    public void healthRefill()
    {
        if (currValue >= maxValue && life.currLives < life.maxHP)
        {
            life.AddHealth(30);
            if (healthBar != null) healthBar.UpdateBar(0, false, UpdateAnim.Heal);
            if (juicebar != null)  juicebar.RemoveJuice(juicebar.currJuice);
        }
    }

    public void UpdateValue(int value)
    {
        if(currValue >= maxValue)
        {
            currValue = maxValue;
            return;
        }
        currValue = value;
        if (currValue > maxValue) { 
            currValue = maxValue;
            soundSource.clip = fullSound;
            if (soundOn) soundSource.Play();
        }


        // Update juicebar
        if (healthBar != null) healthBar.UpdateBar(currValue, false, UpdateAnim.Heal);
        //leftAnimator.SetTrigger("Heal");

    }

    public void RemoveValue(int value)
    {
        currValue -= value;
        if (currValue < 0f) currValue = 0;

        // Update juicebar
        if (healthBar != null) healthBar.UpdateBar(currValue, false, UpdateAnim.Damage);
        //leftAnimator.SetTrigger("Damage");
    }

    public void resetValue()
    {
        RemoveValue(maxValue);
    }
}
