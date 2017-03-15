using UnityEngine;

public interface IPlayable
{
    void DroppedOn(DropZone ZoneDropped);
    void SetSlot(Slot SlotObj);
    void Dispose();
}
