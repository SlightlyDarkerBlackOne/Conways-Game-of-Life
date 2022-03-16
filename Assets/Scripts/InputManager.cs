using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InputManager : MonoBehaviour
{
    public static event Action TogglePause;

    Game game;

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
        if (Input.GetKeyDown(KeyCode.Space)) {
            TogglePause?.Invoke();
        }
    }
}
