using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ObstacleData : CardData, IStateListener
{
#if UNITY_EDITOR
    [MenuItem("Assets/Create/ObstacleData")]
    public static void CreateObstacle()
    {
        ScriptableObjectUtility.CreateAsset<ObstacleData>();
    }
#endif
    [NonSerialized]
    private int currentTrack = 0;
    
    private int CurrentTrack
    {
        get { return currentTrack;  }
        set
        {
            if (value != currentTrack)
                Behaviour.GetComponent<ObstacleBehaviour>().MoveMarkerTo(value);
            currentTrack = value;
        }
    }

    [NonSerialized]
    public Player FacingPlayer;

    [NonSerialized]
    public List<CardData> AssignedCards;

    public int MoneyReward;
    public ObstacleType Type;
    public List<Damage> DamageTrack;

    public override void OnCardDroppedOn(CardData Data)
    {
        base.OnCardDroppedOn(Data);
        OnAssign(Data);
    }

    public override void OnAssign(CardData Data)
    {
        if (AssignedCards == null)
            AssignedCards = new List<CardData>();
        
        AssignedCards.Add(Data);
        Data.OnAssign(this);
        
    }

    public override void OnUnassign(CardData Data)
    {
        if(AssignedCards != null && AssignedCards.Count > 0 && AssignedCards.Contains(Data))
        {
            AssignedCards.Remove(Data);
            Data.OnUnassign(this);
        }
    }

    public override void OnDispose()
    {
        base.OnDispose();
        PlayController.Instance.StartCoroutine(OnDefeat(FacingPlayer));
        OnDefeat(FacingPlayer);
    }

    public virtual void OnFlip(Player Facing)
    {
        this.RegisterStateListener();
        FacingPlayer = Facing;
    }

    public virtual void OnAttack(Player Facing)
    {

    }

    public virtual IEnumerator OnDefeat(Player Facing)
    {
        yield return null;
        this.DisposeStateListener();
        PlayController.Instance.ObstacleDefeated(this);
    }

    public void OnStateChange(PlayState NewState)
    {
        if(NewState == PlayState.TakeDamage)
        {
            if (AssignedCards != null && AssignedCards.Count > 0)
            {
                int TotalDmg = 0;
                Dictionary<CardColor, int> AssignedDamage = new Dictionary<CardColor, int>();
                for (int i = 0; i < AssignedCards.Count; i++)
                {
                    List<Damage> CardDamage = AssignedCards[i].GetDamage();
                    for (int j = 0; j < CardDamage.Count; j++)
                    {
                        if (!AssignedDamage.ContainsKey(CardDamage[j].DamageType))
                        {
                            AssignedDamage.Add(CardDamage[j].DamageType, 0);
                        }

                        TotalDmg += CardDamage[j].Value;
                        AssignedDamage[CardDamage[j].DamageType] += CardDamage[j].Value;
                    }
                }

                List<CardData> CachedAssignedCards = new List<CardData>(AssignedCards);

                int ToDeduce = 0;

                while (DamageTrack.Count > CurrentTrack && AssignedDamage.Count > 0 && TotalDmg > ToDeduce)
                {
                    Damage Current = DamageTrack[CurrentTrack];
                    if (Current.DamageType != CardColor.None && (AssignedDamage.ContainsKey(Current.DamageType) || AssignedDamage.ContainsKey(CardColor.All)) )
                    {
                        if (AssignedDamage.ContainsKey(Current.DamageType))
                        {
                            if (AssignedDamage[Current.DamageType] >= Current.Value)
                            {
                                AssignedDamage[Current.DamageType] -= Current.Value;
                                if (AssignedDamage[Current.DamageType] < 1)
                                    AssignedDamage.Remove(Current.DamageType);
                                TotalDmg -= Current.Value;
                                CurrentTrack++;
                            }
                            else
                                break;
                        }
                        else
                        {
                            AssignedDamage[CardColor.All] -= Current.Value;
                            if (AssignedDamage[CardColor.All] < 1)
                                AssignedDamage.Remove(CardColor.All);
                            TotalDmg -= Current.Value;
                            CurrentTrack++;
                        }

                    }
                    else if (Current.DamageType == CardColor.None && Current.Value <= TotalDmg)
                    {
                        if (AssignedDamage.ContainsKey(CardColor.None))
                        {
                            if(AssignedDamage[CardColor.None] >= Current.Value)
                            {
                                AssignedDamage[CardColor.None] -= Current.Value;
                                if(AssignedDamage[CardColor.None] == 0)
                                    AssignedDamage.Remove(CardColor.None);
                                TotalDmg -= Current.Value;
                            }
                            else if(AssignedDamage[CardColor.None] < Current.Value)
                            {
                                ToDeduce += Current.Value - AssignedDamage[CardColor.None];
                                TotalDmg -= AssignedDamage[CardColor.None];
                                AssignedDamage.Remove(CardColor.None);
                            }
                            if (TotalDmg - ToDeduce > 0)
                                CurrentTrack++;
                        }
                        else
                        {
                            ToDeduce += Current.Value;
                            CurrentTrack++;
                        }
                    }
                    else
                        break;
                }
                CachedAssignedCards.ForEach(x => x.OnDispose());
            }
            if (CurrentTrack >= DamageTrack.Count)
            {
                PlayController.Instance.CurrentPlayer.Money += MoneyReward;
                OnDispose();
            }
            else
            {
                FacingPlayer.HP -= Damage;
                OnAttack(FacingPlayer);
            }
        }
    }
}

public enum ObstacleType
{
    Troll,
    Ork,
    Human,
    Elf,
    Dwarf,
    Tech,
    World
}
