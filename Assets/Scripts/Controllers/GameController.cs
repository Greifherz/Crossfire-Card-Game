using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class GameController : MonoBehaviour
{
    protected static GameController instance;
    public static GameController Instance
    {
        get
        {
            if (instance != null)
                return instance;

            GameObject GameControllerObj = new GameObject("GameController");
            Instance = GameControllerObj.AddComponent<GameController>();
            Instance.Mode = GameMode.Lobby;
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    protected static List<Player> players;
    public static List<Player> Players
    {
        get
        {
            if (players == null)
                players = new List<Player>();
            return players;
        }

        set
        {
            if(value != null)
                players = value;
        }
    }

    public static string EndGameMessage = "";

    public GameMode Mode;    

    void Start()
    {
        Instance = this;
        Begin();
        StartCoroutine(TimedBegin());
    }

    public virtual void Begin()
    {

    }

    public virtual IEnumerator TimedBegin()
    {
        yield return null;
    }

    public virtual void ConfirmAction()
    {

    }
}


public enum GameMode
{
    Lobby,
    Game
}
