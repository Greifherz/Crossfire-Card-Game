using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class ObstacleBehaviour : CardBehaviour
{
    public RectTransform Marker;

    private int CurrentTrack = 0;

    public void MoveMarkerTo(int DamageTrackLevel)
    {
        Vector2 NewPos = (Vector2)Marker.transform.localPosition + new Vector2(15 * (DamageTrackLevel - CurrentTrack), 0);
        Marker.transform.localPosition = NewPos;
        CurrentTrack = DamageTrackLevel;
    }

    public override void Dispose()
    {
        SlotAt = null;
        MoveTo(UIController.Instance.GetDisposeHolder(this).transform.position);

        if (GetComponent<DraggableElement>())
            GetComponent<DraggableElement>().enabled = false;
        if (GetComponent<ZoomOnHoverElement>())
            GetComponent<ZoomOnHoverElement>().enabled = false;

        Disposed = true;
        ZoneAt = null;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if(PlayController.Instance.CurrentState == PlayState.AssignDamage && DraggableElement.DragItem.GetComponent<CardBehaviour>())
        {
            CardBehaviour DroppedCard = DraggableElement.DragItem.GetComponent<CardBehaviour>();
            ObstacleData Obstacle = Data as ObstacleData;
            Obstacle.OnCardDroppedOn(DroppedCard.Data);
            DroppedCard.RegisterDragCallback(delegate
            {
                Obstacle.OnUnassign(DroppedCard.Data);
            });
        }
        else
        {
            CardBehaviour DroppedCard = DraggableElement.DragItem.GetComponent<CardBehaviour>();
            if (DroppedCard == null)
                return;
            DroppedCard.ZoneAt.OnDrop(eventData);
        }
    }
}
