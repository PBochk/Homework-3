using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DiceActivity : MonoBehaviour
{
    [SerializeField] private Side[] sides;
    public int Score { get; private set; }
    private ScoreManager scoreManager;
    private RoundManager roundManager;
    private Rigidbody rb;
    private const float velocityThreshold = 0.00001f;
    public UnityEvent DiceRolled;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        scoreManager = FindFirstObjectByType<ScoreManager>();
        roundManager = FindFirstObjectByType<RoundManager>();
        DiceRolled.AddListener(roundManager.DiceRolled);
    }

    public void OnRoll()
    {
        StartCoroutine(WaitForRollEnd());
    }

    private IEnumerator WaitForRollEnd()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitUntil(() => rb.linearVelocity.magnitude < velocityThreshold && rb.angularVelocity.magnitude < velocityThreshold);
        CheckSides();
        scoreManager.IncreaseTotalScore(Score);
        DiceRolled.Invoke();
    }

    private void CheckSides()
    {
        foreach (var side in sides)
        {
            if (side.IsBottom)
            {
                Score = 7 - side.GetNumber();
                return;
            }
        }
        Score = 0;
    }
}
