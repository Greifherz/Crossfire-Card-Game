using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class BlackMarketCardButton : MonoBehaviour, IPointerClickHandler {

    public int Cost;
    public BlackMarketController Market;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(PlayController.Instance.CurrentPlayer.Money >= Cost)
        {
            PlayController.Instance.CurrentPlayer.Money -= Cost;
            GetComponent<CardBehaviour>().enabled = true;
            Market.Cards.Remove(gameObject);
            transform.SetParent(UIController.Instance.CardsHolder, true);
            UIController.Instance.DrawCard(gameObject);
            Market.DrawNewCard();
            Destroy(this);
        }
        else
        {
            UIController.Warn("Not enough money");
        }
    }
    
}
