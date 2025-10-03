using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// TODO: Split on model and controller 
public class ScoreManager : MonoBehaviour
{
    private RoundManager roundManager;
    private DiceSpawner diceSpawner;
    public UnityEvent ScoreIncreased;
    public UnityEvent ScoreCorrectChange;
    public UnityEvent ScoreIncorrectChange;
    private bool isScoreChanged = false;

    // Fields beyond should be in separate model class really
    private int diceCount;
    private int minScore;
    private int maxScore;
    private int totalScore;
    private int loseScore;
    private int drawScore;
    private int winScore;

    public int LoseScore
    {
        get => loseScore;
        set
        {
            if (minScore <= value && value <= maxScore - 2)
            {
                loseScore = value;
                drawScore = value + 1;
                winScore = value + 2;
                StartCoroutine(OnScoreChange());
            }
            else
                ScoreIncorrectChange.Invoke();
        }
    }

    public int DrawScore
    {
        get => drawScore;
        set
        {
            if (minScore - 1 <= value && value <= maxScore - 1)
            {
                loseScore = value - 1;
                drawScore = value;
                winScore = value + 1;
                StartCoroutine(OnScoreChange());
            }
            else
                ScoreIncorrectChange.Invoke();
        }
    }
    public int WinScore
    {
        get => winScore;
        set
        {
            if (minScore - 2 <= value && value <= maxScore)
            {
                loseScore = value + 2;
                drawScore = value + 1;
                winScore = value;
                StartCoroutine(OnScoreChange());
            }
            else
                ScoreIncorrectChange.Invoke();
        }
    }

    public int Score => totalScore;

    private void Awake()
    {
        roundManager = GetComponent<RoundManager>();
        diceSpawner = GetComponentInChildren<DiceSpawner>();
        diceSpawner.DiceCountChanged.AddListener(OnDiceCountChange);
        OnDiceCountChange(); //Dice count is invoked too late sometimes, so this called manually
        roundManager.RoundStart.AddListener(ReloadScore);
        isScoreChanged = false;
    }

    private void OnDiceCountChange()
    {
        diceCount = diceSpawner.DiceCount;
        minScore = diceCount;
        maxScore = diceCount * 6;
        isScoreChanged = false;
        SetScores();
    }

    // If invoked too early UI won't get it 
    private IEnumerator OnScoreChange()
    {
        yield return new WaitForFixedUpdate();
        isScoreChanged = true;
        ScoreCorrectChange.Invoke();
    }

    public void OnRoll()
    {
        totalScore = 0;
        isScoreChanged = false;
    }

    private void SetScores()
    {
        if (!isScoreChanged)
            DrawScore = Random.Range(minScore + 1, maxScore);
    }

    private void ReloadScore()
    {
        totalScore = 0;
        SetScores();
    }

    public void IncreaseTotalScore(int increment)
    {
        totalScore += increment;
        ScoreIncreased.Invoke();
    }

    public RoundResult OnRoundEnd()
    {
        if (totalScore == drawScore)
        {
            return RoundResult.Draw;
        }
        else if (totalScore >= winScore)
        {
            return RoundResult.Win;
        }
        else
        {
            return RoundResult.Lose;
        }
    }
}
