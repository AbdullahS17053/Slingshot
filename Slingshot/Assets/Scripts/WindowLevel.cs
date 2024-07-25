using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowLevel : MonoBehaviour
{
    public GameObject[] windows;
    public GameObject[] loadingWindows;

    public Material originalParday;
    public Material[] parday;
    public Material[] loadingParday;

    public Material originalBackground;
    public Material[] backgroundMaterials;

    bool loading = false;

    public Text Score;
    int level = 0;
    public int levelChangeScore = 250;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        level = int.Parse(Score.text)/ levelChangeScore;

        if (level > 0 && !loading)
        {
            loadingWindows[level - 1].SetActive(false);
            loadingWindows[level].SetActive(true);
            originalBackground.color = backgroundMaterials[level].color;
            originalParday.color = loadingParday[level].color;
            loading = true;
            StartCoroutine(loadingLevel());
        }

        if (level > 0 && !loading)
        {
            windows[level].SetActive(true);
            originalBackground.color = backgroundMaterials[level].color;
            originalParday.color = parday[level].color;
        }
    }

    IEnumerator loadingLevel()
    {
        yield return new WaitForSeconds(5);
        loading = true;
    }
}
