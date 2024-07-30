using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject loadScreen;
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 60;
        PlayerPrefs.SetInt("tut", 0);
    }
    private void Start()
    {
        Time.timeScale = 1.0f;
    }
    public void LoadLevel(int level)
    {
        PlayerPrefs.SetInt("level", level);
        StartCoroutine(loadingScreen());
        
    }

    IEnumerator loadingScreen()
    {
        
        loadScreen.SetActive(true);
        //loadScreen.GetChild<Text>().text = "hello.";
        loadScreen.GetComponentInChildren<Text>().text = "Dont miss the hoops!";
        yield return new WaitForSecondsRealtime(3);
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        while (!operation.isDone)
        {

            yield return null;
        }
    }
}
