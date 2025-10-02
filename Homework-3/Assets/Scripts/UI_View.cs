using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

// TODO: Split on pure UI view and input related classes
public class UI_View : MonoBehaviour
{
    [SerializeField] private TMP_Text errorText;

    [Header("Parent Canvas")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform canvasRT;
    [SerializeField] private Camera cam;
    private float oldAspect;
    [Header("RoundEnd")]
    [SerializeField] private CanvasGroup roundEnd;
    [SerializeField] private RectTransform roundEndRT;
    [SerializeField] private TMP_Text roundResultText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Button reloadButton;

    // All input stuff should be in separate class
    [Header("InputFields")]
    [SerializeField] private CanvasGroup inputFields;
    [SerializeField] private RectTransform inputFieldsRT;
    [SerializeField] private TMP_InputField diceCountField;
    [SerializeField] private TMP_InputField loseField;
    [SerializeField] private TMP_InputField drawField;
    [SerializeField] private TMP_InputField winField;

    [Header("RoundResults")]
    [SerializeField] private string loseResult;
    [SerializeField] private string drawResult;
    [SerializeField] private string winResult;

    [Header("Scripts")]
    [SerializeField] private DiceSpawner diceSpawner;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private RoundManager roundEndManager;


    private Dictionary<RoundResult, string> results;

    private void Awake()
    {
        results = new()
        {
            { RoundResult.Lose, loseResult},
            { RoundResult.Draw, drawResult},
            { RoundResult.Win, winResult}
        };

        oldAspect = cam.aspect;
        inputFields.interactable = true;

        ResizeChildrenOfCanvas();
        SetCharacterValidation();
        AddExternalListeners();
        AddInternalListeners();
    }

    private void ResizeChildrenOfCanvas()
    {
        inputFieldsRT.sizeDelta = canvasRT.sizeDelta;
        roundEndRT.sizeDelta = canvasRT.sizeDelta;
        StartCoroutine(WaitForResize()); // Remove if resize in realtime is not needed
    }

    private IEnumerator WaitForResize()
    {
        // For SOME REASON RectTransform.hasChanged doesn't detect when parent canvas change because of camera aspect
        // so predicate checks both
        // Does it affect performance badly? idk but in theory it's neat
        yield return new WaitWhile(() => !canvasRT.hasChanged && oldAspect == cam.aspect); 
        canvasRT.hasChanged = false;
        oldAspect = cam.aspect;
        ResizeChildrenOfCanvas();
    }

    private void AddExternalListeners()
    {
        roundEndManager.RoundEnd.AddListener(OnRoundEnd);
        scoreManager.ScoreCorrectChange.AddListener(OnCorrectScoreChange);
        scoreManager.ScoreIncorrectChange.AddListener(OnIncorrectScoreChanged);
        diceSpawner.DiceCountChanged.AddListener(OnDiceCountChange);
    }

    private void AddInternalListeners()
    {
        diceCountField.onEndEdit.AddListener(OnDiceCountFieldChange);
        reloadButton.onClick.AddListener(OnReloadButtonClick);
        loseField.onEndEdit.AddListener(OnLoseScoreFieldChange);
        drawField.onEndEdit.AddListener(OnDrawScoreFieldChange);
        winField.onEndEdit.AddListener(OnWinScoreFieldChange);
    }

    // Ensures that all input is integer
    private void SetCharacterValidation()
    {
        diceCountField.characterValidation = TMP_InputField.CharacterValidation.Integer;
        loseField.characterValidation = TMP_InputField.CharacterValidation.Integer;
        drawField.characterValidation = TMP_InputField.CharacterValidation.Integer;
        winField.characterValidation = TMP_InputField.CharacterValidation.Integer;
    }

    private void OnRoundEnd()
    {
        inputFields.interactable = true;
        roundEnd.gameObject.SetActive(true);
        roundResultText.text = results[roundEndManager.RoundResult];
        scoreText.text = scoreManager.Score.ToString();
    }

    private void OnReloadButtonClick()
    {
        inputFields.interactable = false;
        roundEnd.gameObject.SetActive(false);
        roundEndManager.RoundStart.Invoke();
    }

    private void UpdateScoreFields()
    {
        loseField.text = scoreManager.LoseScore.ToString();
        drawField.text = scoreManager.DrawScore.ToString();
        winField.text = scoreManager.WinScore.ToString();
    }

    // Input field stuff REALLY should be in separate class
    private void OnDiceCountFieldChange(string newDiceCount) => diceSpawner.DiceCount = int.Parse(newDiceCount);
    
    private void OnLoseScoreFieldChange(string newScore) => scoreManager.LoseScore = int.Parse(newScore);
    
    private void OnDrawScoreFieldChange(string newScore) => scoreManager.DrawScore = int.Parse(newScore);

    private void OnWinScoreFieldChange(string newScore) => scoreManager.WinScore = int.Parse(newScore);

    private void OnCorrectScoreChange()
    {
        errorText.gameObject.SetActive(false);
        UpdateScoreFields();
    }

    // By "incorrect" i mean one that doesn't belong to (min allowed score; max allowed score) range
    private void OnIncorrectScoreChanged()
    {
        errorText.gameObject.SetActive(true);
    }
    private void OnDiceCountChange() => diceCountField.text = diceSpawner.DiceCount.ToString();

}
