using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Game game;

    private void Start() {
        game = Game.Instance;
        SetCameraPosition();
    }

    void SetCameraPosition() {
        Camera.main.transform.position = new Vector3(game.GridWidth / 2, game.GridHeight / 2, -10f);
        Camera.main.orthographicSize = game.GridHeight - (game.GridHeight * 0.2f);
    }
}
