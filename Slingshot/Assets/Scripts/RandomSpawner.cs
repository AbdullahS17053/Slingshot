//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class RandomSpawner : MonoBehaviour
//{
//    static public int size = 2;
//    public static Vector3 lastCoin;
//    public static Vector3 lastHoop;
//    public GameObject ui;

//    [SerializeField]

//    //--------------------------------------
//    public bool Coins_Invoke_Check = true;
//    public bool Hoop_Invoke_Check = true;
//    //---------------------------------------

//    public GameObject[] ToSpawn;

//    //Interval Of Spawning.
//    public float[] SpawnInterval;
//    private float cameraWidth;
//    private float cameraHeight;
//    public float[] Ythreshold;
//    public PlaneMovement plane;
    


//    //Gets Random Position From Right Side of The Camera.
//    private Vector3 GetRandomPosition(float y)
//    {
//        //Orthographic Size is Height/2.
//        cameraHeight = Camera.main.orthographicSize;

//        //To Calculate Width Multiply Height/2 with Aspect (width divided By Height).
//        cameraWidth = Camera.main.aspect * cameraHeight;

//        //Set The X-position Of Spawn.
//        float Xpostion = Camera.main.transform.position.x + cameraWidth + 2f;

//        //Get The Random Value of Y-position of Spawn.
//        float Yposition = Random.Range(-cameraHeight + y, cameraHeight - y);

//        return new Vector3(Xpostion, Yposition, 0f); //Change this..
//    }

//    //Spawns The GameObject.
//    void SpawnCoins()
//    {
//        Vector3 SpawnPosition = GetRandomPosition(Ythreshold[0]);
//        //int index = Random.Range(0, NumberOfCollectibles);
//        //Place three Coins In A Row. If it is A Coin.
//        int count = 0;
//        do
//        {
//            // if (CheckOverlap(index)) break;
//            if (Coins_Invoke_Check)
//                Instantiate(ToSpawn[0], SpawnPosition, ToSpawn[0].transform.rotation);
//            lastCoin = SpawnPosition;
//            SpawnPosition.x += 3.5f;
//            count++;
//        } while (count != 3);
//        //lastCoin = SpawnPosition;
//    }

//    void SpawnHoops()
//    {
//        Vector3 SpawnPosition = GetRandomPosition(Ythreshold[1]);
//        if (Hoop_Invoke_Check)
//            Instantiate(ToSpawn[1], SpawnPosition, ToSpawn[1].transform.rotation);
//        lastHoop = SpawnPosition;
//    }

//    void Start()
//    {
//        if (PlayerPrefs.GetInt("level", 1) == 1)
//        {
           
           
//                SpawnInterval[0] = 3.5f;
          
//                SpawnInterval[1] = 2.5f;
           
//        }
//        else if (PlayerPrefs.GetInt("level", 1) == 2)
//        {
//            SpawnInterval[0] = 3.25f;

//            SpawnInterval[1] = 2f;
//        }
//        else
//        {
//            SpawnInterval[0] = 3.0f;

//            SpawnInterval[1] = 1.5f;
//        }
//        //Keeping On Invoking The Function After Some Interval of Time.
//        InvokeRepeating("SpawnCoins", 0f, SpawnInterval[0]);
//        InvokeRepeating("SpawnHoops", 0f, SpawnInterval[1]);

//    }

    

//    private void Update()
//    {
//        if(ui.activeSelf)
//        {
//            Destroy(gameObject);
//        }
        
//    }

//}