using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class CardData : ScriptableObject
{
#if UNITY_EDITOR
    [MenuItem("Assets/Create/CardData")]
    public static void CreateCard()
    {
        ScriptableObjectUtility.CreateAsset<CardData>();
    }
#endif
    [HideInInspector]
    public CardBehaviour Behaviour;

    public string Name;
    public int Damage;
    public int SpecifiedDamage;
    public int MoneyCost;

    public Sprite CardSprite;

    public CardCategory Category = CardCategory.Action;
    public CardColor Color;
    public CardData AssignedTo;
    
    public virtual List<Damage> GetDamage()
    {
        List<Damage> Damages = new List<Damage>();
        if (Damage > 0)
        {
            Damage CommonDamage = new Damage();
            CommonDamage.DamageType = CardColor.None;
            CommonDamage.Value = Damage;
            Damages.Add(CommonDamage);
        }
        if (SpecifiedDamage > 0)
        {
            Damage CardSpecifiedDamage = new Damage();
            CardSpecifiedDamage.DamageType = Color;
            CardSpecifiedDamage.Value = SpecifiedDamage;
            Damages.Add(CardSpecifiedDamage);
        }
        return Damages;
    }

    public virtual void OnPlayed()
    {

    }

    public virtual void OnCardDroppedOn(CardData Data)
    {

    }

    public virtual void OnAssign(CardData Data)
    {
        if (AssignedTo == null)
        {
            AssignedTo = Data;
        }
    }

    public virtual void OnUnassign(CardData Data)
    {
        if(AssignedTo != null && AssignedTo.Equals(Data))
        {
            AssignedTo = null;
        }
    }

    public virtual void OnDispose()
    {
        if (Behaviour != null)
            Behaviour.Dispose();
        if (AssignedTo != null)
        {
            ObstacleData AssignedObstacle = (ObstacleData)AssignedTo;
            if (AssignedObstacle != null)
            {
                AssignedObstacle.AssignedCards.Remove(this);
            }
        }
    }

}

public enum CardCategory //Tipo
{
    Action,
    Obstacle,
    Event,
    PlaceHolder
}

public enum CardColor //Naipe
{
    Blue,
    Red,
    Green,
    Black,
    All,
    None
}
