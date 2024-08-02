using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSpawner : MonoBehaviour
{
    public float initialWait = 2f;
    public List<GameObject> gameObjects;
    public List<int> colNumbers;
    public List<float> waitTime;
    bool waiting = false;
    int index = 0;

    [Header("Order Settings")]
    public List<GameObject> correctOrder = new List<GameObject>();
    private int currentOrderIndex = 0;

    [Header("Spawn Settings")]
    public GameObject spawnContainer;
    public List<Transform> spawnPoints = new List<Transform>();

    public void Start()
    {
        waiting = true;
        index = 0;

        foreach (Transform child in spawnContainer.transform)
        {
            spawnPoints.Add(child);
        }

        SpawnCorrectOrderFruits();

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

    public void OnFruitShot(GameObject fruit)
    {
        string fruitName = fruit.name.Replace("(Clone)", "").Trim();
        if (correctOrder[currentOrderIndex].name == fruitName)
        {
            currentOrderIndex++;
            Debug.Log("Correct Fruit");

            if (currentOrderIndex == correctOrder.Count)
            {
                LevelCompleted();
            }
        }
        else
        {
            Debug.Log("Wrong Fruit");
            LevelFailed();
        }
    }

    public void SpawnCorrectOrderFruits()
    {
        for (int i = 0; i < correctOrder.Count; i++)
        {
            if (i < spawnPoints.Count)
            {
                GameObject fruit = Instantiate(correctOrder[i], spawnPoints[i].position, spawnPoints[i].rotation);
                fruit.transform.SetParent(spawnContainer.transform);

                Collider[] colliders = fruit.GetComponents<Collider>();
                foreach (Collider collider in colliders)
                {
                    collider.enabled = false;
                }

                Rigidbody rb = fruit.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = fruit.AddComponent<Rigidbody>();
                }

                rb.constraints = RigidbodyConstraints.FreezePosition |
                                 RigidbodyConstraints.FreezeRotationX |
                                 RigidbodyConstraints.FreezeRotationZ;

                Destroy(fruit.GetComponent<CustomSpeeds>());
                Renderer renderer = fruit.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material[] materials = renderer.materials;
                    List<Material> materialsList = new List<Material>(materials);

                    Material materialToRemove = materialsList.Find(m => m.name == "BlackedOut");

                    if (materialToRemove != null)
                    {
                        materialsList.Remove(materialToRemove);
                        renderer.materials = materialsList.ToArray();
                        Destroy(materialToRemove);
                    }
                }

                fruit.AddComponent<Rotator>().rotationSpeed = new Vector3( 0f, 20f, 0f);
            }
            else
            {
                break;
            }
        }
    }

    public void LevelCompleted() { }
    public void LevelFailed() { }

    IEnumerator SleepCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        waiting = false;
    }

    public void destroyAll()
    {
        for (int i = 0; i < 5; i++)
        {
            transform.GetChild(i).GetComponent<Spawner>().destroyItem();
        }
    }
}

public class Rotator : MonoBehaviour
{
    public Vector3 rotationSpeed;

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
