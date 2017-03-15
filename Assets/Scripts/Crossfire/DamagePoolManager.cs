using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class DamagePoolManager : MonoBehaviour, IPlayZoneListener
{
    public static DamagePoolManager Instance;

    public List<Damage> DamagePool;

    private Dictionary<string,Text> SpecifiedDamageMarkers;

    
    void Start()
    {
        Instance = this;
        this.RegisterPlayZoneListener();
        Init();
    }

    private void Init()
    {
        foreach (Transform tr in transform)
        {
            string ParentName = tr.name;
            foreach (Transform TrChild in tr)
            {
                if (TrChild.GetComponent<Text>())
                {
                    if (SpecifiedDamageMarkers == null)
                        SpecifiedDamageMarkers = new Dictionary<string, Text>();
                    SpecifiedDamageMarkers.Add(ParentName, TrChild.GetComponent<Text>());
                }
            }
        }

        foreach (var Item in SpecifiedDamageMarkers)
        {
            Item.Value.text = "0";
        }
    }

    public void OnPlayZoneDropped(ICard CardPlayed)
    {
        if (DamagePool == null)
            DamagePool = CardPlayed.GetDamage();
        else
        {
            List<Damage> CardDamage = CardPlayed.GetDamage();
            DamagePool.AddRange(CardDamage);
        }
        UpdateDamagePool();
    }

    public void OnPlayZoneRemoved(ICard CardPlayed)
    {
        if (DamagePool == null)
            return;
        else
        {
            List<Damage> CardDamage = CardPlayed.GetDamage();
            for(int i = 0; i < CardDamage.Count; i ++)
            {
                DamagePool.Remove(CardDamage[i]);
            }
        }
        UpdateDamagePool();
    }

    private void UpdateDamagePool()
    {
        foreach(var Item in SpecifiedDamageMarkers)
        {
            Item.Value.text = "0";
        }

        for(int i = 0; i < DamagePool.Count; i++)
        {
            Damage CurrentDamage = DamagePool[i];
            Text ToUpdate = SpecifiedDamageMarkers[CurrentDamage.DamageType.ToString()];
            ToUpdate.text = (int.Parse(ToUpdate.text) + CurrentDamage.Value).ToString();
        }
    }

}

