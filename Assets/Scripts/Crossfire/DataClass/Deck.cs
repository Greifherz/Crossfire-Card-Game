using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Deck : ScriptableObject
{
    #if UNITY_EDITOR
    [MenuItem("Assets/Create/Deck")]
    public static void CreateDeck()
    {
        ScriptableObjectUtility.CreateAsset<Deck>();
    }
    #endif

    public CardColor Color;
    public List<CardData> CardsInDeck;
}

