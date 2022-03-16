using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Game game;

    private void Start() {
        game = FindObjectOfType<Game>();
        SetCameraPosition();
    }

    void SetCameraPosition() {
        Camera.main.transform.position = new Vector3(game.GridWidth / 2, game.GridHeight / 2, -10f);
    }
}
