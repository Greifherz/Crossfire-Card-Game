using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public static class ExtensionMethods
{
    #region Interface Extensions

    #region IStateListener

    public static void RegisterStateListener(this IStateListener Listener)
    {
        PlayController.RegisterStateListener(Listener);
    }

    public static void DisposeStateListener(this IStateListener Listener)
    {
        PlayController.DisposeStateListener(Listener);
    }

    #endregion

    #region IPlayZoneListener

    public static void RegisterPlayZoneListener(this IPlayZoneListener Listener)
    {
        PlayZone.RegisterPlayzoneListener(Listener);
    }

    public static void DisposePlayZoneListener(this IPlayZoneListener Listener)
    {
        PlayZone.DisposePlayZoneListener(Listener);
    }

    #endregion

    #region IHandZoneListener

    public static void RegisterHandZoneListener(this IHandZoneListener Listener)
    {
        HandZone.RegisterHandZoneListener(Listener);
    }

    public static void DisposeHandZoneListener(this IHandZoneListener Listener)
    {
        HandZone.DisposeHandZoneListener(Listener);
    }

    #endregion

    #region IDropListener

    public static void RegisterDropListener(this IDropListener Listener)
    {
        DropZone.RegisterDropListener(Listener);
    }

    public static void DisposeDropListener(this IDropListener Listener)
    {
        DropZone.DisposeDropListener(Listener);
    }

    #endregion

    #region IDragListener

    public static void RegisterDragListener(this IDragListener Listener)
    {
        DraggableElement.RegisterDragListener(Listener);
    }

    public static void DisposeDragListener(this IDragListener Listener)
    {
        DraggableElement.DisposeDragListener(Listener);
    }

    #endregion

    #endregion


    #region ListExtensions

    #region Shuffle
    public static void Shuffle<T>(this List<T> ListOfStuff)
    {
        int Rng = Random.Range(5, 9);
        for (int i = 0; i < Rng ; i++)
        {
            ListOfStuff.ShuffleOnce<T>();
        }
    }

    public static void ShuffleOnce<T>(this List<T> ListOfStuff)
    {
        int Mid = Mathf.RoundToInt(ListOfStuff.Count / 2);
        Mid += Random.Range(-2, 3);
        while (Mid > ListOfStuff.Count || Mid < 0)
        {
            Mid = Mathf.RoundToInt(ListOfStuff.Count / 2) + Random.Range(-2, 3);
        }

        List<T> First = new List<T>();
        int OriginalSize = ListOfStuff.Count;

        for(int i = 0; i < Mid; i++)
        {
            First.Add(ListOfStuff[0]);
            ListOfStuff.RemoveAt(0);
        }
        List<T> Second = ListOfStuff;

        List<T> Final = new List<T>();

        First.Reverse();

        for (int i = 0; i < OriginalSize; i++)
        {
            if (i % 2 == 0 && First.Count > 0)
            {
                Final.Add(First[0]);
                First.RemoveAt(0);
            }
            else if (Second.Count > 0)
            {
                Final.Add(Second[0]);
                Second.RemoveAt(0);
            }
            else if (First.Count > 0)
            {
                Final.Add(First[0]);
                First.RemoveAt(0);
            }
        }

        ListOfStuff.RemoveRange(0, ListOfStuff.Count);
        foreach(T Element in Final)
        {
            ListOfStuff.Add(Element);
        }
    }
    #endregion

    #endregion
}