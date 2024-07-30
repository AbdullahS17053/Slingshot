using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameWonController : MonoBehaviour, IPointerDownHandler
{
    private int LVL_Loader = 0;
    //public MenuManager _MenuManager;
    public void OnPointerDown(PointerEventData eventData)
    {
       // Debug.Log(eventData.currentInputModule);
    }

    public void MenuClick()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void RestartClick()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }
    
    public void NextLVL()
    {
        LVL_Loader=PlayerPrefs.GetInt("level", 1);
        LVL_Loader = (LVL_Loader + 1) % 3;

        Time.timeScale = 1.0f;
        //_MenuManager.LoadLevel(LVL_Loader);
        PlayerPrefs.SetInt("level", LVL_Loader);
        SceneManager.LoadScene(1);
        // Debug.Log("Next LVL Clicked");


    }
}
