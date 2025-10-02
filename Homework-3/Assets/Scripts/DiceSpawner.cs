using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiceSpawner : MonoBehaviour
{
    [Header("Stats")] // Are they should be in separate model? idk
    [SerializeField] private GameObject dicePrefab;
    [SerializeField] private float distance;
    [SerializeField] private int diceCount;
    
    [Header("Scripts")]
    private RoundManager roundManager;
    public UnityEvent DiceCountChanged;
    private List<GameObject> spawnedDice;
    public int DiceCount
    {
        get => diceCount;
        set
        {
            diceCount = value;
            DiceCountChanged.Invoke();
            Reload();
        }
    }

    private void Awake()
    {
        roundManager = GetComponentInParent<RoundManager>();
        roundManager.RoundStart.AddListener(Reload);
        spawnedDice = new();
        SpawnAllDice();
        // TODO: figure out how to do initial invoke properly
        DiceCountChanged.Invoke(); // If invoked too late, score manager won't get it
        StartCoroutine(InvokeForUI()); // If invoked too early, UI won't get it
    }

    private IEnumerator InvokeForUI()
    {
        yield return new WaitForFixedUpdate();
        DiceCountChanged.Invoke();
    }

    private void Reload()
    {
        foreach (var dice in spawnedDice)
        {
            Destroy(dice);
        }
        SpawnAllDice();
    }

    private void SpawnAllDice()
    {
        var angle = Mathf.PI * 2 / diceCount;
        for (int i = 0; i < diceCount; i++)
        {
            SpawnDice(transform.position, i * angle);
        }
    }

    private void SpawnDice(Vector3 centerPosition, float angle)
    {
        var dice = Instantiate(dicePrefab,
            new Vector3(distance * Mathf.Sin(angle),
            centerPosition.y,
            distance * Mathf.Cos(angle)),
            Quaternion.identity);
        spawnedDice.Add(dice);
    }
}