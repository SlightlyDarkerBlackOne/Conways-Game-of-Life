using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Card : MonoBehaviour, IPointerDownHandler
{
    public bool hasBeenPlayed;

    public int handIndex;

    [SerializeField]
    private float moveDistance = 40f;

    public enum Type
    {
        None,
        StillLife,
        Oscillator,
        SpaceShip
    }

    public Type cardType;

    [SerializeField]
    private Pattern pattern;

    private CardManager cardManager;
    void Start()
    {
        cardManager = FindObjectOfType<CardManager>();
    }

    private void OnEnable() {
        SetCardUI();
    }
    public void OnPointerDown(PointerEventData eventData) {
        if (!hasBeenPlayed) {
            PlayCard();
        }
    }

    private void PlayCard() {
        if (CanPlayCard()) {
            transform.position += Vector3.up * moveDistance;
            hasBeenPlayed = true;
            SetAvailableCardSlot();
            GameManager.Instance.ReduceAvailableActions(CardExpense.ReturnCardExpense(this));
            Invoke("MoveToDiscardPile", 2f); 
        }
    }

    private bool CanPlayCard() {
        return GameManager.Instance.CanPlayCard(this);
    }

    void MoveToDiscardPile() {
        cardManager.AddToDiscardPile(this);
        gameObject.SetActive(false);
    }
    void SetAvailableCardSlot() {
        cardManager.SetAvailableCardSlot(true, handIndex);
    }

    void SetCardUI() {
        transform.GetChild(0).GetChild(0).Find("Image").GetComponent<Image>().sprite = pattern.patternSprite;
        transform.GetChild(0).GetChild(0).Find("DescriptionText").GetComponent<TMPro.TextMeshProUGUI>().text = pattern.description;
        transform.GetChild(0).GetChild(0).Find("TitleText").GetComponent<TMPro.TextMeshProUGUI>().text = pattern.name;
    }
}
