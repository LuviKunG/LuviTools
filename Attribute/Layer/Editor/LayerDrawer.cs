using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LayerAttribute))]
public class LayerDrawer : PropertyDrawer
{
    private const string TYPE_NOT_SUPPORT = "Type not support.";

    public override bool CanCacheInspectorGUI(SerializedProperty property)
    {
        return property.type == "int";
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        if (property.type == "int")
            property.intValue = EditorGUI.LayerField(position, property.intValue);
        else
            EditorGUI.HelpBox(position, TYPE_NOT_SUPPORT, MessageType.Error);
        EditorGUI.indentLevel = indent;
    }
}
