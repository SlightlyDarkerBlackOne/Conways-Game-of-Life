using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Animator pausedTextAnim;
    // Start is called before the first frame update
    void Start()
    {
        InputManager.TogglePause += TogglePausedText;
    }

    void TogglePausedText() {
        bool paused = pausedTextAnim.GetBool("Paused");
        pausedTextAnim.SetBool("Paused", !paused);
    }
}
