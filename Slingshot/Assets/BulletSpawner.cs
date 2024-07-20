using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    private GameObject currentBullet;
    private CinemachineVirtualCamera vcam;
    private Vector3 initialCameraPosition;
    private Quaternion initialCameraRotation;

    void Start()
    {

        vcam = GameObject.FindGameObjectWithTag("vcam").GetComponent<CinemachineVirtualCamera>();
        initialCameraPosition = vcam.transform.position;
        initialCameraRotation = vcam.transform.rotation;
        SpawnBullet();
    }

    void SpawnBullet()
    {
        vcam.transform.position = initialCameraPosition;
        vcam.transform.rotation = initialCameraRotation;

        if (currentBullet != null)
        {
            Destroy(currentBullet);
        }

        currentBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        currentBullet.GetComponent<DragAndShoot>().OnShoot += HandleBulletShot;

        SetLookAtTarget(currentBullet.transform);


    }

    public void SetLookAtTarget(Transform target)
    {
        vcam.LookAt = target;
    }

    void HandleBulletShot()
    {
        StartCoroutine(SpawnNewBulletAfterDelay());
    }

    IEnumerator SpawnNewBulletAfterDelay()
    {
        yield return new WaitForSeconds(1f); 
        SpawnBullet();
    }
}
