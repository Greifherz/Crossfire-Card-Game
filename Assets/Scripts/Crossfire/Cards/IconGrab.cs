using UnityEngine;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class IconGrab : CardData, IPlayZoneListener
{
#if UNITY_EDITOR
    [MenuItem("Assets/Create/Card/IconGrab")]
    public static void CreateIconGrab()
    {
        ScriptableObjectUtility.CreateAsset<IconGrab>();
    }
#endif

    public override void OnPlayed()
    {
        base.OnPlayed();

        Behaviour.CardText.text = "Icon Grab";

        bool AddDamage = false;
        foreach(CardBehaviour Card in PlayZone.PlayedCards)
        {
            if (Card == null) continue;
            if (Card.Equals(Behaviour)) continue;
            if (Card.Data.Color == CardColor.Green)
            {
                AddDamage = true;
                Card.GetComponent<DraggableElement>().enabled = false;
                Behaviour.GetComponent<DraggableElement>().enabled = false;
                break;
            }
        }
        if (AddDamage)
        {
            Damage++;
            Color = CardColor.All;
            Behaviour.CardText.text += " (All) ";
        }
        else
        {
            this.RegisterPlayZoneListener();
        }
    }

    public override void OnDispose()
    {
        base.OnDispose();

        this.DisposePlayZoneListener();
        Damage = 0;
        Color = CardColor.Green;
        Behaviour.CardText.text = "Icon Grab";
    }

    public void OnPlayZoneDropped(ICard Played)
    {
        bool AddDamage = false;
        foreach (CardBehaviour Card in PlayZone.PlayedCards)
        {
            if (Card == null) continue;
            if (Card.Equals(Behaviour)) continue;
            if (Card.Data.Color == CardColor.Green)
            {
                AddDamage = true;
                Card.GetComponent<DraggableElement>().enabled = false;
                Behaviour.GetComponent<DraggableElement>().enabled = false;
                break;
            }
        }
        if (AddDamage)
        {
            Damage++;
            Color = CardColor.All;
            Behaviour.CardText.text += " (All) ";
            this.DisposePlayZoneListener();
        }
    }

    public void OnPlayZoneRemoved(ICard Played)
    {

    }
}
