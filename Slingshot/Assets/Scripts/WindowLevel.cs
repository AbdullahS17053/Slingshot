using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using TMPro;

public class WindowLevel : MonoBehaviour
{
    public GameObject[] windows;
    public GameObject[] loadingWindows;
    public float loadTime;

    public Material originalParday;
    public Material[] parday;
    public Material loadingParday;
    public Color defaultParday;

    public Material originalBackground;
    public Material baseBg;
    public Material[] backgroundMaterials;
    public Material loadingBackgroundMaterial;
    public Color defaultBackground;
    public GameObject bullet;
    public ParticleSystem[] fires;
    public float normalFireLifetime = 1.0f; // Default lifetime for normal fire
    public float fireBoostLifetime = 2.0f; // Lifetime for boosted fire
    public float boostTime = 5.0f; // Duration for which the fire boost should last
    bool fireBoosted = false;

    public float[] time;

    bool load = true;
    bool play = false;
    bool level = false;

    public TMP_Text Score;
    int levelNum = 0;
    int oldLevelNum = -1;
    public int levelChangeScore = 50;

    ScoreCounter scoreCounter;
    GameManager gameManager;
    Lives life;


    // Start is called before the first frame update
    void Start()
    {
        originalBackground.color = defaultBackground;
        originalParday.color = defaultParday;
        // Turn off all GameObjects in the windows array
        for (int i = 0; i < windows.Length; i++)
        {
            if (windows[i] != null)
            {
                windows[i].SetActive(false);
            }
        }
        

        // Turn off all GameObjects in the loadingWindows array
        for (int i = 0; i < loadingWindows.Length; i++)
        {
            if (loadingWindows[i] != null)
            {
                loadingWindows[i].SetActive(false);
            }
        }

        foreach (ParticleSystem fire in fires)
        {
            fire.Play();
        }

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        life = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Lives>();
    }

    // Update is called once per frame
    void Update()
    {
        levelNum = (int.Parse(Score.text)/ levelChangeScore) % windows.Length;
        if(levelNum !=  oldLevelNum)
        {
            level = true;
            oldLevelNum = levelNum;
        }


        if (level && load)
        {
            if(levelNum > 0)
            {
               
                windows[levelNum - 1].SetActive(false);
                originalParday.color = loadingParday.color;

            }
               

            else
            {
               
                windows[windows.Length - 1].SetActive(false);
                originalParday.color = defaultParday;
            }
              
            SetFiresLifetimeToZero();
            loadingWindows[levelNum].SetActive(true);
            originalBackground.color = loadingBackgroundMaterial.color;
            baseBg.color = loadingBackgroundMaterial.color;
            load = false;
            bullet.SetActive(false);
            gameManager.levelStop();
            gameManager.ChangeCameraPriorityToCurtains();

            StartCoroutine(loadingLevel());
        }

        if (level && play)
        {
            //new level 

            life.resetLives();

            play = false;
            loadingWindows[levelNum].SetActive(false);
            windows[levelNum].SetActive(true);
            windows[levelNum].GetComponentInChildren<MainSpawner>().enabled = false;

            // cam
            gameManager.ChangeCameraPriorityToBall();

            StartCoroutine(startSpawner());
            originalBackground.color = backgroundMaterials[levelNum].color;
            baseBg.color = backgroundMaterials[levelNum].color;
            originalParday.color = parday[levelNum].color;
            load = true;
            level = false;
        }
    }

    IEnumerator loadingLevel()
    {
        yield return new WaitForSeconds(loadTime);

        GameObject[] objectsToDelete = GameObject.FindGameObjectsWithTag("WindowObject");

        // Loop through and delete each object
        foreach (GameObject obj in objectsToDelete)
        {
            Destroy(obj);
        }
        play = true;
        bullet.SetActive(true);
        bullet.GetComponent<BulletSpawner>().levelChange(0f);
        SetFiresToNormal();
        //scoreCounter.ResetScore();
        gameManager.levelStart(time[levelNum]);

    }

    IEnumerator startSpawner()
    {
        yield return new WaitForSeconds(1f);
        windows[levelNum].GetComponentInChildren<MainSpawner>().enabled = true;
        windows[levelNum].GetComponentInChildren<MainSpawner>().Start();
    }
    public void SetFiresLifetimeToZero()
    {
        foreach (ParticleSystem fire in fires)
        {
            var main = fire.main;
            main.startLifetime = 0f;
        }
    }
    public void SetFiresToNormal()
    {
        foreach (ParticleSystem fire in fires)
        {
            var main = fire.main;
            main.startLifetime = normalFireLifetime;
        }
        fireBoosted = false;
    }
    public void SetFiresToBoost()
    {
        if (fireBoosted)
            return;
        fireBoosted = true;
        foreach (ParticleSystem fire in fires)
        {
            var main = fire.main;
            main.startLifetime = fireBoostLifetime;
        }
        StartCoroutine(RevertToNormalAfterBoost());
    }

    private IEnumerator RevertToNormalAfterBoost()
    {
        yield return new WaitForSeconds(boostTime);
        SetFiresToNormal();
    }
}
