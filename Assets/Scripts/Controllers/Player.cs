using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Player : IStateListener
{
    private List<IHPListener> HPListeners;
    private List<IMoneyListener> MoneyListeners;
    
    public void RegisterListener(IHPListener Listener)
    {
        if (HPListeners == null)
            HPListeners = new List<IHPListener>();

        if (!HPListeners.Contains(Listener))
            HPListeners.Add(Listener);
    }

    public void RegisterListener(IMoneyListener Listener)
    {
        if (MoneyListeners == null)
            MoneyListeners = new List<IMoneyListener>();

        if (!MoneyListeners.Contains(Listener))
            MoneyListeners.Add(Listener);
    }

    public void DisposeListener(IHPListener Listener)
    {
        if(HPListeners != null && HPListeners.Count > 0)
            HPListeners.Remove(Listener);
    }

    public void DisposeListener(IMoneyListener Listener)
    {
        if (MoneyListeners != null && MoneyListeners.Count > 0)
            MoneyListeners.Remove(Listener);
    }

    private bool started;
    public bool Started
    {
        get { return started; }
        set
        {
            started = value;
            if (value)
            {
                PopulateDeck();
                Hand = new List<CardBehaviour>();
                Draw(StartingHand);
            }
        }
    }

    public CardColor Color;
    public int StartingHand;
    private int hP;
    public int HP
    {
        get { return hP; }
        set
        {
            if(value < 1)
            {
                PlayController.Instance.EndGame("You Died. In Pain.");
            }
            if (value != hP)
            {
                int Dif = hP - value;
                for (int i = 0; i < Dif; i++)
                {
                    float RandomX = UnityEngine.Random.Range(-((Screen.width / 2) -150), ((Screen.width / 2) - 150));
                    float RandomY = UnityEngine.Random.Range(-((Screen.height / 2) - 150), ((Screen.height / 2) - 150));
                    GameObject Splatter = GameObject.Instantiate(Resources.Load("Splatter"), new Vector2(RandomX, RandomY), Quaternion.identity) as GameObject;
                    Splatter.transform.SetParent(UIController.Instance.transform, false);
                }
                if (!SettingsManager.Cheats.GodMode || hP == 0)
                {
                    hP = value;
                    if (HPListeners != null && HPListeners.Count > 0)
                        HPListeners.ForEach(x => x.OnHpChange(value));
                }
            }
        }
    }

    private int money;
    public int Money
    {
        get { return money; }
        set
        {
            if (value != money)
            {
                money = value;
                if (MoneyListeners != null && MoneyListeners.Count > 0)
                    MoneyListeners.ForEach(x => x.OnMoneyChange(value));
            }
        }
    }

    //TODO Enhancements

    public List<CardBehaviour> Deck;
    public List<CardBehaviour> Hand;
    public List<CardBehaviour> Graveyard;

    public List<CardData> FacingObstacles;

    public Player()
    {
        Color = CardColor.Black;
        HP = 12;
        StartingHand = 3;
        Money = 10;

        Deck = new List<CardBehaviour>();
        Hand = new List<CardBehaviour>();
        Graveyard = new List<CardBehaviour>();
        FacingObstacles = new List<CardData>();

        this.RegisterStateListener();
    }

    public void SetFacing(ObstacleData cardData)
    {
        if (FacingObstacles == null)
            FacingObstacles = new List<CardData>();
        FacingObstacles.Add(cardData);
        cardData.OnFlip(this);
    }

    public void PopulateDeck()
    {
        Deck = new List<CardBehaviour>();
        List<CardData> ToBeDeck = ResourceController.GetStartingCards(this);
        for(int i = 0; i < ToBeDeck.Count; i ++)
        {
            CardData Current = ToBeDeck[i];
            GameObject CardObj = UIController.Instance.CreateCard(Current, false);
            CardObj.GetComponent<DraggableElement>().enabled = false;
            CardObj.transform.position = UIController.Instance.DeckPlaceHolder.transform.position;
            Deck.Add(CardObj.GetComponent<CardBehaviour>());
        }
        Deck.Shuffle();
        Deck.Shuffle();
    }

    public void Draw(int Qtd = 2)
    {
        for (int i = 0; i < Qtd; i++)
        {
            if (Deck.Count > 0)
            {
                Deck[0].Show = true;
                UIController.Instance.DrawCard(Deck[0].gameObject);
                Deck.RemoveAt(0);
            }
            else
            {
                PlayController.Instance.StartCoroutine(GraveyardToDeck(Qtd - i));
                break;
            }
        }
    }

    public IEnumerator GraveyardToDeck(int RemainingDraw = 0)
    {
        yield return new WaitForSeconds(0.5f);

        for (int j = 0; j < Graveyard.Count; j++)
        {
            Graveyard[j].Show = false;
            Graveyard[j].Undispose();
            Graveyard[j].ToDeck();
        }
        Deck.AddRange(Graveyard);
        Deck.Shuffle();
        Graveyard = new List<CardBehaviour>();

        UIController.Warn("Shuffling Graveyard to deck");

        yield return new WaitForSeconds(1.5f);

        if(RemainingDraw > 0)
        {
            Draw(RemainingDraw);
        }
    }

    public void OnStateChange(PlayState NewState)
    {
        if(NewState == PlayState.DrawBuy)
        {
            if (Hand.Count <= 3)
                Draw(2);
        }
    }
}
