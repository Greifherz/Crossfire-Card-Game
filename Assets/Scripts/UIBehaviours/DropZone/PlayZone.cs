using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PlayZone : DropZone
{
    public static List<CardBehaviour> PlayedCards;

    public static List<IPlayZoneListener> PlayZoneListeners;

    public static void RegisterPlayzoneListener(IPlayZoneListener Listener)
    {
        if (PlayZoneListeners == null)
            PlayZoneListeners = new List<IPlayZoneListener>();
        if (!PlayZoneListeners.Contains(Listener))
            PlayZoneListeners.Add(Listener);
    }

    public static void DisposePlayZoneListener(IPlayZoneListener Listener)
    {
        if (PlayZoneListeners != null)
        {
            PlayZoneListeners.Remove(Listener);
        }
    }

    protected override void DispatchDropListeners(GameObject DropItem)
    {
        base.DispatchDropListeners(DropItem);
        if (PlayZoneListeners != null)
        {
            MonoBehaviour[] Components = DropItem.GetComponents<MonoBehaviour>();
            for (int j = 0; j < Components.Length; j++)
            {
                MonoBehaviour CurrentComponent = Components[j];
                if ((CurrentComponent as ICard) != null)
                {
                    for (int i = 0; i < PlayZoneListeners.Count; i++)
                    {
                        PlayZoneListeners[i].OnPlayZoneDropped(CurrentComponent as ICard);
                    }
                }
            }
        }
    }

    protected override void RemoveICard(ICard Played)
    {
        if (!Cards.Contains(Played))
            return;
        base.RemoveICard(Played);
        if (PlayZoneListeners != null)
        {
            for (int i = 0; i < PlayZoneListeners.Count; i++)
            {
                PlayZoneListeners[i].OnPlayZoneRemoved(Played);
            }
        }
    }

    public override void OnDropped(GameObject Dropped, GameObject Zone)
    {
        base.OnDropped(Dropped, Zone);

        if (Dropped == null)
            throw new Exception("DropZone -> The object just dropped is null.");
        if (Zone == null)
            throw new Exception("DropZone -> The Zone you dropped on is null.");

        if (Zone.Equals(gameObject))
        {
            if (Dropped.GetComponent<CardBehaviour>())
            {
                if (PlayedCards == null)
                    PlayedCards = new List<CardBehaviour>();
                PlayedCards.Add(Dropped.GetComponent<CardBehaviour>());
                Dropped.GetComponent<CardBehaviour>().OnPlayed();
            }
        }
    }

    public override void OnDragListen(GameObject Dragged)
    {
        if (Dragged.GetComponent<CardBehaviour>() && Cards.Contains(Dragged.GetComponent<CardBehaviour>()))
        {
            PlayedCards.Remove(Dragged.GetComponent<CardBehaviour>());
        }
        base.OnDragListen(Dragged);
    }
}

