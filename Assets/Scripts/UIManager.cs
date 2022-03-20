using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Animator pausedTextAnim;
    [SerializeField]
    Animator patternPanel;
    // Start is called before the first frame update
    void Start()
    {
        InputManager.TogglePause += TogglePausedText;
        patternPanel.SetBool("Show", false);
    }

    void TogglePausedText() {
        bool paused = pausedTextAnim.GetBool("Paused");
        pausedTextAnim.SetBool("Paused", !paused);
    }

    public void TogglePatternPanel() {
        patternPanel.SetBool("Show", !patternPanel.GetBool("Show"));
    }
}
