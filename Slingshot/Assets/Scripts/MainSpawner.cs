using Microlight.MicroBar;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public Image orderImage; // Single UI Image slot
    public Sprite[] fruitSprites; // Array of sprites for fruits

    private Dictionary<string, Sprite> fruitSpriteMap = new Dictionary<string, Sprite>();
    private Lives life;
    public MicroBar patterHealthBar;
    private int currrHealth;
    public TMP_Text healthtext;
    private int fruitNum;
    private int maxHealth;

    public void Start()
    {

        waiting = true;
        index = 0;
        life = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Lives>();
        patterHealthBar = GameObject.FindGameObjectWithTag("PatternHealthBar").GetComponent<MicroBar>();
        healthtext = GameObject.FindGameObjectWithTag("PatternHealthText").GetComponentInChildren<TMP_Text>();
        fruitNum = correctOrder.Count;
        currrHealth = correctOrder.Count * 10;
        maxHealth = currrHealth;
        patterHealthBar.Initialize(currrHealth);
        UpdateHealth(currrHealth);
        InitializeFruitSpriteMap();

        SpawnNextFruitInCanvas();

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


    public void RemoveHealth(int value)
    {

        currrHealth -= value;
        if (currrHealth < 0f) currrHealth = 0;
        //soundSource.clip = hurtSound;
        //if (soundOn) soundSource.Play();

        // Update HealthBar
        if (patterHealthBar != null) patterHealthBar.UpdateBar(currrHealth, false, UpdateAnim.Damage);
        //leftAnimator.SetTrigger("Damage");
        UpdateHealth(currrHealth);
    }

    private void UpdateHealth(int sc)
    {
        healthtext.text = sc.ToString() + "HP";
    }


    public bool OnFruitShot(GameObject fruit)
    {
        string fruitName = fruit.name.Replace("(Clone)", "").Trim();
        if (correctOrder[currentOrderIndex].name == fruitName)
        {
            RemoveHealth(maxHealth / fruitNum);
            StartCoroutine(HandleCorrectFruitShot());
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

    private IEnumerator HandleCorrectFruitShot()
    {
        // Temporarily turn the sprite image black
        orderImage.color = Color.black;

        // Wait for 0.5 seconds
        yield return new WaitForSeconds(0.5f);

        // Move to the next fruit in the queue
        currentOrderIndex++;

        // Check if the level is completed
        if (currentOrderIndex == correctOrder.Count)
        {
            LevelCompleted();
        }
        else
        {
            SpawnNextFruitInCanvas(); // Replace the sprite with the next fruit
        }
    }

    public bool LevelCompleted()
    {
        Debug.Log("Level Completed");
        return true;
    }
    public void LevelFailed() { }

    private void SpawnNextFruitInCanvas()
    {
        if (currentOrderIndex < correctOrder.Count)
        {
            string fruitName = correctOrder[currentOrderIndex].name.Replace("(Clone)", "").Trim();
            if (fruitSpriteMap.TryGetValue(fruitName, out Sprite fruitSprite))
            {
                orderImage.sprite = fruitSprite;
                orderImage.color = Color.white; // Make the image visible
            }
            else
            {
                // Optionally, handle the case where the sprite is not found
                orderImage.color = Color.black; // Set image to black as fallback
            }
        }
    }

    private void InitializeFruitSpriteMap()
    {
        // Populate the fruitSpriteMap dictionary
        for (int i = 0; i < fruitSprites.Length; i++)
        {
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
