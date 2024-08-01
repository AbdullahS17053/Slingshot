//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ObstacleSpawner : MonoBehaviour
//{

//    public bool Invoke_Check = true;
//    public static Vector3 lastSpawn;
//    [SerializeField] 
//    private GameObject[] ToSpawn;
//    public GameObject[] ToSpawn1;
//    public GameObject[] ToSpawn2;
//    public GameObject[] ToSpawn3;
//    public GameObject ui;
//    Vector3 coin = RandomSpawner.lastCoin;
//    Vector3 hoop = RandomSpawner.lastHoop;

//    LinkedList<GameObject> SpawnedList = new LinkedList<GameObject>();
//    LinkedList<GameObject> RemoveAbleObjects = new LinkedList<GameObject>();
    
//    public float spawnDistanceThreshold = 2.0f;
//    //private LinkedList<GameObject> Addptr;

//    //Interval Of Spawning.
//    public float SpawnInterval = 2f;

//    //Gets Random Position From Right Side of The Camera.
//    private Vector2 GetRandomPosition()
//    {
//        //Orthographic Size is Height/2.
//        float cameraHeight = Camera.main.orthographicSize;

//        //To Calculate Width Multiply Height/2 with Aspect (width divided By Height).
//        float cameraWidth = Camera.main.aspect * cameraHeight;

//        //Set The X-position Of Spawn.
//        float Xpostion = Camera.main.transform.position.x + cameraWidth + 2f;

//        //Get The Random Value of Y-position of Spawn.
//        float Yposition = Random.Range(-cameraHeight + 2f, cameraHeight - 2f);

//        return new Vector2(Xpostion, Yposition);
//    }

//    //Spawns The GameObject.

//    //bool checkValues(Vector3 pos)
//    //{
//    //    if( 
//    //}
//    void Spawn()
//    {
//        coin = RandomSpawner.lastCoin;
//        hoop = RandomSpawner.lastHoop;
//        Vector2 SpawnPosition;
//        do
//        {
//            coin = RandomSpawner.lastCoin;
//            hoop = RandomSpawner.lastHoop;
//            SpawnPosition = GetRandomPosition();
//        }
//        while ((Vector2.Distance(SpawnPosition, lastSpawn) < spawnDistanceThreshold) || (Mathf.Abs(SpawnPosition.y- coin.y) < 1f) || (Mathf.Abs(SpawnPosition.y - coin.y) < 1f));
//        int index = Random.Range(0, ToSpawn.Length);

//        if (Invoke_Check)
//            SpawnedList.AddLast(Instantiate(ToSpawn[index], SpawnPosition, ToSpawn[index].transform.rotation));

//        lastSpawn = SpawnPosition;
//    }


//    private void UpdatePositions()
//    {

//        foreach (GameObject ObjectSpawned in SpawnedList)
//        {
//            ObjectSpawned.transform.position += (Vector3.left * 2.5f) * Time.deltaTime;
//            if (ObjectSpawned.transform.position.x < Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect) - 2f)
//            {
//                RemoveAbleObjects.AddLast(ObjectSpawned);
//            }
//        }

//        foreach (GameObject remove in RemoveAbleObjects)
//        {
//            SpawnedList.Remove(remove);
//            Destroy(remove);
//        }
//    }
//    /*  public bool CheckOverlap()
//      {

//          //Call this function to update collider's transform !!
//          Physics2D.SyncTransforms();

//          //Don't forget to get the colliders size, and not the renderer's !!
//          Vector2 boxSize = ToSpawn.GetComponent<Collider2D>().bounds.size;

//          var rads = 1; //Default Radians.
//          //Angle is in degrees !!
//          float angle = rads * Mathf.Rad2Deg;

//          var colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, angle);


//          if (colliders.Length == 0)
//              return false;

//          //check if collider is not the current's gameobject collider !!
//          else if (colliders.Length == 1)
//              if (colliders[0].gameObject == gameObject)
//                  return false;

//          return true;
//      } */

//    void Start()
//    {
//        //Keeping On Invoking The Function After Some Interval of Time.
//        if (PlayerPrefs.GetInt("level", 1) == 1)
//        {
//            ToSpawn = ToSpawn1;
//            SpawnInterval = 3.5f;
           

//        }
//        else if (PlayerPrefs.GetInt("level", 1) == 2)
//        {
//            ToSpawn = ToSpawn2;
//            SpawnInterval = 3f;
//        }
//        else
//        {
//            ToSpawn = ToSpawn3;
//            SpawnInterval = 2f;
//        }
//        InvokeRepeating("Spawn", Random.Range(3f, 10f), SpawnInterval);
//    }

    

//    void Update()
//    {
//        UpdatePositions();
//        if (ui.activeSelf)
//        {
//            Destroy(gameObject);
//        }
//    }

   
//}