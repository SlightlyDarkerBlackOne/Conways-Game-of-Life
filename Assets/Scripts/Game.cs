using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] int gridWidth = 128;
    [SerializeField] int gridHeight = 64;

    public int GridWidth { get; private set; }
    public int GridHeight { get; private set; }

    [SerializeField]
    private float speed = 0.1f;
    private float timer = 0;
    private float simulationTimer = 0;
    [SerializeField]
    private int simulationSteps = 100;
    [SerializeField]
    private bool isUsingSimulationSteps;

    public bool simulationEnabled = false;

    [SerializeField]
    private Pattern activePattern;
    private Pattern emptyPattern;

    Cell[,] grid;

    private void Awake() {
        SetGridSize();
    }
    private void Start() {
        //SetGridSize();
        PlaceCells();

        InputManager.TogglePause += TogglePauseSimulation;
        emptyPattern = activePattern;
    }
    private void OnDestroy() {
        InputManager.TogglePause -= TogglePauseSimulation;
    }

    private void Update() {
        HandleSimulation();
    }
    void HandleSimulation() {
        if (simulationEnabled) {
            if (timer >= speed) {
                timer = 0f;
                CountNeighbours();
                PopulationControl();
            } else {
                timer += Time.deltaTime;
            }
        } 
        if (isUsingSimulationSteps && simulationTimer >= speed * simulationSteps) {
            Debug.Log("simulation ended");
            simulationTimer = 0;
            simulationEnabled = false;
        } else if(isUsingSimulationSteps && simulationEnabled)
            simulationTimer += Time.deltaTime;
    }

    private void CountNeighbours() {
        for (int y = 0; y < gridHeight; y++) {
            for (int x = 0; x < gridWidth; x++) {
                int numNeighbours = 0;

                //Up
                if(y + 1 < gridHeight) {
                    if (grid[x, y + 1].IsAlive) {
                        numNeighbours++;
                    }
                }
                //Right
                if (x + 1 < gridWidth) {
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
                if (x + 1 < gridWidth && y + 1 < gridHeight) {
                    if (grid[x + 1, y + 1].IsAlive) {
                        numNeighbours++;
                    }
                }
                //UpLeft
                if (x - 1 >= 0 && y + 1 < gridHeight) {
                    if (grid[x - 1, y + 1].IsAlive) {
                        numNeighbours++;
                    }
                }
                //DownLeft
                if (x + 1 < gridWidth && y - 1 >= 0) {
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
        for (int y = 0; y < gridHeight; y++) {
            for (int x = 0; x < gridWidth; x++) {
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
        for (int y = 0; y < gridHeight; y++) {
            for (int x = 0; x < gridWidth; x++) {
                Cell cell = Instantiate(Resources.Load("Prefabs/Cell", typeof(Cell)), new Vector2(x, y), Quaternion.identity, this.transform) as Cell;
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
        GridWidth = gridWidth;
        GridHeight = gridHeight;
        grid = new Cell[gridWidth, gridHeight];
    }

    void TogglePauseSimulation() {
        simulationEnabled = !simulationEnabled;
    }
  
    public void SetCellAliveOnCoordinates(int x, int y) {
        if (AreCoordinatesInGrid(x, y)) {
            ToggleCellAliveOnClick(x, y);
            GameManager.Instance.EnableEndTurn();
        }
    }
    public void SetPatternAliveOnCoordinates(int x, int y) {
        bool coordinatesInGrid = true;
        coordinatesInGrid = PatternCoordinatesInGrid(x, y, coordinatesInGrid);
        if (!coordinatesInGrid)
            return;
        for (int i = 0; i < activePattern.patternArray.GridSize.y; i++) {
            for (int j = 0; j < activePattern.patternArray.GridSize.x; j++) {
                if (activePattern.patternArray.GetCell(i,j)) {
                    SetCellAliveOnCoordinates(x + i, y + j);
                }
            }
        }
        activePattern = emptyPattern;
    }

    public void SetPatternAliveOnDragRelease() {
        Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int x = Mathf.RoundToInt(mousePoint.x);
        int y = Mathf.RoundToInt(mousePoint.y);
        SetPatternAliveOnCoordinates(x, y);
    }
    public void SetPatternHoverOnCoordinates(int x, int y) {
        CleanHoverGrid();
        bool coordinatesInGrid = true;
        coordinatesInGrid = PatternCoordinatesInGrid(x, y, coordinatesInGrid);
        if (!coordinatesInGrid)
            return;
        for (int i = 0; i < activePattern.patternArray.GridSize.y; i++) {
            for (int j = 0; j < activePattern.patternArray.GridSize.x; j++) {
                if (activePattern.patternArray.GetCell(i, j)) {
                    grid[x + i, y + j].isHovered = true;
                }
            }
        }
    }

    private bool PatternCoordinatesInGrid(int x, int y, bool coordinatesInGrid) {
        for (int i = 0; i < activePattern.patternArray.GridSize.y; i++) {
            for (int j = 0; j < activePattern.patternArray.GridSize.x; j++) {
                if (activePattern.patternArray.GetCell(i, j)) {
                    if (!AreCoordinatesInGrid(x + i, y + j)) {
                        coordinatesInGrid = false;
                        return coordinatesInGrid;
                    }
                }
            }
        }

        return coordinatesInGrid;
    }

    bool AreCoordinatesInGrid(int x, int y) {
        if (x >= 0 && y >= 0 && x < gridWidth && y < gridHeight) {
            return true;
        }
        return false;
    }
    void ToggleCellAliveOnClick(int x, int y) {
        grid[x, y].SetAlive(!grid[x, y].IsAlive);
    }

    public void CleanGrid() {
        for (int y = 0; y < gridHeight; y++) {
            for (int x = 0; x < gridWidth; x++) {
                grid[x, y].SetAlive(false);
            }
        }
    }

    private void CleanHoverGrid() {
        for (int y = 0; y < gridHeight; y++) {
            for (int x = 0; x < gridWidth; x++) {
                grid[x, y].isHovered = false;
            }
        }
    }

    public void RunSimulation() {
        CleanGrid();
        PlaceCells();
    }

    public void SetActivePattern(Pattern pattern) {
        activePattern = pattern;
    }

    public int GetNumberOfAliveCells() {
        int numberOfAliveCells = 0;
        for (int y = 0; y < gridHeight; y++) {
            for (int x = 0; x < gridWidth; x++) {
                if (grid[x, y].IsAlive) {
                    numberOfAliveCells++;       
                }
            }
        }
        return numberOfAliveCells;
    }

    public void UseSimulationWithSteps() {
        isUsingSimulationSteps = true;
    }
}
