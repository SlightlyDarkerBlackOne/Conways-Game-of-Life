using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private int gridWidth = 128;
    [SerializeField]
    private int gridHeight = 64;

    private static int SCREEN_WIDTH = 64;
    private static int SCREEN_HEIGHT = 48;

    [SerializeField]
    private float speed = 0.1f;
    private float timer = 0;

    Cell[,] grid = new Cell[SCREEN_WIDTH, SCREEN_HEIGHT];

    private void Start() {
        SetGridSize();
        PlaceCells();
    }

    private void Update() {
        if(timer >= speed) {
            timer = 0f;
            CountNeighbours();
            PopulationControl();
        } else {
            timer += Time.deltaTime;
        }
    }

    private void CountNeighbours() {
        for (int y = 0; y < SCREEN_HEIGHT; y++) {
            for (int x = 0; x < SCREEN_WIDTH; x++) {
                int numNeighbours = 0;

                //Up
                if(y + 1 < SCREEN_HEIGHT) {
                    if (grid[x, y + 1].IsAlive) {
                        numNeighbours++;
                    }
                }
                //Right
                if (x + 1 < SCREEN_WIDTH) {
                    if (grid[x+1, y].IsAlive) {
                        numNeighbours++;
                    }
                }
                //Down
                if (y - 1 >= 0) {
                    if (grid[x, y - 1].IsAlive) {
                        numNeighbours++;
                    }
                }
                //Left
                if (x - 1 >= 0) {
                    if (grid[x-1, y].IsAlive) {
                        numNeighbours++;
                    }
                }
                //UpRight
                if (x + 1 < SCREEN_WIDTH && y + 1 < SCREEN_HEIGHT) {
                    if (grid[x + 1, y + 1].IsAlive) {
                        numNeighbours++;
                    }
                }
                //UpLeft
                if (x - 1 >= 0 && y + 1 < SCREEN_HEIGHT) {
                    if (grid[x - 1, y + 1].IsAlive) {
                        numNeighbours++;
                    }
                }
                //DownLeft
                if (x + 1 < SCREEN_WIDTH && y - 1 >= 0) {
                    if (grid[x + 1, y - 1].IsAlive) {
                        numNeighbours++;
                    }
                }
                //DownRight
                if (x - 1 >= 0 && y - 1 >= 0) {
                    if (grid[x - 1, y - 1].IsAlive) {
                        numNeighbours++;
                    }
                }

                grid[x, y].numNeighbours = numNeighbours;
            }
        }
    }

    private void PopulationControl() {
        for (int y = 0; y < SCREEN_HEIGHT; y++) {
            for (int x = 0; x < SCREEN_WIDTH; x++) {
                if (grid[x, y].IsAlive) {
                    if (grid[x,y].numNeighbours != 2 && grid[x, y].numNeighbours != 3) {
                        grid[x, y].SetAlive(false);
                    }
                } else {
                    if (grid[x,y].numNeighbours == 3) {
                        grid[x, y].SetAlive(true);
                    }
                }
            }
        }
    }
    private void PlaceCells() {
        for (int y = 0; y < SCREEN_HEIGHT; y++) {
            for (int x = 0; x < SCREEN_WIDTH; x++) {
                Cell cell = Instantiate(Resources.Load("Prefabs/Cell", typeof(Cell)), new Vector2(x, y), Quaternion.identity) as Cell;
                grid[x, y] = cell;
                grid[x, y].SetAlive(RandomAliveCell());
            }
        }
    }

    bool RandomAliveCell() {
        int rand = UnityEngine.Random.Range(0, 100);
        if(rand >= 75) {
            return true;
        } else {
            return false;
        }
    }

    private void SetGridSize() {
        SCREEN_HEIGHT = gridHeight;
        SCREEN_WIDTH = gridWidth;
        grid = new Cell[SCREEN_WIDTH, SCREEN_HEIGHT];
    }
}
