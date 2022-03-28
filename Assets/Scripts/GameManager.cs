using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : GenericSingletonClass<GameManager>
{
    [SerializeField]
    private int availableActions;
    private int startAvailableActions = 4;

    [SerializeField]
    private int maximumTurns = 6;
    [SerializeField]
    private float gameOverDelayTime = 1.5f;

    private bool canEndTurn = true;
    private bool cardInPlay;
    private bool canDrawCard;

    public Color player1Color;
    public Color player2Color;

    public bool isLocalMultiplayer;
    public bool isPlayerTurn = true;

    public event Action<int> OnAvailableActionsChanged;
    public event Action<int> TurnEnded;
    public event Action<int> GameOver;

    private void Start() {
        SetAvailableActionsToDefault();
        OnAvailableActionsChanged?.Invoke(availableActions);

        TurnEnded?.Invoke(maximumTurns);
    }

    public bool CanPlayCard(Card card) {
        return ((CardExpense.GetCardExpense(card) <= availableActions) && !cardInPlay);
    }

    void SetAvailableActionsToDefault() {
        availableActions = startAvailableActions;
        OnAvailableActionsChanged?.Invoke(availableActions);
    }
    public void ReduceAvailableActions(int amount) {
        availableActions -= amount;
        if(availableActions < 0) {
            Debug.LogError("Available actions less than 0. Card Amount is wrong.");
        }
        OnAvailableActionsChanged?.Invoke(availableActions);
    }
    public bool CanDrawCard() {
        return canDrawCard;
    }
    public void DisableCardDraw() {
        canDrawCard = false;
    }
    public void EndTurn() {
        if (canEndTurn) {
            Debug.Log("EndTurn");
            maximumTurns--;
            if (maximumTurns <= 0) {
                Debug.Log("GameOver");
                StartCoroutine(ShowEndScreen());
                return;
            } 

            TurnEnded?.Invoke(maximumTurns);
            SetAvailableActionsToDefault();
            canDrawCard = true;
            GetComponent<InputManager>().InvokePause();
            if(isLocalMultiplayer)
                isPlayerTurn = !isPlayerTurn;
        }
    }
    public void DisableEndTurn() {
        canEndTurn = false;
        cardInPlay = true;
    }
    public void EnableEndTurn() {
        canEndTurn = true;
        cardInPlay = false;
    }
    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator ShowEndScreen() {
        Debug.Log("BeforeShow");
        yield return new WaitForSeconds(gameOverDelayTime);
        int aliveCells = GetComponent<Game>().GetNumberOfAliveCells();
        GameOver?.Invoke(aliveCells);
        Debug.Log("AfterShow");
    }
}
