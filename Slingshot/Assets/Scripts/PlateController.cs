using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateController : MonoBehaviour
{
    public GameObject outline;
    public GameObject fragments;
    public float destroyWait = 5f;
    // Start is called before the first frame update
    void Start()
    {
        outline.SetActive(true);
        fragments.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OutlineHit()
    {
        fragments.SetActive(true);
        StartCoroutine(DestroyAfter());
    }

    IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(destroyWait);
    }
}
