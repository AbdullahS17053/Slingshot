using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete : MonoBehaviour
{
    public int perHitHealth = 10;
    private Lives life;
    private int food;
    private WindowLevel windowLevel;
    private MainSpawner mainSpawner;

    public Vector3 increasedScale = new Vector3(3f, 3f, 3f);
    public float bigDuration = 0.5f; 
    public float lerpDuration = 0.5f;
    void Start()
    {
        windowLevel = GameObject.FindGameObjectWithTag("GameManager").GetComponent<WindowLevel>();
        life = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Lives>();
        mainSpawner = FindObjectOfType<MainSpawner>();
        food = LayerMask.NameToLayer("GoodFood");

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("WindowObject") && !collision.gameObject.GetComponent<CustomSpeeds>().launched)
        {
            if (collision.gameObject.layer == food)
            {

             
                if (windowLevel.GetLevelName() == "Blue Windows" && mainSpawner.isPattern) // meaning if pattern is started in blue windows
                {
                    Destroy(collision.gameObject);
                }
                else {

                    StartCoroutine(LerpToZeroScaleRoutine(collision.gameObject));


                }

            }
        }
    }
    IEnumerator LerpToZeroScaleRoutine(GameObject target)
    {
        Destroy(target.GetComponent<CustomSpeeds>());
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        Vector3 initialScale = target.transform.localScale;

        yield return StartCoroutine(LerpScale(target.transform, initialScale, increasedScale, bigDuration));

        yield return StartCoroutine(LerpScale(target.transform, increasedScale, Vector3.zero, lerpDuration));

        CameraShake.Instance.ShakeCamera(1.2f, 5f, 0.5f);
        life.RemoveHealth(perHitHealth);
        Destroy(target);
    }

    IEnumerator LerpScale(Transform transform, Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = to;
    }
}
