using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WindowLevel : MonoBehaviour
{
    public GameObject[] windows;
    public GameObject[] loadingWindows;

    public Material originalParday;
    public Material[] parday;
    public Material loadingParday;
    public Color defaultParday;

    public Material originalBackground;
    public Material[] backgroundMaterials;
    public Material loadingBackgroundMaterial;
    public Color defaultBackground;
    public GameObject bullet;

    bool load = true;
    bool play = false;
    bool level = false;

    public Text Score;
    int levelNum = 0;
    int oldLevelNum = -1;
    public int levelChangeScore = 50;

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
                

            loadingWindows[levelNum].SetActive(true);
            originalBackground.color = loadingBackgroundMaterial.color;
            load = false;
            bullet.SetActive(false);
            StartCoroutine(loadingLevel());
        }

        if (level && play)
        {
            play = false;
            loadingWindows[levelNum].SetActive(false);
            windows[levelNum].SetActive(true);
            windows[levelNum].GetComponentInChildren<MainSpawner>().enabled = false;
            StartCoroutine(startSpawner());
            originalBackground.color = backgroundMaterials[levelNum].color;
            originalParday.color = parday[levelNum].color;
            load = true;
            level = false;
        }
    }

    IEnumerator loadingLevel()
    {
        yield return new WaitForSeconds(5f);
        play = true;
        bullet.SetActive(true);
        bullet.GetComponent<BulletSpawner>().levelChange(0f);
    }

    IEnumerator startSpawner()
    {
        yield return new WaitForSeconds(2f);
        windows[levelNum].GetComponentInChildren<MainSpawner>().enabled = true;
        windows[levelNum].GetComponentInChildren<MainSpawner>().Start();
    }
}
