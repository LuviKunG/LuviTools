using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DontDestroyOnLoad))]
public class DontDestroyOnLoadEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("This object will not destroy by loading new scene.", MessageType.Info);
    }
}