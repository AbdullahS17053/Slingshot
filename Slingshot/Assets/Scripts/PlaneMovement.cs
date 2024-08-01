//using System.Collections;
//using System.Collections.Generic;
//using Unity.Mathematics;
//using Unity.VisualScripting;
////using UnityEditor.U2D;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//using static UnityEngine.GraphicsBuffer;

//public class PlaneMovement : MonoBehaviour
//{
//    //Get Pause UI
//    // public GameObject PauseMenuUI;

//    //Get Game Over UI.
//    public static bool poweredUp = false;
//    float origrotspeed;
//    bool trig = true;
//    bool timer = false;
//    int curr = 0;
//    public GameObject hoopText;
//    public Animator anim;
//    public GameObject fumes;
//    public GameObject GameOverUI;
//    public GameObject GameWonUI;
//    public GameObject scoreMenu;
//    public AudioSource glide;
//    public AudioSource win;
//    public AudioSource clock;
//    public AudioSource power;
//    public Color goldenColor;
//    Color origcolor;
//    public AudioSource lose;
//    public AudioSource rubber;
//    public AudioSource fire;
//    public AudioSource crumple;
//    public ParallaxBackground_0 controller;
//    static public bool sling = false;
//    bool cam = false;
//    bool camMoved = false;
//    Vector2 slingTemp;
//    public SpriteRenderer Redbar;
//    float CameraWidth;
//    bool fall = false;

//    //Variables Of Tutorial..

//    //-------------------------------------
//    public GameObject Tutorial_UI;
//    public TMPro.TextMeshProUGUI Hoop_Text_Animation;
//    public GameObject Enemies_Spawner;
//    public GameObject Tut_Hoop;
//    public GameObject Tut_Cloud;
//    public GameObject Collectables_Spawner;
//    private bool FirstTime = true;
//    private bool ActiveStatus = false;
//    //------------------------------------

//    //Game Over Varaible.
//    public bool GameOvered = false;

//    //Game paused Variable.
//    private bool GamePaused = false;
//    static public bool GameWon = false;
//    bool balloonFlag = false;
//    int balloonCounter = 0;
//    bool inBar = false;

//    private bool Invisble_Plane = false;
//    public DrawWithMouse drawController;
//    Vector3[] positions;
//    bool startMovement = false;
//    public float speed = 10f;
//    float origspeed;
//    public float acc;
//    int totalCount = 0;
//    int moveIndex = 0;
//    bool colliderCheck = false;
//    bool justMoved = true;
//    int coincount = 0;
//    Vector2 dir;
//    float angle;
//    int count = 0;
//    bool angleMove = false;
//    Vector3 bottomCam;
//    //UnityEngine.Camera cam;

//    [SerializeField] public Text ScoreText;
//    public Color levelTwo;
//    public Color levelThree;
//    public Color trailTwo;
//    public Color trailThree;

//    float rotspeed;

//    [SerializeField] public Text TotalHoops;
//    [SerializeField] public Text CoinText;
//    [SerializeField] public Text HoopText;

//    private int Coins = 0;
//    private int Hoops = 0;
//    public CoinSlider slider;

//    private void Awake()
//    {
//        //drawController = new DrawWithMouse();
        
//        Redbar.color = new Color(Redbar.color.r, Redbar.color.g, Redbar.color.b, 0);
//        if (PlayerPrefs.GetInt("level", 1) == 1)
//        {
//            FirstTime = true;
//            Tutorial_UI.SetActive(false);
//            Tut_Cloud.SetActive(false);
//            Tut_Hoop.SetActive(false);
//            TotalHoops.text = "/5";

