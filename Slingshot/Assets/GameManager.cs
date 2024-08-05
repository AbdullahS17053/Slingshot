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

    public float limit = 60f;
    private float currentTime;
    public Text timerText;
    public bool start = false;

    void Start()
    {
        ballcam = GameObject.FindGameObjectWithTag("vcam").GetComponent<CinemachineVirtualCamera>();
        curtaincam = GameObject.FindGameObjectWithTag("CurtainCam").GetComponent<CinemachineVirtualCamera>();

        curtaincam.Priority = 20;
        StartCoroutine(StartGameDelay());
    }

    public void levelStart(float time)
    {
        currentTime = time;
        start = true;
       
    }
    public void levelStop()
    {
        currentTime = 0;
        start = false;
        
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
