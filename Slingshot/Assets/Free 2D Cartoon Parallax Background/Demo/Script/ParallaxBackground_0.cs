//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ParallaxBackground_0 : MonoBehaviour
//{
//    public bool Camera_Move;
//    public float Camera_MoveSpeed = 1.5f;
//    float origcamspeed;
//    [Header("Layer Setting")]
//    public float[] Layer_Speed = new float[7];
//    public GameObject[] Layer_Objects = new GameObject[7];

//    private Transform _camera;
//    private float[] startPos = new float[7];
//    private float boundSizeX;
//    private float sizeX;
//    private GameObject Layer_0;
//    void Start()
//    {
//        if (PlayerPrefs.GetInt("level", 1) == 1)
//        {
//            Camera_MoveSpeed = 6.3f;
//            origcamspeed = Camera_MoveSpeed;
//        }
//        else if (PlayerPrefs.GetInt("level", 1) == 2)
//        {
//            Camera_MoveSpeed = 7f;
//            origcamspeed = Camera_MoveSpeed;

//        }
//        else
//        {
//            Camera_MoveSpeed = 8f;
//            origcamspeed = Camera_MoveSpeed;

//        }

//        _camera = Camera.main.transform;
//        sizeX = 1;
//        //Debug.Log(sizeX);
//        boundSizeX = Layer_Objects[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
//        //Debug.Log(boundSizeX);
//        for (int i=0;i<5;i++)
//        {
//            startPos[i] = _camera.position.x;
//        }
//    }

//    public IEnumerator CamSpeed()
//    {
//        while(Camera_MoveSpeed!=8.5f)
//        {
//            Camera_MoveSpeed = Mathf.Lerp(Camera_MoveSpeed, 8.5f, Time.deltaTime * 6f);
//            yield return null;
//        }
//    }


//    void Update(){
//        //Moving camera
//        if(!PlaneMovement.poweredUp)
//            Camera_MoveSpeed = Mathf.Lerp(Camera_MoveSpeed, origcamspeed, Time.deltaTime * 6f);
//        if (Camera_Move){
//        _camera.position += Vector3.right * Time.deltaTime * Camera_MoveSpeed;
//        }
//        for (int i=0;i<5;i++){
//            float temp = (_camera.position.x * (1-Layer_Speed[i]) );
//            float distance = _camera.position.x  * Layer_Speed[i];
//            Layer_Objects[i].transform.position = new Vector2 (startPos[i] + distance, _camera.position.y);
//            if (temp > startPos[i] + boundSizeX*sizeX - 10f){
//                startPos[i] += boundSizeX*sizeX ;
//            }else if(temp < startPos[i] - boundSizeX*sizeX){
//                startPos[i] -= boundSizeX*sizeX;
//            }
            
//        }
//    }
//}
