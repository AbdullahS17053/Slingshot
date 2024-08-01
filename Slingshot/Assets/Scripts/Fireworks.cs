//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Fireworks : MonoBehaviour
//{
//    public GameObject[] fireWorks;
//    bool won = false;
//    bool sling = false;
//    bool active = false;
//    // Start is called before the first frame update
//    private void Awake()
//    {
        
//    }
//    void Start()
//    {
//        for (int i = 0; i < fireWorks.Length; i++)
//        {
//            fireWorks[i].SetActive(false);
//        }
//        won = PlaneMovement.GameWon;
//    }

//    private void helperFirework(GameObject obj)
//    {
//        obj.SetActive(true);
//        //obj.GetComponentInChildren<AudioSource>().Play();
//        obj.GetComponentInChildren<ParticleSystem>().Play();
//    }
//    // Update is called once per frame
//    void Update()
//    {
//        won = PlaneMovement.GameWon;
//        sling = PlaneMovement.sling;
//        if(won && !active && !sling)
//        {
//            active = true;
//            for(int i = 0;i < 3; i++)
//            {
//                helperFirework(fireWorks[i]);
//            }
//        }
//    }
//}
