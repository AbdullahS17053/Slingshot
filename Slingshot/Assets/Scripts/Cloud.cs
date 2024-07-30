using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private float minStrikeTime;
    [SerializeField] private float maxStrikeTime;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        

        StartCoroutine("Strike");
    }

    IEnumerator Strike()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minStrikeTime, maxStrikeTime));
            gameObject.GetComponent<AudioSource>().Play();
            anim.SetTrigger("strike");
        }
    }
}
