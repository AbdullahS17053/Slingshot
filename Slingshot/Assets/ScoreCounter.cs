using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public Text Score; 
    private int CurrScore;

    private void Start()
    {
        CurrScore = 0;
        UpdateUI(CurrScore);
    }

    public void AddScore(int points)
    {
        CurrScore += points;
        UpdateUI(CurrScore);
    }

    private void UpdateUI(int sc)
    {
        Score.text = sc.ToString();
    }
}
