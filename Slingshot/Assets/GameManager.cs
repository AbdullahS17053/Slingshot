using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private CinemachineVirtualCamera ballcam;
    private CinemachineVirtualCamera curtaincam;
    public float camTime;

    public float startTime = 60f;
    private float currentTime;
    public Text timerText;

    void Start()
    {
        ballcam = GameObject.FindGameObjectWithTag("vcam").GetComponent<CinemachineVirtualCamera>();
        curtaincam = GameObject.FindGameObjectWithTag("CurtainCam").GetComponent<CinemachineVirtualCamera>();

        curtaincam.Priority = 20;
        StartCoroutine(StartGameDelay());

        currentTime = startTime;
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerText();
            yield return null;
        }

        currentTime = 0;
        UpdateTimerText();
        TimerEnded();
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void AddTime(float timeToAdd)
    {
        currentTime += timeToAdd;
        if (currentTime > startTime) // Optional: Cap the time to startTime
        {
            currentTime = startTime;
        }
        UpdateTimerText();
    }

    public void SubtractTime(float timeToSubtract)
    {
        currentTime -= timeToSubtract;
        if (currentTime < 0)
        {
            currentTime = 0;
            TimerEnded(); // Ensure timer ends if it goes below zero
        }
        UpdateTimerText();
    }

    void TimerEnded()
    {
        // Add any additional actions to perform when the timer ends
    }

    public void ChangeCameraPriorityToCurtains()
    {
        curtaincam.Priority = 20;
        ballcam.Priority = 10;
    }

    public void ChangeCameraPriorityToBall()
    {
        ballcam.Priority = 20;
        curtaincam.Priority = 10;
    }

    IEnumerator StartGameDelay()
    {
        yield return new WaitForSeconds(camTime);
        ChangeCameraPriorityToBall();
    }
}
