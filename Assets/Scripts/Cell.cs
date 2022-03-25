using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool IsAlive { get; private set; }
    public int numNeighbours = 0;

    public bool isHovered;

    public void SetAlive(bool alive) {
        IsAlive = alive;

        if (alive) {
            GetComponent<SpriteRenderer>().enabled = true;
        } else {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void SetHovered() {
        if (isHovered) {
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        } else {
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void Update() {
        SetHovered();
    }

    private void OnMouseEnter() {
        Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int x = Mathf.RoundToInt(mousePoint.x);
        int y = Mathf.RoundToInt(mousePoint.y);
        FindObjectOfType<Game>().SetPatternHoverOnCoordinates(x, y);
    }
}
