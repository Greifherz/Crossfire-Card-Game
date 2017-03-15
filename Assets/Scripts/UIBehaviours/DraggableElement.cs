using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public class DraggableElement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    #region Static Properties

    protected static GameObject dragItem = null;
    public static GameObject DragItem
    {
        get { return dragItem; }
        set
        {
            if(value != null && !value.Equals(dragItem))
            {
                dragItem = value;
                if (DragListeners != null)
                {
                    for (int i = 0; i < DragListeners.Count; i++)
                    {
                        DragListeners[i].OnDragListen(value);
                    }
                }
            }
            else if(value == null)
            {
                dragItem = value;
                //If there's drag end listener, place it here
            }
        }
    }


    protected static List<IDragListener> DragListeners;

    #endregion

    #region Static Methods

    public static void RegisterDragListener(IDragListener Listener)
    {
        if (DragListeners == null)
            DragListeners = new List<IDragListener>();
        if (!DragListeners.Contains(Listener))
            DragListeners.Add(Listener);
    }

    public static void DisposeDragListener(IDragListener Listener)
    {
        if(DragListeners != null)
        {
            DragListeners.Remove(Listener);
        }
    }

    #endregion

    #region Instance Properties

    

    [HideInInspector]
    public List<Action> ObjectActionCallbacks;

    public bool UseOffset = true;
    protected Vector2 OffSet;
    protected CanvasGroup UICanvasGroup;

    protected bool dragging = false;
    public bool Dragging
    {
        get { return dragging; }
        set
        {
            if (dragging != value)
            {
                if (value)
                {
                    DragItem = gameObject;
                    if (UseOffset)
                        OffSet = new Vector2(transform.position.x, transform.position.y) - (Vector2)Input.mousePosition;
                    OnDragStart();
                    if (UICanvasGroup != null)
                    {
                        UICanvasGroup.blocksRaycasts = false;
                    }
                }
                else
                {
                    DragItem = null;
                    if (UICanvasGroup != null)
                    {
                        UICanvasGroup.blocksRaycasts = true;
                    }
                    OnDragEnd();
                }
                dragging = value;
            }
        }
    }

    #endregion

    #region Interface Implementation

    public void OnBeginDrag(PointerEventData Data)
    {
        Dragging = true;
    }

    public void OnEndDrag(PointerEventData Data)
    {
        Dragging = false;
    }

    public void OnDrag(PointerEventData Data)
    {
        if (UseOffset)
            transform.position = Data.position + OffSet;
        else
            transform.position = Data.position;
    }

    #endregion

    #region Callbacks

    void Start()
    {
        UICanvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void RegisterCallback(Action ToRegister)
    {
        if (ObjectActionCallbacks == null)
            ObjectActionCallbacks = new List<Action>();
        if(!ObjectActionCallbacks.Contains(ToRegister))
            ObjectActionCallbacks.Add(ToRegister);
    }

    public virtual void OnDragStart()
    {
        if (ObjectActionCallbacks != null && ObjectActionCallbacks.Count > 0)
        {
            List<Action> TempCallbackList = new List<Action>(ObjectActionCallbacks);
            StartCoroutine(CheckAndCall(TempCallbackList));
        }
        ObjectActionCallbacks = new List<Action>();
    }

    public virtual void OnDragEnd()
    {

    }

    public IEnumerator CheckAndCall(List<Action> CallbackList)
    {
        yield return null;
        yield return null;
        yield return null;

        if(dragging)
        {
            foreach (Action Callback in CallbackList)
                Callback();
        }
        else
        {
            ObjectActionCallbacks.AddRange(CallbackList);
        }

    }

    #endregion

    #region Implementation

    #endregion
}
