using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    private GameObject currentBullet;
    public DragAndShoot gun;
    public float spawnTime = 1f;

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

        currentBullet = Instantiate(bulletPrefab,transform.position,bulletPrefab.transform.rotation);
        currentBullet.transform.SetParent(transform);
        currentBullet.GetComponent<DragAndShoot>().OnShoot += HandleBulletShot;
    }

    void HandleBulletShot()
    {
       StartCoroutine(SpawnBulletAfterDelay());
    }

    public void levelChange(float delay)
    {
        StartCoroutine(SpawnBulletAfterLevelChange(delay));
    }

    IEnumerator SpawnBulletAfterDelay()
    {
        yield return new WaitForSeconds(spawnTime);
        SpawnBullet();
    }

    IEnumerator SpawnBulletAfterLevelChange(float time)
    {
        yield return new WaitForSeconds(time);
        SpawnBullet();
    }
}
