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

    // Start is called before the first frame update
    void Start()
    {
        InputManager.TogglePause += TogglePausedText;
        patternPanel.SetBool("Show", false);
        CardManager.OnDeckChanged += UpdateDeckSizeText;
        GameManager.Instance.OnAvailableActionsChanged += UpdateAvailableActionsText;
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

    void TogglePausedText() {
        bool paused = pausedTextAnim.GetBool("Paused");
        pausedTextAnim.SetBool("Paused", !paused);
    }

    public void TogglePatternPanel() {
        patternPanel.SetBool("Show", !patternPanel.GetBool("Show"));
    }
}
