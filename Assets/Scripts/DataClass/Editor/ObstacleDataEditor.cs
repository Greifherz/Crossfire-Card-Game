using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObstacleData))]
public class ObstacleDataEditor : Editor
{
    private Damage ToAdd;
    private bool Change = false;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        

        ObstacleData Data = (ObstacleData)target;

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("New Track");
        ToAdd.DamageType = (CardColor)EditorGUILayout.EnumPopup(ToAdd.DamageType, GUILayout.Width(100));
        if (ToAdd.DamageType == CardColor.None)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Value ",GUILayout.Width(45));
            ToAdd.Value = EditorGUILayout.IntField(ToAdd.Value, GUILayout.Width(50));
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            ToAdd.Value = 1;
        }

        if (GUILayout.Button("Add", GUILayout.Width(100)))
        {
            if (Data.DamageTrack == null)
                Data.DamageTrack = new List<Damage>();
            Data.DamageTrack.Add(ToAdd);
            ToAdd = new Damage();
            Change = true;
        }

        if (Change)
        {
            Change = false;
            EditorUtility.SetDirty(target);
        }

    }
}
