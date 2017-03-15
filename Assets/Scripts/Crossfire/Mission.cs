using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Mission : ScriptableObject
{

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Mission")]
    public static void CreateMission()
    {
        ScriptableObjectUtility.CreateAsset<Mission>();
    }
#endif

    public Deck Obstacles;
    public Deck HardObstacles;//TODO
    public Deck Events;//TODO
    public Deck BlackMarket;//TODO

    [HideInInspector]
    public List<CardData> CachedObstacles;
    [HideInInspector]
    public List<CardData> CachedHardObstacles;
    [HideInInspector]
    public List<CardData> CachedEvents;
    [HideInInspector]
    public List<CardData> CachedBlackMarket;

    public int ObstaclesPerScene = 1;

    public int SceneCount;
    [HideInInspector][System.NonSerialized]
    public int CurrentScene = 0;

    public virtual void OnSceneStart()
    {
        if (CurrentScene == 0)
        {
            int PlayerCount = GameController.Players.Count;

            CachedObstacles = new List<CardData>(Obstacles.CardsInDeck);

            CachedObstacles.Shuffle();

            for (int i = 0; i < PlayerCount; i++)
            {
                Player CurrentPlayer = GameController.Players[i];
                for(int j = 0; j < ObstaclesPerScene; j++ )
                    CreateObstacle(CurrentPlayer);
            }
            CurrentScene++;
        }
        else
        {
            PlayController.Instance.EndGame();
        }
    }

    public virtual void CreateObstacle(Player PlayerToFace)
    {
        ObstacleData ToAddObstacle = CachedObstacles[0] as ObstacleData;
        CachedObstacles.RemoveAt(0);
        if (ToAddObstacle == null)
            throw new System.Exception("Tried to use a non-obstacle card as an obstacle.");
        GameObject ObstacleCardObj = UIController.Instance.CreateObstacle(ToAddObstacle);
        ObstacleCardObj.GetComponent<CardBehaviour>().Show = true;
        ObstacleData AddedObstacle = ObstacleCardObj.GetComponent<CardBehaviour>().Data as ObstacleData;
        PlayerToFace.SetFacing(AddedObstacle);
        UIController.Instance.SetObstacle(ObstacleCardObj);

        if (PlayController.Instance.ActiveObstacles == null)
            PlayController.Instance.ActiveObstacles = new List<ObstacleData>();
        PlayController.Instance.ActiveObstacles.Add(AddedObstacle);
    }

}
