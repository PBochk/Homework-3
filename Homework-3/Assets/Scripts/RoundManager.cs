using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour
{
    public RoundResult RoundResult { get; private set; }
    private DiceSpawner diceSpawner;
    private ScoreManager scoreManager;
    private int diceCount;
    private int rolledDiceCount;
    public UnityEvent RoundStart;
    public UnityEvent RoundEnd;
    public UnityEvent ScoreChange;

    private void Awake()
    {
        diceSpawner = GetComponentInChildren<DiceSpawner>();
        scoreManager = GetComponent<ScoreManager>();
    }

    public void OnRoll()
    {
        diceCount = diceSpawner.DiceCount;
        rolledDiceCount = 0;
        StartCoroutine(WaitForRoll());
    }

    private IEnumerator WaitForRoll()
    {
        yield return new WaitWhile(() => rolledDiceCount < diceCount);
        RoundResult = scoreManager.OnRoundEnd();
        RoundEnd.Invoke();
    }

    public void DiceRolled()
    {
        rolledDiceCount++;
    }
}
