using System;
using System.Collections;
using UnityEngine;

public class DiceActivity : MonoBehaviour
{
    [SerializeField] private Side[] sides;
    [SerializeField] private ScoreManager scoreChecker;
    public int Score { get; private set; }
    private Rigidbody rb;
    private const float velocityThreshold = 0.00001f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();   
    }

    public void OnRoll()
    {
        StartCoroutine(WaitForRollEnd(rb));
    }

    private IEnumerator WaitForRollEnd(Rigidbody rb)
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitUntil(() => rb.linearVelocity.magnitude < velocityThreshold && rb.angularVelocity.magnitude < velocityThreshold);
        CheckSides();
        scoreChecker.IncreaseTotalScore(Score);
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
