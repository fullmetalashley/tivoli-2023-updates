using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PnPTextReader))]
public class PnPTextEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PnPTextReader reader = (PnPTextReader)target;

        GUILayout.BeginHorizontal();
            if (GUILayout.Button("Display Text"))
            {
                reader.DisplayFile();
            }
            if (GUILayout.Button("Break Text"))
            {
                reader.BreakText();
            }
        GUILayout.EndHorizontal();
    }
}
