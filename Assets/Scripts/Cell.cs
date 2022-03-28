using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool IsAlive { get; private set; }
    public int numNeighbours = 0;
    public int ownNeighbours = 0;
    public int enemyNeighbours = 0;

    public bool isHovered;
    public bool isPlayerCell = true;

    [SerializeField]
    private SpriteRenderer sR;
    
    public void SetAlive(bool alive) {
        IsAlive = alive;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (alive) {
            GetComponent<SpriteRenderer>().enabled = true;
            if (isPlayerCell) {
                spriteRenderer.color = GameManager.Instance.player1Color;
            } else if(!isPlayerCell){
                spriteRenderer.color = GameManager.Instance.player2Color;
            }
        } else {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void SetHovered() {
        if (isHovered) {
            sR.enabled = true;
        } else {
            sR.enabled = false;
        }
    }

    private void Update() {
        SetHovered();
    }

    private void OnMouseEnter() {
        Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int x = Mathf.RoundToInt(mousePoint.x);
        int y = Mathf.RoundToInt(mousePoint.y);
        Game.Instance.SetPatternHoverOnCoordinates(x, y);
    }
}
