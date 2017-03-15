using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class HandZone : DropZone
{
    public static List<IHandZoneListener> HandZoneListeners;

    public static void RegisterHandZoneListener(IHandZoneListener Listener)
    {
        if (HandZoneListeners == null)
            HandZoneListeners = new List<IHandZoneListener>();
        if (!HandZoneListeners.Contains(Listener))
            HandZoneListeners.Add(Listener);
    }

    public static void DisposeHandZoneListener(IHandZoneListener Listener)
    {
        if (HandZoneListeners != null)
        {
            HandZoneListeners.Remove(Listener);
        }
    }

    protected override void DispatchDropListeners(GameObject DropItem)
    {
        base.DispatchDropListeners(DropItem);
        if (HandZoneListeners != null)
        {
            MonoBehaviour[] Components = DropItem.GetComponents<MonoBehaviour>();
            for (int j = 0; j < Components.Length; j++)
            {
                MonoBehaviour CurrentComponent = Components[j];
                if ((CurrentComponent as ICard) != null)
                {
                    for (int i = 0; i < HandZoneListeners.Count; i++)
                    {
                        HandZoneListeners[i].OnHandZoneDropped(CurrentComponent as ICard);
                    }
                }
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
            if(Dropped.GetComponent<CardBehaviour>())
            {
                PlayController.Instance.CurrentPlayer.Hand.Add(Dropped.GetComponent<CardBehaviour>());
            }
        }
    }

    public override void OnDragListen(GameObject Dragged)
    {
        if (Dragged.GetComponent<CardBehaviour>() && Cards.Contains(Dragged.GetComponent<CardBehaviour>()))
        {
            PlayController.Instance.CurrentPlayer.Hand.Remove(Dragged.GetComponent<CardBehaviour>());
        }
        base.OnDragListen(Dragged);
    }

    protected override void RemoveICard(ICard Played)
    {
        if (!Cards.Contains(Played))
            return;
        base.RemoveICard(Played);
        if (HandZoneListeners != null)
        {
            for (int i = 0; i < HandZoneListeners.Count; i++)
            {
                HandZoneListeners[i].OnHandZoneRemoved(Played);
            }
        }
    }
}

