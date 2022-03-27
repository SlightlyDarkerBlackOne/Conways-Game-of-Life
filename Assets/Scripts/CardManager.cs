using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardManager : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    private List<Card> discardPile = new List<Card>();
    public Transform[] cardSlots;
    public bool[] availableCardSlots;

    [SerializeField]
    private int numberOfStartingCards = 3;
    [SerializeField]
    private float drawDelay = 0.1f;
    [SerializeField]
    private float startDrawDelay = 1f;

    public static event Action<int> OnDeckChanged;

    private void Start() {
        OnDeckChanged?.Invoke(deck.Count);

        StartCoroutine(DrawStartingCards(numberOfStartingCards));
    }

    private IEnumerator DrawStartingCards(int count) {
        yield return new WaitForSeconds(startDrawDelay);
        while (count > 0) {
            yield return new WaitForSeconds(drawDelay);
            DrawCard();
            count--;
        }
    }
    public void TryDrawCard() {
        if (GameManager.Instance.CanDrawCard()) {
            DrawCard();
            GameManager.Instance.DisableCardDraw();
        }
    }

    private void DrawCard() {
        if(deck.Count >= 1) { 
            Card randCard = deck[UnityEngine.Random.Range(0, deck.Count)];

            for (int i = 0; i < availableCardSlots.Length; i++) {
                if (availableCardSlots[i]) {
                    randCard.gameObject.SetActive(true);
                    randCard.handIndex = i;
                    randCard.transform.position = cardSlots[i].position;
                    randCard.hasBeenPlayed = false;
                    availableCardSlots[i] = false;
                    deck.Remove(randCard);
                    OnDeckChanged?.Invoke(deck.Count);
                    return;
                }
            }  
        }
    }


    public void Shuffle() {
        if(discardPile.Count >= 1) {
            foreach (Card card in discardPile) {
                deck.Add(card);
            }
            discardPile.Clear();
        }
    }
    
    public void AddToDiscardPile(Card card) {
        discardPile.Add(card);
    }
    public void SetAvailableCardSlot(bool result,int index) {
        availableCardSlots[index] = result;
    }
}
