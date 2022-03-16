using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InputManager : MonoBehaviour
{
    public static event Action TogglePause;

    Game game;
    [SerializeField]
    Pattern pattern;

    [SerializeField]
    KeyCode pauseSimulation;

    void Awake()
    {
        game = FindObjectOfType<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        UserInput();
    }

    void UserInput() {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(mousePoint.x);
            int y = Mathf.RoundToInt(mousePoint.y);

            game.SetCellAliveOnCoordinates(x,y);
        }
        if (Input.GetMouseButtonDown(1)) {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(mousePoint.x);
            int y = Mathf.RoundToInt(mousePoint.y);
            
            game.SetPatternAliveOnCoordinates(x, y, pattern.patternArray, pattern.patternArray.GridSize.y, pattern.patternArray.GridSize.x);
        }
        if (Input.GetKeyDown(pauseSimulation)) {
            InvokePause();
        }
    }
    public void InvokePause() {
        TogglePause?.Invoke();
    }
}
