using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CardBehaviour : MonoBehaviour, ICard, IStateListener, IDropHandler
{
    [HideInInspector]
    public DropZone zoneAt;
    public DropZone ZoneAt
    {
        get { return zoneAt; }
        set { zoneAt = value; }
    }

    public Image CardBack;
    public Image CardImage;
    public Text CardText;
    public int ZoneIndex;

    protected bool show = false;
    public bool Show
    {
        get { return show; }
        set
        {
            if (value == show)
                return;

            if(value)
            {
                CardBack.enabled = false;
                CardImage.enabled = true;
                CardText.enabled = true;
                GetComponent<ZoomOnHoverElement>().enabled = true;
                if (data != null && data.Category == CardCategory.Action)
                    GetComponent<DraggableElement>().enabled = true;
            }
            else
            {
                CardBack.enabled = true;
                CardImage.enabled = false;
                CardText.enabled = false;
                GetComponent<DraggableElement>().enabled = false;
                GetComponent<ZoomOnHoverElement>().enabled = false;
            }

            show = value;

        }
    }

    protected bool Moving = false;
    protected bool Disposed = false;

    protected Slot slotAt;
    public Slot SlotAt
    {
        get
        {
            return slotAt;
        }
        set
        {
            if (Disposed)
                return;

            if(slotAt != null)
            {
                SlotAt.ObjectOnTop = null;
            }

            slotAt = value;

            if (value != null)
            {
                MoveTo(value);
                slotAt.ObjectOnTop = gameObject;
            }
        }
    }

    protected CardData data;
    public virtual CardData Data
    {
        get { return data; }
        set
        {
            data = Instantiate(value);
            data.Behaviour = this;
            CardText.text = data.Name;
            CardImage.sprite = data.CardSprite;
        }
    }

    protected virtual void MoveTo(Slot SlotObj)
    {
        if (Moving)
        {
            StopAllCoroutines();
        }
        StartCoroutine(MoveToRoutine((RectTransform)SlotObj.transform));
    }

    protected virtual void MoveTo(Vector2 Pos)
    {
        if (Moving)
        {
            StopAllCoroutines();
        }
        StartCoroutine(MoveToRoutine(Pos));
    }

    protected virtual IEnumerator MoveToRoutine(Vector2 Pos)
    {
        OnStartMoveSlot();

        yield return null;

        Vector2 Dir = (Pos - (Vector2)transform.position).normalized * 18;

        while (Vector2.Distance((Vector2)transform.position, Pos) > Dir.magnitude + 0.1f)
        {
            transform.position += (Vector3)Dir;
            OnUpdateMoveSlot();
            yield return null;
        }

        transform.position = Pos;
        if (ZoneAt != null)
            transform.SetSiblingIndex(ZoneAt.GetIndex(this));
        yield return null;

        OnArriveMoveSlot();
    }

    protected virtual IEnumerator MoveToRoutine(RectTransform Slot)
    {
        OnStartMoveSlot();

        yield return null;
        Vector2 Pos = Slot.position;

        Vector2 Dir = (Pos - (Vector2)transform.position).normalized * 18;

        while (Vector2.Distance(transform.position, Pos) > Dir.magnitude + 0.1f)
        {
            transform.position += (Vector3)Dir;
            OnUpdateMoveSlot();
            yield return null;
        }

        transform.position = Slot.transform.position;

        if (ZoneAt != null && Data.Category != CardCategory.Obstacle)
            transform.SetSiblingIndex(ZoneAt.GetIndex(this));

        yield return null;

        OnArriveMoveSlot();
    }

    protected virtual void OnStartMoveSlot()
    {
        Moving = true;
    }

    protected virtual void OnUpdateMoveSlot()
    {

    }

    protected virtual void OnArriveMoveSlot()
    {
        Moving = false;
        transform.SetAsFirstSibling();
    }

    public virtual void Undispose()
    {
        Disposed = false;
    }

    public virtual void Dispose()
    {
        SlotAt = null;
        MoveTo(UIController.Instance.GetDisposeHolder().transform.position);

        if (GetComponent<DraggableElement>())
            GetComponent<DraggableElement>().enabled = false;
        if (GetComponent<ZoomOnHoverElement>())
            GetComponent<ZoomOnHoverElement>().enabled = false;

        PlayController.Instance.CurrentPlayer.Hand.Remove(this);
        PlayController.Instance.CurrentPlayer.Graveyard.Add(this);

        Disposed = true;
        ZoneAt = null;

    }

    public virtual void ToDeck()
    {
        MoveTo(UIController.Instance.DeckPlaceHolder.transform.position);
    }

    public virtual void SetSlot(Slot SlotObj)
    {
        SlotAt = SlotObj;
    }

    public virtual void DroppedOn(DropZone ZoneDropped)
    {
        if (GetComponent<DraggableElement>())
            GetComponent<DraggableElement>().Dragging = false;
        if (GetComponent<ZoomOnHoverElement>())
        {
            ZoomOnHoverElement ZoomElement = GetComponent<ZoomOnHoverElement>();
            if (ZoomElement.Hovering)
            {
                ZoomElement.OriginalIndex = 0;
            }
            ZoomElement.Hovering = false;
        }
        ZoneAt = ZoneDropped;
        SlotAt = ZoneDropped.GetSlot(this);
    }

    public List<Damage> GetDamage()
    {
        if (Data == null)
            throw new Exception("Card With no data attached dropped in playzone.");

        return Data.GetDamage();
    }

    public void RegisterDragCallback(Action ToRegister)
    {
        if (GetComponent<DraggableElement>())
            GetComponent<DraggableElement>().RegisterCallback(ToRegister);
    }

    void Start()
    {
        this.RegisterStateListener();
    }

    public void OnPlayed()
    {
        Data.OnPlayed();
    }

    public void OnUnplayed()
    {

    }

    public virtual void OnStateChange(PlayState NewState)
    {
        if (Data as ObstacleData == null && Show && !Disposed)
        {
            if (NewState == PlayState.PlayCards || NewState == PlayState.AssignDamage)
            {
                if(NewState == PlayState.PlayCards)
                {
                    GetComponent<DraggableElement>().enabled = true;
                }
                else if (NewState == PlayState.AssignDamage)
                {
                    if (ZoneAt.Equals(UIController.Instance.PlayZone))
                    {
                        GetComponent<DraggableElement>().enabled = true;
                    }
                    else
                    {
                        GetComponent<DraggableElement>().enabled = false;
                    }
                }
            }
            else
            {
                if (!Disposed && ZoneAt != null && ZoneAt.Equals(UIController.Instance.PlayZone))
                    Dispose();

                GetComponent<DraggableElement>().enabled = false;
            }
        }
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        ZoneAt.OnDrop(eventData);        
    }
}
