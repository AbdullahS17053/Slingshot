using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    private GameObject currentBullet;
    public Transform spawnPoint;
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

        currentBullet = Instantiate(bulletPrefab,spawnPoint.position, transform.rotation);
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

    IEnumerator SpawnBulletAfterDelay()
    {
        yield return new WaitForSeconds(spawnTime);
        SpawnBullet();
    }
}
