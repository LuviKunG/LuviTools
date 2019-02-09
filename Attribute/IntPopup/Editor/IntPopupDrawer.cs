using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(IntPopupAttribute))]
public class IntPopupDrawer : PropertyDrawer
{
    private IntPopupAttribute intPopup;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        intPopup = attribute as IntPopupAttribute;
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        Rect rect = new Rect(position.x, position.y, position.width, position.height);
        property.intValue = EditorGUI.IntPopup(rect, property.intValue, intPopup.name, intPopup.value);
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}