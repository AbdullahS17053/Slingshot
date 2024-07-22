using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowWindow : MonoBehaviour

{
    public Material outline;
    public Material glow;
    public MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetOn()
    {
        Material[] currentMaterials = meshRenderer.materials;
        currentMaterials[1] = glow;
        meshRenderer.materials = currentMaterials;
    }

    public void SetOff()
    {
        Material[] currentMaterials = meshRenderer.materials;
        currentMaterials[1] = outline;
        meshRenderer.materials = currentMaterials;
    }
}
