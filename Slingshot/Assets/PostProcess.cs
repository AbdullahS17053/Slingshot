using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcess : MonoBehaviour
{

    public static PostProcess Instance { get; private set; }

    public DepthOfField focus;

    public PostProcessVolume volume;

    public float focusDistance;

    private void Awake()
    {
        Instance = this;
        volume = GameObject.FindGameObjectWithTag("PostProcessing").GetComponent<PostProcessVolume>();
        if (volume != null && volume.profile.TryGetSettings(out focus))
        {
            
        }
        else
        {
           
        }
    }

    public void BlurScreenOn()
    {
        if (focus != null)
        {
            Debug.Log("Blur On");
            focus.focusDistance.value = focusDistance;
        }
    }

    public void BlurScreenOff()
    {
        if (focus != null)
        {
            Debug.Log("Blur Off");
            focus.focusDistance.value = 10f;
        }
    }
}
