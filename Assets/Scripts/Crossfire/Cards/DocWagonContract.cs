using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DocWagonContract : CardData
{
#if UNITY_EDITOR
    [MenuItem("Assets/Create/Card/DocWagonContract")]
    public static void CreateDocWagonContract()
    {
        ScriptableObjectUtility.CreateAsset<DocWagonContract>();
    }
#endif

    public override void OnPlayed()
    {
        base.OnPlayed();

        Behaviour.GetComponent<DraggableElement>().enabled = false;
        PlayController.Instance.CurrentPlayer.HP += 2;
    }
}
