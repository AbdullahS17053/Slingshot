using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;
    private float shakeTimeTotal;
    private float startingIntensity;
    private float startingFrequency;
    private float targetIntensity;
    private float targetFrequency;

    private void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float frequency, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (cinemachineBasicMultiChannelPerlin != null)
        {
            startingIntensity = cinemachineBasicMultiChannelPerlin.m_AmplitudeGain;
            startingFrequency = cinemachineBasicMultiChannelPerlin.m_FrequencyGain;
            targetIntensity = intensity;
            targetFrequency = frequency;
            shakeTimer = time;
            shakeTimeTotal = time;
        }
        else
        {
            Debug.LogError("CinemachineBasicMultiChannelPerlin component not found on the CinemachineVirtualCamera.");
        }
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (cinemachineBasicMultiChannelPerlin != null)
            {
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, targetIntensity, (  shakeTimer) / shakeTimeTotal);
                cinemachineBasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(startingFrequency, targetFrequency, (  shakeTimer) / shakeTimeTotal);
            }
        }
    }
}