//        }
//        else if (PlayerPrefs.GetInt("level", 1) == 2)
//        {
//            FirstTime = false;
//            hoopText.transform.position = new Vector3(hoopText.transform.position.x -22f, hoopText.transform.position.y, hoopText.transform.position.z);
//            ActiveStatus = false;
//            Tutorial_UI.SetActive(false);
//            Tut_Hoop.SetActive(false);
//            Tut_Cloud.SetActive(false);
//            TotalHoops.text = "/10";
//        }
//        else
//        {
//            FirstTime = false;
//            hoopText.transform.position = new Vector3(hoopText.transform.position.x - 22f, hoopText.transform.position.y, hoopText.transform.position.z);
//            ActiveStatus = false;
//            Tutorial_UI.SetActive(false);
//            Tut_Cloud.SetActive(false);
//            Tut_Hoop.SetActive(false);
//            TotalHoops.text = "/15";
//        }
//        Time.timeScale = 1.0f;
//        glide.Stop();
//    }
//    private void Start()
//    {
//        poweredUp = false;
//        bottomCam = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - Camera.main.orthographicSize, 0f);
//        if (PlayerPrefs.GetInt("level", 1) == 1)
//        {
//            speed = 24f;
//            origspeed = speed;
//            rotspeed = 13f;
//            origrotspeed = rotspeed;

//            //-------------------------------------------
//            if (FirstTime && PlayerPrefs.GetInt("tut",0) == 0)
//            {
//                PlayerPrefs.SetInt("tut", 1);
//                //Debug.Log("First Time");
//                Vector2 Plane_init_Pos = new Vector2(-8f, 2.64f);
//                transform.position = Plane_init_Pos;
//                Tutorial_UI.SetActive(true);
//                Tut_Hoop.SetActive(true);
//                Tut_Cloud.SetActive(true);
//                Enemies_Spawner.GetComponent<ObstacleSpawner>().Invoke_Check = false;
//                Collectables_Spawner.GetComponent<RandomSpawner>().Coins_Invoke_Check = false;
//                // RandomSpawner HoopSpwaner = Collectables_Spawner.GetComponents<RandomSpawner>()[1];
//                // HoopSpwaner.Hoop_Invoke_Check = false;
//                Collectables_Spawner.GetComponent<RandomSpawner>().Hoop_Invoke_Check = false;
//                controller.Camera_Move = false;
//                FirstTime = false;
//                ActiveStatus = true;
//                scoreMenu.SetActive(false);
//            }
//            //--------------------------------------------

//        }
//        else if (PlayerPrefs.GetInt("level", 1) == 2)
//        {
//            speed = 30f;
//            origspeed = speed;
//            rotspeed = 15f;
//            origrotspeed = rotspeed;

//            gameObject.GetComponent<SpriteRenderer>().color = levelTwo;
//            gameObject.GetComponent<LineRenderer>().startColor = trailTwo;
//            gameObject.GetComponent<LineRenderer>().endColor = trailTwo;

//        }
//        else
//        {
//            speed = 35f;
//            origspeed = speed;
//            rotspeed = 17f;
//            origrotspeed = rotspeed;

//            gameObject.GetComponent<SpriteRenderer>().color = levelThree;
//            gameObject.GetComponent<LineRenderer>().startColor = trailThree;
//            gameObject.GetComponent<LineRenderer>().endColor = trailThree;
//        }
//        origcolor = gameObject.GetComponent<SpriteRenderer>().color;
//        //cam = UnityEngine.Camera.main;
//    }

//    private void OnMouseDown()
//    {
//        anim.StopPlayback();
//        curr = 0;
//    }
//    private void OnMouseDrag()
//    {
        
//        if (!gameObject.GetComponent<LineRenderer>().enabled)
//        {
//            return;
//        }
        
//        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//        if (!GetComponentInChildren<BoxCollider2D>().OverlapPoint(mousePosition) || colliderCheck)
//        {
//            if (!colliderCheck)
//            {
//                drawController.startLine(transform.position);

//            }
//            drawController.UpdateLine();
//            colliderCheck = true;
//            //Debug.Log("Called");
//            //GetComponent<Collider2D>().IsSceneBound
//        }
//    }

//    private void OnMouseUp()
//    {

