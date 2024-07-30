using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balloonScript : MonoBehaviour
{
    public Animator anim;
    public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DoNothing"))
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            anim.SetTrigger("pop");
            StartCoroutine(balloonWait());
        }
    }

    IEnumerator balloonWait()
    {
        audio.Play();
        yield return new WaitForSeconds(0.517f);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
