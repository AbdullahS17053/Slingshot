using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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
    private float startingHurtIntensity;
    private float targetingHurtIntensity;

    public Vignette vignette;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();

        // Initialize PostProcessing Volume
        PostProcessVolume volume = GameObject.FindGameObjectWithTag("PostProcessing").GetComponent<PostProcessVolume>();
        if (volume != null)
        {
            // Make sure vignette is in the volume profile
            if (volume.profile.TryGetSettings(out vignette))
            {
                vignette.intensity.value = 0;
                startingHurtIntensity = 0.5f;
                targetingHurtIntensity = 0.774f;
            }
            else
            {
                Debug.LogError("Vignette effect not found in PostProcessVolume profile.");
            }
        }
        else
        {
            Debug.LogError("PostProcessVolume component not found on GameObject with tag 'PostProcessing'.");
        }
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

            // Initialize vignette settings
            CallPostProcessing();
        }
        else
        {
            Debug.LogError("CinemachineBasicMultiChannelPerlin component not found on the CinemachineVirtualCamera.");
        }
    }

    public void CallPostProcessing()
    {
        // Make sure vignette is initialized before setting values
        if (vignette != null)
        {
            vignette.intensity.value = 0;
            HurtEffect();
        }
    }

    public void HurtEffect()
    {
        if (vignette != null)
        {
            StartCoroutine(FadeVignetteIntensity(startingHurtIntensity, targetingHurtIntensity, 0.2f));
        }
    }

    private IEnumerator FadeVignetteIntensity(float startValue, float endValue, float duration)
    {
        float elapsedTime = 0f;

        // Increase intensity to target
        while (elapsedTime < duration / 2)
        {
            elapsedTime += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(startValue, endValue, elapsedTime / (duration / 2));
            yield return null;
        }

        // Set to target intensity
        vignette.intensity.value = endValue;

        // Reduce intensity to 0
        elapsedTime = 0f;
        float EndingDuration = 2f;
        while (elapsedTime < EndingDuration)
        {
            elapsedTime += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(endValue, 0f, elapsedTime / (EndingDuration / 2));
            yield return null;
        }

        // Ensure intensity is exactly 0
        vignette.intensity.value = 0f;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (cinemachineBasicMultiChannelPerlin != null)
            {
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, targetIntensity, (shakeTimer) / shakeTimeTotal);
                cinemachineBasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(startingFrequency, targetFrequency, (shakeTimer) / shakeTimeTotal);
            }
        }
    }

}
