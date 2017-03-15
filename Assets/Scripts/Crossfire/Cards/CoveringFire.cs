using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CoveringFire : CardData
{
#if UNITY_EDITOR
    [MenuItem("Assets/Create/Card/CoveringFire")]
    public static void CreateCoveringFire()
    {
        ScriptableObjectUtility.CreateAsset<CoveringFire>();
    }
#endif

    public override void OnPlayed()
    {
        base.OnPlayed();

        Behaviour.GetComponent<DraggableElement>().enabled = false;
        PlayController.Instance.CurrentPlayer.HP++;
    }
}