//        if (!gameObject.GetComponent<LineRenderer>().enabled)
//        {
//            return;
//        }
//        totalCount = drawController.line.positionCount;
//        positions = new Vector3[drawController.line.positionCount];
//        drawController.line.GetPositions(positions);



//        //------------------------------------------------
//        if (!ActiveStatus)
//        {
//            startMovement = true;
//            glide.volume = 0f;
//            glide.Play();
//            StartCoroutine(StartFade(glide, 0.5f, 0.5f));
//        }
//        else if (CheckHoopPass())
//        {
//            Tutorial_UI.SetActive(false);
//            scoreMenu.SetActive(true);
//            Hoop_Text_Animation.gameObject.SetActive(true);
//            controller.Camera_Move = true;
//            Enemies_Spawner.GetComponent<ObstacleSpawner>().Invoke_Check = true;
//            Collectables_Spawner.GetComponent<RandomSpawner>().Coins_Invoke_Check = true;
//            Collectables_Spawner.GetComponent<RandomSpawner>().Hoop_Invoke_Check = true;
//            startMovement = true;
//            glide.volume = 0f;
//            glide.Play();
//            StartCoroutine(StartFade(glide, 0.5f, 0.5f));
//            ActiveStatus = false;
//        }

//        acc = 0.005f;
//        moveIndex = 0;
//        if (moveIndex > positions.Length - 1 || !colliderCheck)
//        {
//            startMovement = false;
//        }
//        colliderCheck = false;

//    }

//    //--------------------------------------------------
//    bool CheckHoopPass()
//    {
//        int positions = drawController.line.positionCount;

//        for (int i = 0; i < positions - 1; i++)
//        {
//            Vector3 startPoint = drawController.line.GetPosition(i);
//            Vector3 EndPoint = drawController.line.GetPosition(i + 1);

//            Vector2 startPoint2D = new Vector2(startPoint.x, startPoint.y);
//            Vector2 endPoint2D = new Vector2(EndPoint.x, EndPoint.y);

//            Collider2D[] colliders = Physics2D.OverlapCapsuleAll(startPoint2D, endPoint2D, CapsuleDirection2D.Horizontal, drawController.line.endWidth - 2f);

//            foreach (Collider2D collider in colliders)
//            {
//                if (collider.gameObject.CompareTag("Hoop") && endPoint2D.x >= 11f && (endPoint2D.y > -1f && endPoint2D.y < 4f))
//                {
//                    return true;
//                }
//            }
//        }
//        return false;
//    }

//    //---------------------------------------------


//    private IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
//    {
//        float currentTime = 0;
//        float start = audioSource.volume;
//        while (currentTime < duration)
//        {
//            currentTime += Time.deltaTime;
//            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
//            if(audioSource.volume == 0 && targetVolume == 0f)
//            {
//                audioSource.Stop();
//            }
//            yield return null;
//        }
//        yield break;
//    }

//    public void OnBecameInvisible()
//    {
//        if (!GameOvered && !GamePaused && !GameWon && GameOverUI != null && !Invisble_Plane)
//        {
//            if (transform.position.x < Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect)
//            {
//                lose.Play();
//                Time.timeScale = 0f;
//                GameOverUI.SetActive(true);
//                GameOvered = true;
//            }
//        }
//    }


//    IEnumerator wait(float s)
//    {
//        clock.PlayDelayed(s - 2.5f);
//        yield return new WaitForSecondsRealtime(s);
//        timer = false;
//        poweredUp = false;
//        speed = origspeed;
//        rotspeed = origrotspeed;
//        gameObject.GetComponent<SpriteRenderer>().color = origcolor;
//        anim.SetTrigger("idle");
//        power.Stop();
//        fumes.SetActive(false);
//        clock.Stop();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if ((transform.position.y > Camera.main.transform.position.y + Camera.main.orthographicSize) || (transform.position.y < Camera.main.transform.position.y - Camera.main.orthographicSize))
//        {
//            lose.Play();
//            Time.timeScale = 0f;
//            GameOverUI.SetActive(true);
//            GameOvered = true;
//            Invisble_Plane = true;
//        }

