using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    private GameObject currentBullet;
    //public CinemachineFreeLook vcam;
    public DragAndShoot gun;
    public float spawnTime = 1f;

    void Start()
    {
        //vcam = GameObject.FindGameObjectWithTag("vcam").GetComponent<CinemachineFreeLook>();
        SpawnBullet();
    }

    void SpawnBullet()
    {
        if (currentBullet != null)
        {
            Destroy(currentBullet);
        }

        currentBullet = Instantiate(bulletPrefab,transform.position, transform.rotation);
        currentBullet.transform.SetParent(transform);
        //currentBullet.transform.SetParent(spawnPoint.transform,true);
        //gun.setBullet(currentBullet);
        currentBullet.GetComponent<DragAndShoot>().OnShoot += HandleBulletShot;
        //SetLookAtTarget(currentBullet.transform);
    }

    //public void SetLookAtTarget(Transform target)
    //{
    //    vcam.LookAt = target;
    //}

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
