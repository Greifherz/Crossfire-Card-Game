using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;


public class CheatWindow : EditorWindow
{
    [MenuItem("GameSettings/CheatSettings")]
    public static void ShowWindow()
    {
        GetWindow(typeof(CheatWindow));
    }

    void OnGUI()
    {
        Editor Edit = Editor.CreateEditor(Resources.Load<CheatSettings>("Tools/CheatSettings"));
        Edit.OnInspectorGUI();
    }

}

