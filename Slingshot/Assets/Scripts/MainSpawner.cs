using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
  
    [Header("UI Settings")]
    public List<Image> orderImages = new List<Image>(); // List of UI Images
    public Sprite[] fruitSprites; // Array of sprites for fruits

    private Dictionary<string, Sprite> fruitSpriteMap = new Dictionary<string, Sprite>();
    private Lives life;

    public void Start()
    {
        waiting = true;
        index = 0;
        life = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Lives>();
        InitializeFruitSpriteMap();

        SpawnCorrectOrderFruitsInCanvas();  //spawn the images of the fruits in order.

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

    public bool OnFruitShot(GameObject fruit)
    {
        string fruitName = fruit.name.Replace("(Clone)", "").Trim();
        if (correctOrder[currentOrderIndex].name == fruitName)
        {

            // delete the image from the canvas

            if (currentOrderIndex < orderImages.Count)
            {
                //Destroy(orderImages[currentOrderIndex].gameObject);
                orderImages[currentOrderIndex].color = Color.black;
            }

            currentOrderIndex++;
            Debug.Log("Correct Fruit");

            if (currentOrderIndex == correctOrder.Count)
            {
                LevelCompleted();
            }

            return true;
        }
        else
        {
            Debug.Log("Wrong Fruit");
            life.RemoveHealth(20);
            LevelFailed();
            return false;
        }
    }


    public bool LevelCompleted() {

        return true;
    }
    public void LevelFailed() { }

    public void SpawnCorrectOrderFruitsInCanvas()
    {
        for (int i = 0; i < correctOrder.Count && i < orderImages.Count; i++)
        {
            string fruitName = correctOrder[i].name.Replace("(Clone)", "").Trim();
            if (fruitSpriteMap.TryGetValue(fruitName, out Sprite fruitSprite))
            {
                orderImages[i].sprite = fruitSprite;
            }
            else
            {
                //orderImages[i].sprite = defaultSprite; // Fallback to a default sprite if the fruit name is not found
                orderImages[i].color = Color.black;
            }
        }
    }

    private void InitializeFruitSpriteMap()
    {
        // Populate the fruitSpriteMap dictionary
        // Make sure fruitSprites array corresponds to fruit names properly
        for (int i = 0; i < fruitSprites.Length; i++)
        {
            // Assuming fruit names are unique and match sprites names
            // For example, "Apple" should match with "Apple" sprite
            fruitSpriteMap[fruitSprites[i].name] = fruitSprites[i];
        }
    }




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
