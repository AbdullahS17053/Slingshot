using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HoopCollision : MonoBehaviour
{

    private ChildHoop Middle;
    public Animator anim;
    float NTime;
    AnimatorStateInfo animStateInfo;
    bool animationFinished;
    bool play = false;
    public Animator anim1;
    public Animator anim2;
    public AudioSource audioSource;

    public float deleteDelay = 2.0f; // Delay in seconds
    // Start is called before the first frame update
    void Start()
    {
        Middle = transform.Find("MiddleCollider").GetComponent<ChildHoop>();
        Middle.OnTriggerEnter2D_Action += MiddleTrigger;
    }

    IEnumerator waitforanim()
    {
        yield return new WaitForSeconds(0.517f);
        gameObject.SetActive(false);
    }
    private void DeleteHoop()
    {
        audioSource.Play();
        anim1.SetTrigger("pop");
        anim2.SetTrigger("pop");
        StartCoroutine(waitforanim());
    }
    private void MiddleTrigger(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Invoke("DeleteHoop", deleteDelay);
        }
            
            
    }
    // Update is called once per frame
    void Update()
    {
        
        
    }
}
