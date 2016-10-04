using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(CanvasGroup))]
public class CanvasGroupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        CanvasGroup canvas = (CanvasGroup)target;
        canvas.alpha = EditorGUILayout.Slider("Alpha", canvas.alpha, 0f, 1f);
        canvas.interactable = EditorGUILayout.Toggle("Interactable", canvas.interactable);
        canvas.blocksRaycasts = EditorGUILayout.Toggle("Blocks Raycast", canvas.blocksRaycasts);
        canvas.ignoreParentGroups = EditorGUILayout.Toggle("Ignore Parent Groups", canvas.ignoreParentGroups);
        serializedObject.ApplyModifiedProperties();
    }
}