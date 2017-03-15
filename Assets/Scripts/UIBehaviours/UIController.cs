using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class UIController : MonoBehaviour,IDropListener, IStateListener {

    public static UIController Instance;

    public Image WarningPanelImage;

    public GameObject BlackMarketButton;

    public DropZone PlayZone;
    public DropZone HandZone;

    public Transform CardsHolder;
    public Transform ObstaclesHolder;

    public Slot[] ObstaclesPlaceHolders;

    public RectTransform DeckPlaceHolder;
    public RectTransform GraveyardPlaceHolder;

    public RectTransform ObstacleStartHolder;
    public RectTransform ObstacleDisposeHolder;

    void Start()
    {
        Instance = this;
        this.RegisterDropListener();
        this.RegisterStateListener();
        StartCoroutine(TimedStart());
    }

    public RectTransform GetDisposeHolder()
    {
        return GraveyardPlaceHolder;
    }

    public RectTransform GetDisposeHolder(CardBehaviour Behaviour)
    {
        if (typeof(ObstacleBehaviour).IsAssignableFrom(Behaviour.GetType()))
            return ObstacleDisposeHolder;
        return GraveyardPlaceHolder;
    }

    public void Draw(int Qtd = 1)
    {
        for(int i = 0; i < Qtd; i++)
        {
            DraggableElement.DragItem = Instantiate(Resources.Load("Card") as GameObject, DeckPlaceHolder.position,Quaternion.identity) as GameObject;
            DraggableElement.DragItem.transform.SetParent(CardsHolder);
            HandZone.OnDrop(new PointerEventData(EventSystem.current));
        }
    }

    public IEnumerator TimedStart()
    {
        yield return new WaitForSeconds(0.1f);
    }

    public void OnDropped(GameObject Dropped, GameObject Zone)
    {
        if (Dropped == null)
            throw new Exception("UIController -> The object just dropped is null.");
        if (Zone == null)
            throw new Exception("UIController -> The Zone you dropped on is null.");

        MonoBehaviour[] Components = Dropped.GetComponents<MonoBehaviour>();
        for (int i = 0; i < Components.Length; i++)
        {
            MonoBehaviour CurrentComponent = Components[i];
            if ((CurrentComponent as ICard) != null)
            {
                ICard CurrentCard = (CurrentComponent as ICard);
                CurrentCard.DroppedOn(Zone.GetComponent<DropZone>());
            }
        }
    }

    public void SetObstacle(GameObject obstacleCardObj)
    {
        int ObstacleCount = PlayController.Instance.CurrentPlayer.FacingObstacles.Count;        

        CardBehaviour ObstacleCard = obstacleCardObj.GetComponent<CardBehaviour>();
        if (ObstacleCard == null)
            throw new Exception("Card you tried to set as obstacle is not a card");

        ObstacleCard.ZoneAt = PlayZone;

        for (int i = 0; i < ObstaclesPlaceHolders.Length; i++)
        {
            if (ObstaclesPlaceHolders[i].ObjectOnTop == null)
            {
                ObstacleCard.SetSlot(ObstaclesPlaceHolders[i].GetComponent<Slot>());
                break;
            }
        }
    }

    public void DrawCard(GameObject Drawn)
    {
        DraggableElement.DragItem = Drawn;
        if (PlayController.Instance.CurrentState != PlayState.PlayCards)
            Drawn.GetComponent<DraggableElement>().enabled = false;
        HandZone.OnDrop(new PointerEventData(EventSystem.current));
    }

    public GameObject CreateObstacle(ObstacleData Data)
    {
        GameObject Card = Instantiate(Resources.Load("Obstacle") as GameObject, ObstacleStartHolder.position, Quaternion.identity) as GameObject;
        Card.name = Data.name;
        Card.GetComponent<ObstacleBehaviour>().Data = Instantiate(Data);
        Card.GetComponent<ObstacleBehaviour>().Data.name = Card.GetComponent<ObstacleBehaviour>().Data.name.Replace("(Clone)", "");
        Card.transform.SetParent(ObstaclesHolder);

        return Card;
    }

    public GameObject CreateCard(CardData Data,bool Drawn = true)
    {
        GameObject Card = Instantiate(Resources.Load("Card") as GameObject, DeckPlaceHolder.position, Quaternion.identity) as GameObject;
        Card.name = Data.name;
        Card.GetComponent<CardBehaviour>().Data = Instantiate(Data);
        Card.GetComponent<CardBehaviour>().Data.name = Card.GetComponent<CardBehaviour>().Data.name.Replace("(Clone)", "");
        Card.transform.SetParent(CardsHolder);

        if (Drawn)
        {
            DraggableElement.DragItem = Card;
            HandZone.OnDrop(new PointerEventData(EventSystem.current));
        }

        return Card;
    }

    public void OnConfirm()
    {
        PlayController.Instance.ConfirmAction();
    }

    private static bool IsWarning = false;

    public static void Warn(string Message)
    {
        if (IsWarning)
        {
            Instance.StopAllCoroutines();
        }

        Instance.StartCoroutine(Warning(Message));
    }

    public static IEnumerator Warning(string Message)
    {
        IsWarning = true;

        yield return null;
        Image WarnPanel = Instance.WarningPanelImage;
        Text WarnPanelText = WarnPanel.GetComponentInChildren<Text>();

        WarnPanelText.text = Message;

        WarnPanel.color = new Color(WarnPanel.color.r, WarnPanel.color.g, WarnPanel.color.b, 0);
        WarnPanelText.color = new Color(WarnPanelText.color.r, WarnPanelText.color.g, WarnPanelText.color.b, 0);

        while (WarnPanel.color.a < 1)
        {
            WarnPanel.color = new Color(WarnPanel.color.r, WarnPanel.color.g, WarnPanel.color.b, WarnPanel.color.a + 0.05f);
            WarnPanelText.color = new Color(WarnPanelText.color.r, WarnPanelText.color.g, WarnPanelText.color.b, WarnPanelText.color.a + 0.05f);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        while (WarnPanel.color.a > 0)
        {
            WarnPanel.color = new Color(WarnPanel.color.r, WarnPanel.color.g, WarnPanel.color.b, WarnPanel.color.a - 0.05f);
            WarnPanelText.color = new Color(WarnPanelText.color.r, WarnPanelText.color.g, WarnPanelText.color.b, WarnPanelText.color.a - 0.05f);
            yield return null;
        }

        IsWarning = false;
    }

    public void OnStateChange(PlayState NewState)
    {
        if(NewState != PlayState.DrawBuy)
        {
            if(BlackMarketButton != null)
                BlackMarketButton.SetActive(false);
        }
        else
        {
            if (BlackMarketButton != null)
                BlackMarketButton.SetActive(true);
        }
    }
}
