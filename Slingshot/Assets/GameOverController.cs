using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    //GameOver Condition.Public So that it could be accesible By plane.
   // [SerializeField] public bool IsGameOver = false;

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Demo_Scene");
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
