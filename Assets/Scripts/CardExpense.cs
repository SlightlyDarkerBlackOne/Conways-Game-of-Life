using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardExpense
{
    private static int stillLifeExpense = 1;
    private static int oscillatorExpense = 2;
    private static int spaceShipExpense = 3;
    public static int GetCardExpense(Card card) {
        switch (card.Pattern.cardType) {
            case Pattern.Type.StillLife:
                return stillLifeExpense;
            case Pattern.Type.Oscillator:
                return oscillatorExpense;
            case Pattern.Type.SpaceShip:
                return spaceShipExpense;
            default:
                return -1;
        }
    }
}
