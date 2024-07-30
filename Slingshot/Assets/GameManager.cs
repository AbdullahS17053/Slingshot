using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private CinemachineVirtualCamera ballcam;
    private CinemachineVirtualCamera curtaincam;
    public float camTime;

    void Start()
    {
        ballcam = GameObject.FindGameObjectWithTag("vcam").GetComponent<CinemachineVirtualCamera>();
        curtaincam = GameObject.FindGameObjectWithTag("CurtainCam").GetComponent<CinemachineVirtualCamera>();


        curtaincam.Priority = 20;
        StartCoroutine(StartGameDelay());


    }

    public void ChangeCameraPriorityToCurtains() {

        curtaincam.Priority = 20;
        ballcam.Priority = 10;

    }

    public void ChangeCameraPriorityToBall() { 
    
        ballcam.Priority = 20;
        curtaincam.Priority = 10;
    }

    IEnumerator StartGameDelay() {

        yield return new WaitForSeconds(camTime);


        ballcam.Priority = 20;
        curtaincam.Priority = 10;
    }
   
}
