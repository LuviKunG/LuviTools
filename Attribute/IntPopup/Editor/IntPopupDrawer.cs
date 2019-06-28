using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(IntPopupAttribute))]
public class IntPopupDrawer : PropertyDrawer
{
    private const string TYPE_NOT_SUPPORT = "Type not support.";

    private IntPopupAttribute intPopup;

    public override bool CanCacheInspectorGUI(SerializedProperty property)
    {
        if (property.type == "int")
        {
            intPopup = attribute as IntPopupAttribute;
            return true;
        }
        else return false;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        using (var scopeProperty = new EditorGUI.PropertyScope(position, label, property))
        {
            var indent = EditorGUI.indentLevel;
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            EditorGUI.indentLevel = 0;
            var rect = new Rect(position.x, position.y, position.width, position.height);
            if (property.type == "int")
            {
                property.intValue = EditorGUI.IntPopup(rect, property.intValue, intPopup.name, intPopup.value);
            }
            else
            {
                EditorGUI.HelpBox(rect, TYPE_NOT_SUPPORT, MessageType.Error);
            }
            EditorGUI.indentLevel = indent;
        }
    }
}