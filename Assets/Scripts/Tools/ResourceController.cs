using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class ResourceController
{
    private static Dictionary<string, Deck> CachedDecks;
    private static Dictionary<string, CardData> CachedCardsData;

    public static List<CardData> GetStartingCards(Player player)
    {
        List<CardData> StartingCards = new List<CardData>();

        if (CachedDecks == null)
            LoadDecks();

        StartingCards = new List<CardData>(CachedDecks[player.Color.ToString()].CardsInDeck);

        return StartingCards;
    }

    public static void LoadCardsData()
    {
        CardData[] CardsData = Resources.LoadAll<CardData>("Cards");
        for(int i = 0; i < CardsData.Length; i++)
        {
            CardData CurrentCardData = CardsData[i];
            if (CachedCardsData == null)
                CachedCardsData = new Dictionary<string, CardData>();
            if (CachedCardsData.ContainsKey(CurrentCardData.name))
                continue;
            CachedCardsData.Add(CurrentCardData.name, CurrentCardData);
        }
    }

    public static void LoadDecks()
    {
        Deck[] DecksData = Resources.LoadAll<Deck>("Decks");
        for (int i = 0; i < DecksData.Length; i++)
        {
            Deck CurrentDeck = DecksData[i];
            if (CachedDecks == null)
                CachedDecks = new Dictionary<string, Deck>();
            if (CachedDecks.ContainsKey(CurrentDeck.Color.ToString()))
                continue;
            CachedDecks.Add(CurrentDeck.Color.ToString(), CurrentDeck);
        }
    }

    public static CardData LoadCardData(string Name)
    {
        if (CachedCardsData == null)
            LoadCardsData();

        if (!CachedCardsData.ContainsKey(Name))
            return null;
        return CachedCardsData[Name];
    }


}