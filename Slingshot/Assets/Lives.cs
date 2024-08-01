using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{

    public TMP_Text lives;
    public TMP_Text Diamonds;
    public int CurrLives;
    public int CurrDiamonds;

    public GameObject bullet;
    public GameObject curtain1;
    public GameObject curtain2;

    private Animator animator1;
    private Animator animator2;
    bool isPlaying = false;

    private CoinSlider slider;

    void Start()
    {
        slider = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<CoinSlider>();
        CurrLives = slider.GetHealth();
        UpdateUI(CurrLives);
        UpdateDiamond(CurrDiamonds);

        curtain1 = GameObject.FindGameObjectWithTag("Curtain1");
        curtain2 = GameObject.FindGameObjectWithTag("Curtain2");

        if (curtain1 != null) { 
        
            animator1 = curtain1.GetComponent<Animator>();
        }
        if (curtain2 != null)
        {
            animator2 = curtain2.GetComponent<Animator>();
        }
    }

    private void Update()
    {

        if (CurrLives == 0 && isPlaying == false) {
            PlayAnimations(animator1 , "CurtainsMovementNew 1");
            PlayAnimations(animator1, "CurtainsMovementNew 2");

            isPlaying = true;

        }
        //if(slider!=null)
        //slider.SetSliderBar(CurrLives);
    }
    private void PlayAnimations(Animator animator , string animationName) {

        animator1.SetTrigger("CloseCurtain1");
        animator2.SetTrigger("CloseCurtain2");

        bullet = GameObject.FindGameObjectWithTag("Projectile");
        Destroy(bullet.gameObject, 0.5f);
    }

    public void AddDiamond() {

        CurrDiamonds++;
        UpdateDiamond(CurrDiamonds);
    }

    public void RemoveDiamond(int price)
    {
        if (CurrDiamonds - price >= 0) { 
        
            CurrDiamonds -= price;
        }
    }

    public void resetLives() {
        CurrLives = slider.GetHealth();
        slider.ResetHealth();
        UpdateUI(CurrLives);

    }
    public void AddLife() {

        if (CurrLives != 0) { 
        
            CurrLives++;
            slider.HealthIncreaseSlider();
            UpdateUI(CurrLives);
        }
    }
    public void LifeLost() {

        if (CurrLives != 0) { 
        
            CurrLives--;
            slider.HealthDecreaseSlider();
            UpdateUI(CurrLives);
        }
    }

    private void UpdateDiamond(int sc) {
        Diamonds.text = sc.ToString();
    }
    private void UpdateUI(int sc)
    {
        lives.text = sc.ToString();
    }
}
