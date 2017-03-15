using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BlackMarketContacts : CardData
{
#if UNITY_EDITOR
    [MenuItem("Assets/Create/Card/BlackMarketContacts")]
    public static void CreateBlackMarketContacts()
    {
        ScriptableObjectUtility.CreateAsset<BlackMarketContacts>();
    }
#endif

    public override void OnPlayed()
    {
        base.OnPlayed();

        Behaviour.GetComponent<DraggableElement>().enabled = false;
        UIController.Instance.BlackMarketButton.SetActive(true);
    }

}
