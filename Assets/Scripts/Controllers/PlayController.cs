using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController : GameController
{
    public static new PlayController Instance
    {
        get
        {
            return GameController.Instance as PlayController;
        }

        set
        {
            GameController.Instance = value;
        }
    }

    private static List<IStateListener> StateListeners;

    public static void RegisterStateListener(IStateListener Listener)
    {
        if (StateListeners == null)
            StateListeners = new List<IStateListener>();
        if (!StateListeners.Contains(Listener))
            StateListeners.Add(Listener);
    }

    public static void DisposeStateListener(IStateListener Listener)
    {
        if (StateListeners != null)
        {
            StateListeners.Remove(Listener);
        }
    }

    public Mission PlayingMission;

    protected PlayState currentState;
    public PlayState CurrentState
    {
        get { return currentState; }
        set
        {
            if (StateListeners != null && StateListeners.Count > 0)
                StateListeners.ForEach(x => x.OnStateChange(value));
            currentState = value;
        }
    }

    protected Player currentPlayer;
    public Player CurrentPlayer
    {
        get { return currentPlayer; }
        set
        {
            if (value == null)
                return;
            currentPlayer = value;
            if (!value.Started)
                value.Started = true;
        }
    }

    public void ObstacleDefeated(ObstacleData Obstacle)
    {
        ActiveObstacles.Remove(Obstacle);

        if(ActiveObstacles.Count == 0)
        {
            PlayingMission.OnSceneStart();
        }

    }

    public int PlayerCount
    {
        get
        {
            if (players == null)
                return 0;
            else
                return players.Count;
        }
    }

    public int BlackMarketCards = 6;

    public bool Ending = false;

    public List<ObstacleData> ActiveObstacles;

    public override void Begin()
    {
        if (players == null)
            players = new List<Player>();
    }

    public override IEnumerator TimedBegin()
    {
        yield return new WaitForSeconds(0.1f);

        UIController.Warn("Game Started");

        Player TestPlayer = new Player();

        players.Add(TestPlayer);

        CurrentPlayer = TestPlayer;

        PlayingMission.OnSceneStart();

        CurrentState = PlayState.PlayCards;

    }

    public void EndGame(string Message = "")
    {
        StartCoroutine(ToGameOver(Message));
    }

    public IEnumerator ToGameOver(string Message = "")
    {
        Ending = true;

        yield return new WaitForSeconds(2f);

        if (Message.Equals(""))
        {
            EndGameMessage = "Winner";
            UIController.Warn("Game Ended");
        }
        else
        {
            EndGameMessage = "You lost.";
            UIController.Warn(Message);
        }

        yield return new WaitForSeconds(2f);
        
        SceneManager.LoadSceneAsync("Game Over");
    }

    public virtual IEnumerator TimedNextState()
    {
        yield return null;

        //Do stuff that needs timing

        yield return new WaitForSeconds(2);

        if (!Ending)
        {
            ConfirmAction();
        }
    } 

    public override void ConfirmAction()
    {
        switch(CurrentState)
        {
            case PlayState.PlayCards:
                CurrentState = PlayState.AssignDamage;
                break;
            case PlayState.AssignDamage:
                CurrentState = PlayState.TakeDamage;
                break;
            case PlayState.TakeDamage:
                CurrentState = PlayState.DrawBuy;
                break;
            case PlayState.DrawBuy:
                CurrentState = PlayState.PlayCards;
                break;
            default:
                CurrentState = PlayState.PlayCards;
                break;
        }

        UIController.Warn(currentState.ToString());

        if(CurrentState == PlayState.TakeDamage)
        {
            StartCoroutine(TimedNextState());
        }

    }

}

public enum PlayState
{
    PlayCards,
    AssignDamage,
    TakeDamage,
    DrawBuy
}
