using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Katana : CardData, IPlayZoneListener, IStateListener
{
    private List<CardBehaviour> PlayedBlackCards;

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Card/Katana")]
    public static void CreateKatana()
    {
        ScriptableObjectUtility.CreateAsset<Katana>();
    }
#endif

    public override void OnPlayed()
    {
        base.OnPlayed();

        Behaviour.CardText.text = "Katana";
        Damage = 0;
        PlayedBlackCards = new List<CardBehaviour>();
        foreach (CardBehaviour Card in PlayZone.PlayedCards)
        {
            if (Card == null) continue;
            if (Card.Equals(Behaviour)) continue;
            if (Card.Data.Color == CardColor.Black)
            {
                PlayedBlackCards.Add(Card);
            }
        }

        Damage += PlayedBlackCards.Count;
        Behaviour.CardText.text += " +" + Damage.ToString();
        this.RegisterPlayZoneListener();
        this.RegisterStateListener();
    }

    public override void OnDispose()
    {
        Behaviour.CardText.text = "Katana";
        this.DisposePlayZoneListener();
        this.DisposeStateListener();
        Damage = 0;
    }

    public void OnPlayZoneDropped(ICard Played)
    {
        Behaviour.CardText.text = "Katana";
        Damage = 0;
        PlayedBlackCards = new List<CardBehaviour>();
        foreach (CardBehaviour Card in PlayZone.PlayedCards)
        {
            if (Card == null) continue;
            if (Card.Equals(Behaviour)) continue;
            if (Card.Data.Color == CardColor.Black)
            {
                PlayedBlackCards.Add(Card);
            }
        }

        Damage += PlayedBlackCards.Count;
        Behaviour.CardText.text += " +" + Damage.ToString();
    }

    public void OnPlayZoneRemoved(ICard Played)
    {
        Behaviour.CardText.text = "Katana";
        Damage = 0;
        PlayedBlackCards = new List<CardBehaviour>();
        foreach (CardBehaviour Card in PlayZone.PlayedCards)
        {
            if (Card == null) continue;
            if (Card.Equals(Behaviour)) continue;
            if (Card.Data.Color == CardColor.Black)
            {
                PlayedBlackCards.Add(Card);
            }
        }

        Damage += PlayedBlackCards.Count;
        Behaviour.CardText.text += " +" + Damage.ToString();
    }

    public void OnStateChange(PlayState NewState)
    {
        if(NewState == PlayState.AssignDamage)
        {
            this.DisposePlayZoneListener();
        }
    }
}
