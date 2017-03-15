using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ZoomOnHoverElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Instance Properties

    protected float MaxScale;
    protected float StartingScale;

    [HideInInspector]
    public int OriginalIndex;

    public float MaxSize = 1.3f;
    public bool Resizing = false;

    protected bool hovering = false;
    public bool Hovering
    {
        get
        {
            return hovering;
        }
        
        set
        {
            if(hovering != value && DraggableElement.DragItem == null)
            {
                if(!hovering && value)
                {
                    ResizeWrap();
                }
                else
                {
                    ResizeWrap(false);
                }
                hovering = value;
            }
        }
    }

    #endregion

    #region Interface Implementation

    public void OnPointerEnter(PointerEventData Data)
    {
        Hovering = true;
    }

    public void OnPointerExit(PointerEventData Data)
    {
        if (DraggableElement.DragItem == null || !DraggableElement.DragItem.Equals(gameObject))
            Hovering = false;
    }

    #endregion

    #region Callbacks

    void Start()
    {
        MaxScale = transform.localScale.magnitude * MaxSize;
    }

    public virtual void OnResizeStart()
    {

    }

    public virtual void OnResizeUpdate()
    {

    }

    public virtual void OnResizeEnd()
    {

    }

    #endregion

    #region Implementation

    protected void ResizeWrap(bool Increase = true)
    {
        if(Resizing)
        {
            StopAllCoroutines();
        }

        StartCoroutine(Resize(Increase));
    }

    protected IEnumerator Resize(bool Increase = true)
    {
        if (StartingScale == 0)
        {
            StartingScale = transform.localScale.magnitude;
        }

        if (!Resizing)
        {
            OnResizeStart();
            Resizing = true;
            if (Increase)
            {
                OriginalIndex = transform.GetSiblingIndex();
                transform.SetAsLastSibling();
            }
        }

        yield return null;


        if (Increase)
        {
            while (transform.localScale.magnitude < MaxScale)
            {
                transform.localScale += new Vector3(0.1f, 0.1f, 0);
                OnResizeUpdate();
                yield return null;
            }
        }
        else
        {
            while (transform.localScale.magnitude > StartingScale)
            {
                transform.localScale -= new Vector3(0.1f, 0.1f, 0);
                OnResizeUpdate();
                yield return null;
            }
        }

        yield return null;
        
        if (!Increase && DraggableElement.DragItem == null)
            transform.SetSiblingIndex(OriginalIndex);
        else
        {
            transform.SetAsLastSibling();
        }

        OnResizeEnd();
        Resizing = false;
    }

    #endregion

}
