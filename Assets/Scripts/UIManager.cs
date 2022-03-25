using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Animator pausedTextAnim;
    [SerializeField]
    Animator patternPanel;

    [SerializeField] private TMPro.TextMeshProUGUI deckSizeText;
    [SerializeField] private TMPro.TextMeshProUGUI availableActionsText;
    [SerializeField] private TMPro.TextMeshProUGUI finalScoreText;
    [SerializeField] private TMPro.TextMeshProUGUI remainingTurnsText;

    // Start is called before the first frame update
    void Start()
    {
        InputManager.TogglePause += TogglePausedText;
        patternPanel.SetBool("Show", false);
        CardManager.OnDeckChanged += UpdateDeckSizeText;
        GameManager.Instance.OnAvailableActionsChanged += UpdateAvailableActionsText;
        GameManager.Instance.GameOver += ShowGameOverScreen;
        GameManager.Instance.TurnEnded += UpdateTurnsText;
    }

    private void ShowGameOverScreen(int score) {
        transform.Find("GameOverPanel").gameObject.SetActive(true);
        finalScoreText.text = score.ToString();
    }

    private void OnDestroy() {
        InputManager.TogglePause -= TogglePausedText;
        CardManager.OnDeckChanged -= UpdateDeckSizeText;
    }
    private void UpdateAvailableActionsText(int numberOfAvailableActions) {
        availableActionsText.text = numberOfAvailableActions.ToString();
    }

    private void UpdateDeckSizeText(int count) {
        deckSizeText.text = count.ToString();

    }
    private void UpdateTurnsText(int count) {
        remainingTurnsText.text = count.ToString();
    }
    private void TogglePausedText() {
        bool paused = pausedTextAnim.GetBool("Paused");
        pausedTextAnim.SetBool("Paused", !paused);
    }

    public void TogglePatternPanel() {
        patternPanel.SetBool("Show", !patternPanel.GetBool("Show"));
    }
}