//        if (GameOverUI.activeSelf || GameWonUI.activeSelf)
//        {
//            scoreMenu.SetActive(false);
//            slider.GameObject().SetActive(false);
//        }

//        if (!inBar)
//        {
//            float Alpha = Redbar.color.a - (1f * Time.deltaTime);
//            if (Alpha < 0f)
//                Alpha = 0f;
//            Redbar.color = new Color(Redbar.color.r, Redbar.color.g, Redbar.color.b, Alpha);
//        }
//        else
//        {
//            float Alpha = Redbar.color.a + (0.6f * Time.deltaTime);
//            if (Alpha > 1f)
//                Alpha = 1f;
//            Redbar.color = new Color(Redbar.color.r, Redbar.color.g, Redbar.color.b, Alpha);
//        }

//        if (fall)
//        {
//            if (!camMoved)
//            {
//                if (sling && transform.position.x != slingTemp.x)
//                {
//                    //Camera.main.transform.position = Vector2.MoveTowards(Camera.main.transform.position, transform.position, 10f * Time.deltaTime);
//                    transform.position = Vector2.MoveTowards(transform.position, slingTemp, 10f * Time.deltaTime);
//                    transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.Euler(0f, 0f, 30f), 12f * Time.deltaTime);
//                    Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 5f, 2.5f * Time.deltaTime);
//                }
//                else
//                {
//                    sling = false;

//                    //StartCoroutine(wait(20f));
//                }
//                if (GameOvered && !sling)
//                {
//                    //Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 10f, 4f * Time.deltaTime);
//                    transform.position += (Vector3.down * 7f * Time.deltaTime);
//                    transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.Euler(0f, 0f, -90f), 0.5f * Time.deltaTime);
//                    Camera.main.transform.position = Vector2.MoveTowards(Camera.main.transform.position, transform.position, 10f * Time.deltaTime);
//                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -10f);

//                    if (transform.position.y <= bottomCam.y && !GameOverUI.active)
//                    {
//                        Time.timeScale = 0f;
//                        lose.Play();
//                        GameOverUI.SetActive(true);
//                    }
//                }
//            }
//            else
//            {
//                if (Camera.main.transform.position.x != transform.position.x && Camera.main.transform.position.y != transform.position.y)
//                {
//                    Camera.main.transform.position = Vector2.MoveTowards(Camera.main.transform.position, transform.position, 10f * Time.deltaTime);
//                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -10f);

//                }
//                else
//                {
//                    //camMoved = true;
//                }
//            }

//        }

//        if (cam)
//        {
//            if (camMoved)
//            {
//                if (sling && transform.position.x != slingTemp.x)
//                {
//                    //Camera.main.transform.position = Vector2.MoveTowards(Camera.main.transform.position, transform.position, 10f * Time.deltaTime);
//                    transform.position = Vector2.MoveTowards(transform.position, slingTemp, 10f * Time.deltaTime);
//                    transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.Euler(0f, 0f, 45f), 12f * Time.deltaTime);
//                    Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 5f, 2.5f * Time.deltaTime);
//                }
//                else 
//                {
//                    if(sling)
//                    {
//                        sling = false;
//                        glide.Play();
//                        StartCoroutine(StartFade(glide, 0.5f, 0.25f));

//                    }
                    
                    

//                    //StartCoroutine(wait(20f));
//                }
//                if (GameWon && !sling)
//                {

//                    //StartCoroutine(StartFade(glide, 0.5f, 0f));
//                    Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 10f, 4f * Time.deltaTime);
//                    transform.position += (Vector3.right * 20f * 1.5f) * Time.deltaTime;

