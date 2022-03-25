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

    [SerializeField]
    private Pattern pattern;
    public Pattern Pattern { get { return pattern; } private set { pattern = value; } }

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
            SetActivePattern();
            SetAvailableCardSlot();
            GameManager.Instance.ReduceAvailableActions(CardExpense.GetCardExpense(this));
            GameManager.Instance.DisableEndTurn();
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
    void SetActivePattern() {
        FindObjectOfType<Game>().SetActivePattern(Pattern);
    }

    void SetCardUI() {
        transform.GetChild(0).GetChild(0).Find("Image").GetComponent<Image>().sprite = Pattern.patternSprite;
        transform.GetChild(0).GetChild(0).Find("DescriptionText").GetComponent<TMPro.TextMeshProUGUI>().text = Pattern.description;
        transform.GetChild(0).GetChild(0).Find("TitleText").GetComponent<TMPro.TextMeshProUGUI>().text = Pattern.name;
        transform.GetChild(0).GetChild(0).Find("ActionsText").GetComponent<TMPro.TextMeshProUGUI>().text = CardExpense.GetCardExpense(this).ToString();
    }
}
