//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class PauseMenuController : MonoBehaviour
//{
//    //Variable to Check if Game is Paused.
//    public bool GamePaused = false;

//    //Paused Panel UI to Set Active UI.
//    [SerializeField] private GameObject PauseUI;

//    //Plane For Game Over Condition Check.
//    public GameObject Plane;

//    //To Get Plane Movement Script from Plane.
//    private PlaneMovement _planeMovement;

//    private void Start()
//    {
//        _planeMovement =Plane.GetComponent<PlaneMovement>();
//    }
//    // Update is called once per frame
//    void Update()
//    {
//        //If it is Escape and Game is Not Over. Pause The game.
//        if (Input.GetKeyDown(KeyCode.Escape) && !_planeMovement.GameOvered)
//        {
//            if (GamePaused)
//            {
//                Resume();
//            }
//            else
//            {
//                Pause();
//            }
//        }
//    }
//    public void Resume()
//    {
//        Plane.GetComponent<LineRenderer>().enabled = true;
//        PauseUI.SetActive(false);
//        Time.timeScale = 1f;
//        GamePaused = false;
//    }

//    public void Pause()
//    {
//        Plane.GetComponent<LineRenderer>().enabled = false;
//        Time.timeScale = 0f;
//        PauseUI.SetActive(true);
//        GamePaused = true;
//    }

//    //To Load Game Start Menu
//    public void LoadMenu()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene(0);
//    }

//    public void QuitGame()
//    {
//        Input.backButtonLeavesApp = true;
//        //Debug.Log("Quit Game Called");
//        Application.Quit();
//    }
//}
