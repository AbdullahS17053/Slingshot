using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{ 
    public List<GameObject> gameObjects;
    public List<int> colNumbers;
    public List<int> waitTime;
    public Transform spawnPoint;
    bool waiting = false;
    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(index >= gameObjects.Count)
            index = 0;

        if(!waiting)
        {
            if (gameObjects[index] != null)
                Instantiate(gameObjects[index],spawnPoint);
            waiting = true;
            StartCoroutine(SleepCoroutine(waitTime[index]));
            index++;
        }

        //foreach (GameObject ObjectSpawned in SpawnedList)
        //{
        //    ObjectSpawned.transform.position += (Vector3.left * 2.5f) * Time.deltaTime;
        //    if (ObjectSpawned.transform.position.x < Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect) - 2f)
        //    {
        //        RemoveAbleObjects.AddLast(ObjectSpawned);
        //    }
        //}
    }

    IEnumerator SleepCoroutine(int time)
    {
        yield return new WaitForSeconds(time);
        waiting = false;
    }
}
