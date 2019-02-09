using UnityEngine;
using UnityEditor;
using LuviKunG.Update;

[CustomEditor(typeof(LuviUpdate))]
public class LuviUpdateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Don't forget this. Please change Script Execution Order of LuviUpdate to very first.", MessageType.Warning);
    }
}