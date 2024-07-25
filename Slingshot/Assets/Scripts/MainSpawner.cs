using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public float initialWait = 2f;
    public List<GameObject> gameObjects;
    public List<int> colNumbers;
    public List<float> waitTime;
    bool waiting = false;
    int index = 0;
    // Start is called before the first frame update
    public void Start()
    {
        waiting = true;
        index = 0;
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("WindowObject");

        // Loop through each object and destroy it
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }
        StartCoroutine(SleepCoroutine(initialWait));
    }

    // Update is called once per frame
    void Update()
    {

        if (!waiting)
        {
            if (index >= colNumbers.Count)
            {
                index = 0;
                waiting = true;
                StartCoroutine(SleepCoroutine(initialWait));
                return;
            }
            transform.GetChild(colNumbers[index] - 1).GetComponent<Spawner>().item = gameObjects[index];
            waiting = true;
            StartCoroutine(SleepCoroutine(waitTime[index]));
            index++;
            
        }
    }

    IEnumerator SleepCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        waiting = false;
    }
}