//                    if ((transform.position.x > 2.5f + Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect) && !GameWonUI.active)
//                    {
//                        Time.timeScale = 1f;
//                        //win.Play();
//                        GameWonUI.SetActive(true);
//                        GameWon = false;
//                    }
//                }
//            }
//            else
//            {
//                if (Camera.main.transform.position.x != transform.position.x && Camera.main.transform.position.y != transform.position.y)
//                {
//                    //transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.Euler(0f, 0f, 0f), 6f * Time.deltaTime);
//                    Camera.main.transform.position = Vector2.MoveTowards(Camera.main.transform.position, transform.position, 10f * Time.deltaTime);
//                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -10f);

//                }
//                else
//                {
//                    camMoved = true;
//                    rubber.Play();
//                }
//            }

//        }

//        if (gameObject.GetComponent<LineRenderer>().enabled)
//        {
//            if (balloonFlag)
//            {
//                Vector2 temp = transform.position;
//                temp.x -= 0.25f;
//                transform.position = Vector2.MoveTowards(transform.position, temp, speed * Time.deltaTime);
//                transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.Euler(0f, 0f, 45f), 16f * Time.deltaTime);
//                balloonCounter++;
//                if (balloonCounter > 15)
//                {
//                    balloonFlag = false;
//                    balloonCounter = 0;
//                }
//            }



//            if (startMovement)
//            {


//                Vector2 currentpos = positions[moveIndex];
//                if (moveIndex < positions.Length / 2)
//                {
//                    acc += 0.08f;
//                }
//                else if (moveIndex < positions.Length * 3 / 4)
//                {
//                    acc += 0.16f;
//                }
//                else if (acc - 0.004 > 0)
//                {
//                    acc -= 0.08f;
//                }




//                transform.position = Vector2.MoveTowards(transform.position, currentpos, (speed + acc) * Time.deltaTime);

//                if (justMoved)
//                {
//                    angleMove = false;
//                    dir = currentpos - (Vector2)transform.position;
//                    angle = Mathf.Atan2(dir.normalized.y, dir.normalized.x);
//                    //Debug.Log(angle * Mathf.Rad2Deg);
//                    if (Mathf.Abs(angle * Mathf.Rad2Deg) >= 15f)
//                    {
//                        angleMove = true;
//                    }
//                }
//                if (angleMove)
//                    transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg), rotspeed * Time.deltaTime);

//                float distance = Vector2.Distance(transform.position, currentpos);
//                if (distance < 0.05f)
//                {
//                    moveIndex++;
//                    justMoved = true;
//                }
//                else
//                {
//                    justMoved = false;
//                }

//                if (moveIndex > positions.Length - 1)
//                {
//                    startMovement = false;
//                    speed = origspeed;
//                    StartCoroutine(StartFade(glide, 0.5f, 0f));
                    
                    
//                    // transform.rotation = Quaternion.Euler(0f, 0f, 0f);
//                }

//            }

            
//        }

//        if (!balloonFlag && !fall && !startMovement)
//        {
//            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.Euler(0f, 0f, 0f), 6f * Time.deltaTime);
//        }

//        if (poweredUp && !timer)
//        {
//            //Debug.Log("poweredup");
//            fumes.SetActive(true);
//            power.Play();
//            anim.SetTrigger("golden");
//            StartCoroutine(wait(8));
//            StartCoroutine(controller.CamSpeed());
//            timer = true;
//            //gameObject.GetComponent<SpriteRenderer>().color = goldenColor;

//        }
//        if (timer)
//        {
//            speed = Mathf.Lerp(speed, 40f, Time.deltaTime * 5f);
//            rotspeed = Mathf.Lerp(rotspeed, 19f, Time.deltaTime * 5f);
//        }

//    }

//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Bar"))
//        {
//            inBar = false;
//        }


//    }
//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (GameWon)
//        {
//            return;
//        }

