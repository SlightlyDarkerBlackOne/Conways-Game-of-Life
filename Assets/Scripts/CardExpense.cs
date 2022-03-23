using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardExpense
{
    private static int stillLifeExpense = 1;
    private static int oscillatorExpense = 2;
    private static int spaceShipExpense = 3;
    public static int ReturnCardExpense(Card card) {
        switch (card.cardType) {
            case Card.Type.StillLife:
                return stillLifeExpense;
            case Card.Type.Oscillator:
                return oscillatorExpense;
            case Card.Type.SpaceShip:
                return spaceShipExpense;
            default:
                return -1;
        }
    }
}
