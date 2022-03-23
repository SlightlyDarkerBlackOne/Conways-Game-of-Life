using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : GenericSingletonClass<GameManager>
{
    [SerializeField]
    private int availableActions;
    private int startAvailableActions = 4;

    public event Action<int> OnAvailableActionsChanged;
    private void Start() {
        SetAvailableActionsToDefault();
        OnAvailableActionsChanged?.Invoke(availableActions);
    }

    public bool CanPlayCard(Card card) {
        return (CardExpense.ReturnCardExpense(card) <= availableActions);
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
}
