using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class DropForwarder : MonoBehaviour, IDropHandler
{
    public DropZone ZoneToForward;

    public void OnDrop(PointerEventData eventData)
    {
        ZoneToForward.OnDrop(eventData);
    }
}
