using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool IsAlive { get; private set; }
    public int numNeighbours = 0;
    
    public void SetAlive(bool alive) {
        IsAlive = alive;

        if (alive) {
            GetComponent<SpriteRenderer>().enabled = true;
        } else {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
