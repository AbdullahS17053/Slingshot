using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCloud : MonoBehaviour
{
    [SerializeField] private GameObject rainCloudPrefab;

    [Range(0.0f, 100.0f)] public float spawnChance;

    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0f, 100f) < spawnChance)
        {
            //Debug.Log("Rain Cloud Spawned");
            Instantiate(rainCloudPrefab, transform.position - new Vector3(5, 0, 0), Quaternion.identity);
        }
    }
}
