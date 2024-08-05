using Microlight.MicroBar;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class Lives : MonoBehaviour
{
    [Header("Microbar Prefab")]
    [SerializeField] MicroBar healthBar;
    [SerializeField] MicroBar PatternHealthBar;


    [Header("Sounds")]
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip healSound;
    [SerializeField] AudioSource soundSource;
    [SerializeField] Text soundButtonText;
    public bool soundOn = false;

    public TMP_Text health;
    public TMP_Text diamonds;
    public int currLives;
    public int currDiamonds;
    public int maxHP = 100;

    public GameObject bullet;
    public GameObject curtainLeft;
    public GameObject curtainRight;

    private Animator animator1;
    private Animator animator2;
    bool isPlaying = false;

    void Start()
    {
        currLives = maxHP;
        healthBar.Initialize(maxHP);
        UpdateHealth(currLives);
        //UpdateDiamond(currDiamonds);

        curtainLeft = GameObject.FindGameObjectWithTag("Curtain1");
        curtainRight = GameObject.FindGameObjectWithTag("Curtain2");

        if (curtainLeft != null) { 
        
            animator1 = curtainLeft.GetComponent<Animator>();
        }
        if (curtainRight != null)
        {
            animator2 = curtainRight.GetComponent<Animator>();
        }
    }

    private void Update()
    {

        if (currLives == 0 && isPlaying == false) {
            PlayAnimations(animator1 , "CurtainsMovementNew 1");
            PlayAnimations(animator1, "CurtainsMovementNew 2");

            isPlaying = true;

        }
    }
    private void PlayAnimations(Animator animator , string animationName) {

        animator1.SetTrigger("Closecurtain1");
        animator2.SetTrigger("CloseCurtain2");

        bullet = GameObject.FindGameObjectWithTag("Projectile");
        Destroy(bullet.gameObject, 0.5f);
    }

    private void UpdateDiamond(int sc)
    {
        diamonds.text = sc.ToString();
    }

    public void AddDiamond() {

        currDiamonds++;
        UpdateDiamond(currDiamonds);
    }

    public void RemoveDiamond(int price)
    {
        if (currDiamonds - price >= 0) { 
        
            currDiamonds -= price;
        }
    }

    private void UpdateHealth(int sc)
    {
        health.text = sc.ToString() + "HP";
    }

    public void AddHealth(int value) {

        currLives += value;
        if (currLives > maxHP) currLives = maxHP;
        soundSource.clip = healSound;
        if (soundOn) soundSource.Play();

        // Update HealthBar
        if (healthBar != null) healthBar.UpdateBar(currLives, false, UpdateAnim.Heal);
        //leftAnimator.SetTrigger("Heal");
        UpdateHealth(currLives);
    }
    public void RemoveHealth(int value) {

        currLives -= value;
        if (currLives < 0f) currLives = 0;
        soundSource.clip = hurtSound;
        if (soundOn) soundSource.Play();

        // Update HealthBar
        if (healthBar != null) healthBar.UpdateBar(currLives, false, UpdateAnim.Damage);
        //leftAnimator.SetTrigger("Damage");
        UpdateHealth(currLives);
    }

    public void resetHealth()
    {
        AddHealth(maxHP);
    }
}
