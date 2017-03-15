using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public class DropZone : MonoBehaviour , IDropHandler, IDropListener, IDragListener
{
    private static List<IDropListener> DropListeners;

    public static void RegisterDropListener(IDropListener Listener)
    {
        if (DropListeners == null)
            DropListeners = new List<IDropListener>();
        if (!DropListeners.Contains(Listener))
            DropListeners.Add(Listener);
    }

    public static void DisposeDropListener(IDropListener Listener)
    {
        if(DropListeners != null)
        {
            DropListeners.Remove(Listener);
        }
    }

    public List<Slot> Slots;
    protected List<ICard> Cards;

    void Start()
    {
        Cards = new List<ICard>();
        this.RegisterDragListener();
        this.RegisterDropListener();
    }

    public void DisposeCards()
    {
        if (Cards == null)
            throw new Exception("Cards were null on dispose.");
        List<ICard> CachedCards = Cards;
        for (int i = 0; i < CachedCards.Count; i++)
        {
            Cards[i].Dispose();
            Cards.RemoveAt(i);
            i--;
        }
    }

    protected virtual void RemoveICard(ICard Played)
    {
        if (!Cards.Contains(Played))
            return;
        Cards.Remove(Played);
        for(int i = 0; i < Cards.Count; i++)
        {
            Cards[i].SetSlot(Slots[i]);
        }
    }

    public int GetIndex(ICard Caller)
    {
        if (!Cards.Contains(Caller))
            throw new System.Exception("IPlayable not registered in the zone. Cannot get slot position.");
        return Cards.Count - 1 - Cards.IndexOf(Caller);
    }

    public Slot GetSlot(ICard Caller)
    {
        if (!Cards.Contains(Caller))
            throw new System.Exception("IPlayable not registered in the zone. Cannot get slot position.");
        return Slots[Cards.IndexOf(Caller)];
    }
    
    public Vector2 GetSlotPos(ICard Caller)
    {
        if (!Cards.Contains(Caller))
            throw new System.Exception("IPlayable not registered in the zone. Cannot get slot position.");
        return Slots[Cards.IndexOf(Caller)].transform.position;
    }

    public void AddICard(ICard ToAdd)
    {
        if(!Cards.Contains(ToAdd))
        {
            Cards.Add(ToAdd);
        }
    }

    public void OnDrop(PointerEventData Data)
    {
        DispatchDropListeners(DraggableElement.DragItem);
        DraggableElement.DragItem = null;
    }

    protected virtual void DispatchDropListeners(GameObject DropItem)
    {
        for (int i = 0; i < DropListeners.Count; i++)
        {
            DropListeners[i].OnDropped(DropItem, gameObject);
        }
    }

    public virtual void OnDropped(GameObject Dropped, GameObject Zone)
    {
        if (Dropped == null)
            throw new Exception("DropZone -> The object just dropped is null.");
        if (Zone == null)
            throw new Exception("DropZone -> The Zone you dropped on is null.");

        if (Zone.Equals(gameObject))
        {
            MonoBehaviour[] Components = Dropped.GetComponents<MonoBehaviour>();
            for (int i = 0; i < Components.Length; i++)
            {
                MonoBehaviour CurrentComponent = Components[i];
                if ((CurrentComponent as ICard) != null)
                {
                    ICard CurrentCard = (CurrentComponent as ICard);
                    AddICard(CurrentCard);
                }
            }
        }
    }

    public virtual void OnDragListen(GameObject Dragged)
    {
        MonoBehaviour[] Components = Dragged.GetComponents<MonoBehaviour>();
        for (int i = 0; i < Components.Length; i++)
        {
            MonoBehaviour CurrentComponent = Components[i];
            if ((CurrentComponent as ICard) != null)
            {
                ICard CurrentCard = (CurrentComponent as ICard);
                RemoveICard(CurrentCard);
            }
        }
    }

}
