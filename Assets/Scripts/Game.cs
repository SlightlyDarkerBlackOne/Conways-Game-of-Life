using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : GenericSingletonClass<Game>
{
    [SerializeField] int gridWidth = 128;
    [SerializeField] int gridHeight = 64;

    public int GridWidth { get; private set; }
    public int GridHeight { get; private set; }

    [SerializeField]
    private int alivePercentage = 75;
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
                int ownNeighbours = 0;
                int enemyNeightbours = 0;

                //Up
                if(y + 1 < gridHeight) {
                    if (grid[x, y + 1].IsAlive) {
                        numNeighbours++;
                        CountOwnAndEnemyNeighbours(y, x, x, y+1, ref enemyNeightbours, ref ownNeighbours);
                    }
                }
                //Right
                if (x + 1 < gridWidth) {
                    if (grid[x+1, y].IsAlive) {
                        numNeighbours++;
                        CountOwnAndEnemyNeighbours(y, x, x + 1, y, ref enemyNeightbours, ref ownNeighbours);
                    }
                }
                //Down
                if (y - 1 >= 0) {
                    if (grid[x, y - 1].IsAlive) {
                        numNeighbours++; 
                        CountOwnAndEnemyNeighbours(y, x, x, y - 1, ref enemyNeightbours, ref ownNeighbours);
                    }
                }
                //Left
                if (x - 1 >= 0) {
                    if (grid[x-1, y].IsAlive) {
                        numNeighbours++;
                        CountOwnAndEnemyNeighbours(y, x, x - 1, y, ref enemyNeightbours, ref ownNeighbours);
                    }
                }
                //UpRight
                if (x + 1 < gridWidth && y + 1 < gridHeight) {
                    if (grid[x + 1, y + 1].IsAlive) {
                        numNeighbours++;
                        CountOwnAndEnemyNeighbours(y, x, x + 1, y + 1, ref enemyNeightbours, ref ownNeighbours);
                    }
                }
                //UpLeft
                if (x - 1 >= 0 && y + 1 < gridHeight) {
                    if (grid[x - 1, y + 1].IsAlive) {
                        numNeighbours++;
                        CountOwnAndEnemyNeighbours(y, x, x - 1, y + 1, ref enemyNeightbours, ref ownNeighbours);
                    }
                }
                //DownLeft
                if (x + 1 < gridWidth && y - 1 >= 0) {
                    if (grid[x + 1, y - 1].IsAlive) {
                        numNeighbours++;
                        CountOwnAndEnemyNeighbours(y, x, x + 1, y - 1, ref enemyNeightbours, ref ownNeighbours);
                    }
                }
                //DownRight
                if (x - 1 >= 0 && y - 1 >= 0) {
                    if (grid[x - 1, y - 1].IsAlive) {
                        numNeighbours++;
                        CountOwnAndEnemyNeighbours(y, x, x - 1, y - 1, ref enemyNeightbours, ref ownNeighbours);
                    }
                }

                grid[x, y].numNeighbours = numNeighbours;
                grid[x, y].ownNeighbours = ownNeighbours;
                grid[x, y].enemyNeighbours = enemyNeightbours;
            }
        }
    }

    private void CountOwnAndEnemyNeighbours(int y, int x, int x1, int y1, ref int enemyNeighbours, ref int ownNeighbours) {
        if ((grid[x1, y1].isPlayerCell && grid[x, y].isPlayerCell) || (!grid[x1, y1].isPlayerCell && !grid[x, y].isPlayerCell)) {
            ownNeighbours++;
        } else {
            enemyNeighbours++;
        }
    }

    private void PopulationControl() {
        for (int y = 0; y < gridHeight; y++) {
            for (int x = 0; x < gridWidth; x++) {
                if (grid[x, y].IsAlive) {
                    if ((grid[x,y].numNeighbours != 2 && grid[x, y].numNeighbours != 3) || grid[x,y].enemyNeighbours > grid[x, y].ownNeighbours) {
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
    //TODO Object pooling
    private void PlaceCells() {
        for (int y = 0; y < gridHeight; y++) {
            for (int x = 0; x < gridWidth; x++) {
                Cell cell = Instantiate(Resources.Load("Prefabs/Cell", typeof(Cell)), new Vector2(x, y), Quaternion.identity, this.transform) as Cell;
                grid[x, y] = cell;
                SetRandomCell(y, x);
            }
        }
    }

    private void SetRandomCell(int y, int x) {
        bool alive = RandomAliveCell();
        if (alive && GameManager.Instance.isLocalMultiplayer) {
            grid[x, y].isPlayerCell = UnityEngine.Random.Range(0, 100) <= 50 ? true : false;
        }
        grid[x, y].SetAlive(alive);
    }

    private bool RandomAliveCell() {
        int rand = UnityEngine.Random.Range(0, 100);
        if(rand >= (100-alivePercentage)) {
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
  
    public void SetCellAliveOnCoordinates(int x, int y, bool isPlayerCell = true) {
        if (AreCoordinatesInGrid(x, y)) {
            ToggleCellAliveOnClick(x, y, isPlayerCell);
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
                    SetCellAliveOnCoordinates(x + i, y + j, GameManager.Instance.isPlayerTurn);
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
                        return false;
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
    void ToggleCellAliveOnClick(int x, int y, bool isPlayerCell) {
        grid[x, y].isPlayerCell = isPlayerCell;
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
        for (int y = 0; y < gridHeight; y++) {
            for (int x = 0; x < gridWidth; x++) {
                SetRandomCell(y, x);
            }
        }
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
