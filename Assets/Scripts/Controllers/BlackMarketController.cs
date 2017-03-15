using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlackMarketController : MonoBehaviour
{
    public static List<CardData> BlackMarketCards;
    public static List<CardData> DisplayedCards;

    public bool Showing = false;

    public RectTransform CardsHolder;

    public List<Slot> CardSlots;
    public List<GameObject> Cards;

    // Use this for initialization
    void Start()
    {
        //Begin();
	}
	
    public void Begin()
    {
        if (DisplayedCards == null && PlayController.Instance.PlayingMission.BlackMarket != null)
        {
            Deck BlackMarketDeck = PlayController.Instance.PlayingMission.BlackMarket;
            int BlackMarketCardsQuantity = PlayController.Instance.BlackMarketCards;

            BlackMarketCards = new List<CardData>(BlackMarketDeck.CardsInDeck);
            BlackMarketCards.Shuffle();

            DisplayedCards = new List<CardData>();
            for (int i = 0; i < BlackMarketCardsQuantity && BlackMarketCards.Count > 0; i++)
            {
                DrawNewCard();
            }
        }
    }

    public Slot GetFreeSlot()
    {
        if (CardSlots == null || CardSlots.Count == 0)
            return null;

        foreach(Slot s in CardSlots)
        {
            if (s.ObjectOnTop == null)
                return s;
        }

        return null;
    }

    public void DrawNewCard()
    {
        if (BlackMarketCards == null || BlackMarketCards.Count < 1) return;

        int Index = Random.Range(0, BlackMarketCards.Count);
        CardData Card = BlackMarketCards[Index];
        DisplayedCards.Add(Card);
        BlackMarketCards.RemoveAt(Index);

        GameObject CardObject = UIController.Instance.CreateCard(Card, false);
        CardObject.AddComponent<BlackMarketCardButton>().Cost = Card.MoneyCost;
        CardObject.GetComponent<BlackMarketCardButton>().Market = this;
        if (Cards == null)
            Cards = new List<GameObject>();
        Cards.Add(CardObject);
        CardObject.transform.SetParent(CardsHolder);
        CardObject.GetComponent<CardBehaviour>().SetSlot(GetFreeSlot());
        CardObject.GetComponent<CardBehaviour>().Show = true;

        CardObject.GetComponent<DraggableElement>().enabled = false;
        CardObject.GetComponent<CardBehaviour>().enabled = false;
    }

    public void Show()
    {
        Showing = true;
        gameObject.SetActive(true);
        StartCoroutine(ShowRoutine());
    }

    public IEnumerator ShowRoutine()
    {
        yield return null;
        yield return null;
        if (DisplayedCards == null)
            Begin();
    }

    public void Hide()
    {
        Showing = false;
        gameObject.SetActive(false);
    }
}
