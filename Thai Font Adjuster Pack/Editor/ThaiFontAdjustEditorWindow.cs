using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ThaiFontAdjustEditorWindow : EditorWindow
{
    private string message = "";
    private string thaiMessage = "";

    [MenuItem("Window/Thai Font Adjuster Unicode Converter")]
    public static ThaiFontAdjustEditorWindow OpenWindow()
    {
        ThaiFontAdjustEditorWindow window = GetWindow<ThaiFontAdjustEditorWindow>(true, "Thai Font Adjuster Unicode Converter", true);
        window.Show();
        return window;
    }

    public void OnGUI()
    {
        EditorGUILayout.LabelField(new GUIContent("Insert Thai message here."));
        EditorGUI.BeginChangeCheck();
        message = EditorGUILayout.TextArea(message);
        if (EditorGUI.EndChangeCheck())
        {
            if (!string.IsNullOrEmpty(message))
            {
                thaiMessage = ThaiFontAdjuster.Adjust(message);
            }
            else
            {
                thaiMessage = "";
            }
        }
        EditorGUILayout.LabelField(new GUIContent("Output."));
        EditorGUILayout.TextArea(thaiMessage);
        if (GUILayout.Button("Copy"))
        {
            UnityEngine.TextEditor t = new UnityEngine.TextEditor();
            t.text = thaiMessage;
            t.SelectAll();
            t.Copy();
        }
    }
}