//        if(!poweredUp)
//        {
//            if (collision.CompareTag("Cloud") && !fall)
//            {
//                anim.SetTrigger("fire");
//                fire.Play();
//                fall = true;
//                StartCoroutine(StartFade(glide, 0.5f, 0f)); ;
//                controller.Camera_Move = false;
//                sling = true;
//                startMovement = false;
//                bottomCam = new Vector3(Camera.main.transform.position.x, -6f + Camera.main.transform.position.y - Camera.main.orthographicSize, 0f);
//                GameOvered = true;
//                slingTemp = transform.position;
//                slingTemp.x -= 4.5f;
//                gameObject.GetComponent<LineRenderer>().enabled = false;
//            }

//            if (collision.CompareTag("Bird") && !fall)
//            {
//                anim.SetTrigger("crumple");
//                crumple.Play();
//                fall = true;
//                StartCoroutine(StartFade(glide, 0.5f, 0f)); ;
//                controller.Camera_Move = false;
//                sling = true;
//                startMovement = false;
//                bottomCam = new Vector3(Camera.main.transform.position.x, -6f + Camera.main.transform.position.y - Camera.main.orthographicSize, 0f);
//                GameOvered = true;
//                slingTemp = transform.position;
//                slingTemp.x -= 4.5f;
//                gameObject.GetComponent<LineRenderer>().enabled = false;
//            }

//            if (collision.CompareTag("Bar"))
//            {
//                inBar = true;
//            }

//            if (collision.CompareTag("Balloon"))
//            {
//                balloonFlag = true;
//                startMovement = false;
//                StartCoroutine(StartFade(glide, 0.5f, 0f)); ;
//            }
//        }

//        if (collision.gameObject.tag == "Collectible" && coincount == 0)
//        {
//            coincount++;
//            Coins++;
//            if(!poweredUp)
//                slider.CollectCoin();
//            CoinText.text = Coins.ToString();
//        }

//        else if (collision.gameObject.tag == "Collectible" && coincount == 1)
//        {
//            coincount = 0;
//        }

//        if (collision.gameObject.tag == "Hoop" && count == 0)
//        {
//            count++;

            

//        }

//        else if (collision.gameObject.tag == "Hoop" && count == 1)
//        {
//            Hoops++;
//            HoopText.text = Hoops.ToString();
//            if (PlayerPrefs.GetInt("level", 1) == 1)
//            {
//                if (Hoops == 5 && (!GameOvered && !GamePaused && !GameWon && GameOverUI != null))
//                {
//                    gameObject.GetComponent<LineRenderer>().enabled = false;

//                    StartCoroutine(StartFade(glide, 0.5f, 0f));;
//                    //StartCoroutine(wait(0.267f));
                    
//                    controller.Camera_Move = false;
//                    sling = true;
//                    startMovement = false;
//                    GameWon = true;
//                    slingTemp = transform.position;
//                    cam = true;
//                    slingTemp.x -= 8f;
//                }

//            }
//            else if (PlayerPrefs.GetInt("level", 1) == 2)
//            {
//                if (Hoops == 10 && (!GameOvered && !GamePaused && !GameWon && GameOverUI != null))
//                {
//                    gameObject.GetComponent<LineRenderer>().enabled = false;
//                    StartCoroutine(StartFade(glide, 0.5f, 0f));;
//                    controller.Camera_Move = false;
//                    sling = true;
//                    startMovement = false;
//                    GameWon = true;
//                    cam = true;
//                    slingTemp = transform.position;
//                    slingTemp.x -= 8f;
//                }
//            }
//            else
//            {
//                if (Hoops == 15 && (!GameOvered && !GamePaused && !GameWon && GameOverUI != null))
//                {
//                    gameObject.GetComponent<LineRenderer>().enabled = false;
                    
//                    StartCoroutine(StartFade(glide, 0.5f, 0f));;
//                    controller.Camera_Move = false;
//                    sling = true;
//                    startMovement = false;
//                    GameWon = true;
//                    cam = true;
//                    slingTemp = transform.position;
//                    slingTemp.x -= 8f;

//                }
//            }
//            count = 0;
//        }
//    }





//}
