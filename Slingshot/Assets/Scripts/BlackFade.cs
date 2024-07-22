using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackFade : MonoBehaviour
{
    // Start is called before the first frame update

    public float fadeDuration = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WindowObject"))
        {
            // Get the MeshRenderer component
            MeshRenderer meshRenderer = other.gameObject.GetComponent<MeshRenderer>();
            if (meshRenderer != null && meshRenderer.materials.Length > 1)
            {
                // Start the fade coroutine
                StartCoroutine(FadeAlpha(meshRenderer.materials[2], fadeDuration));
            }
        }
    }

    private IEnumerator FadeAlpha(Material material, float duration)
    {
        // Ensure the material has a color property
        if (material.HasProperty("_Color"))
        {
            Color startColor = material.color;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0); // Target color with alpha = 0
            float timeElapsed = 0f;

            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                float lerpValue = Mathf.Clamp01(timeElapsed / duration);
                material.color = Color.Lerp(startColor, endColor, lerpValue);
                yield return null;
            }

            // Ensure the final color is exactly the target color
            material.color = endColor;
        }
        else
        {
            Debug.LogWarning("Material does not have a _Color property.");
        }
    }
}
