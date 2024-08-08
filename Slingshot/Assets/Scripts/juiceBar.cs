using Microlight.MicroBar;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class juiceBar : MonoBehaviour
{
    [Header("Microbar Prefab")]
    [SerializeField] MicroBar juicebar;
   


    [Header("Sounds")]
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip healSound;
    [SerializeField] AudioSource soundSource;
    public bool soundOn = false;

    public int currJuice = 0;
    public int maxJuice = 100;

    void Start()
    {
        soundOn = false;
        juicebar.Initialize(maxJuice);
        resetJuice();
        soundOn = true;
    }

    private void Update()
    {

       
    }

    public void AddJuice(int value)
    {
        currJuice += value;
        if (currJuice > maxJuice) currJuice = maxJuice;
        soundSource.clip = healSound;
        if (soundOn) soundSource.Play();

        // Update juicebar
        if (juicebar != null) juicebar.UpdateBar(currJuice, false, UpdateAnim.Heal);
        //leftAnimator.SetTrigger("Heal");
       
    }
    public void RemoveJuice(int value)
    {
        currJuice -= value;
        if (currJuice < 0f) currJuice = 0;
        soundSource.clip = hurtSound;
        if (soundOn) soundSource.Play();

        // Update juicebar
        if (juicebar != null) juicebar.UpdateBar(currJuice, false, UpdateAnim.Damage);
        //leftAnimator.SetTrigger("Damage");
    }

    public void resetJuice()
    {
        RemoveJuice(maxJuice);
    }
    public void resetBartoZero() {

        currJuice = 0;
        if (juicebar != null) juicebar.UpdateBar(0, false, UpdateAnim.Damage);
    }
}
