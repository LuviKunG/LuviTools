using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StringPopupAttribute))]
public class StringPopupDrawer : PropertyDrawer
{
    private StringPopupAttribute stringPopup;
    private int index;
    private bool flagStart = false;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        stringPopup = attribute as StringPopupAttribute;
        if (!flagStart)
        {
            flagStart = true;
            CheckIndex(property);
        }
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        Rect rect = new Rect(position.x, position.y, position.width, position.height);
        EditorGUI.BeginChangeCheck();
        index = EditorGUI.Popup(rect, index, stringPopup.name);
        property.stringValue = stringPopup.value[index];
        if (EditorGUI.EndChangeCheck())
        {
            CheckIndex(property);
        }
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    private void CheckIndex(SerializedProperty property)
    {
        for (int i = 0; i < stringPopup.value.Length; i++)
        {
            if (stringPopup.value[i] == property.stringValue)
            {
                index = i;
                break;
            }
        }
    }
}