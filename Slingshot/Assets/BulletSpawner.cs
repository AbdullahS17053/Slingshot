using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    private GameObject currentBullet;

    void Start()
    {
        SpawnBullet();
    }

    void SpawnBullet()
    {
        if (currentBullet != null)
        {
            Destroy(currentBullet);
        }

        currentBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        currentBullet.GetComponent<DragAndShoot>().OnShoot += HandleBulletShot;
    }

    void HandleBulletShot()
    {
        StartCoroutine(SpawnNewBulletAfterDelay());
    }

    IEnumerator SpawnNewBulletAfterDelay()
    {
        yield return new WaitForSeconds(1f); // Optional delay before spawning the next bullet
        SpawnBullet();
    }
}
