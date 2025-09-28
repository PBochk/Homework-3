using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    private int totalScore;
    public void IncreaseTotalScore(int increment)
    {
        totalScore += increment;
        scoreText.text = totalScore.ToString();
    }
}
