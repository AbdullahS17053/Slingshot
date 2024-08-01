using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    public TMP_Text Score; 
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

    public void SubtractScore(int points) {

        if ((CurrScore - points) >= 0)
        {
            CurrScore -= points;
            UpdateUI(CurrScore);
        }
    }

    public void ResetScore() { 
    
        CurrScore=0;
        UpdateUI(CurrScore);
    }
    private void UpdateUI(int sc)
    {
        Score.SetText(sc.ToString());
    }
}
