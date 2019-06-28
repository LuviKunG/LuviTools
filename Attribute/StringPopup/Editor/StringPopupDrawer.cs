using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StringPopupAttribute))]
public class StringPopupDrawer : PropertyDrawer
{
    private const string TYPE_NOT_SUPPORT = "Type not support.";

    private StringPopupAttribute stringPopup;
    private int currentIndex;

    public override bool CanCacheInspectorGUI(SerializedProperty property)
    {
        if (property.type == "string")
        {
            stringPopup = attribute as StringPopupAttribute;
            currentIndex = GetCurrentIndex(stringPopup, property);
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
            Color cacheColor = GUI.color;
            var indent = EditorGUI.indentLevel;
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            EditorGUI.indentLevel = 0;
            var rect = new Rect(position.x, position.y, position.width, position.height);
            if (property.type == "string")
            {
                using (var scopeChange = new EditorGUI.ChangeCheckScope())
                {
                    if (currentIndex < 0)
                        GUI.color = Color.red;
                    currentIndex = EditorGUI.Popup(rect, currentIndex, stringPopup.name);
                    GUI.color = cacheColor;
                    if (scopeChange.changed)
                        property.stringValue = stringPopup.value[currentIndex];
                }
            }
            else
            {
                EditorGUI.HelpBox(rect, TYPE_NOT_SUPPORT, MessageType.Error);
            }
            EditorGUI.indentLevel = indent;
        }
    }

    private int GetCurrentIndex(StringPopupAttribute attribute, SerializedProperty property)
    {
        for (int i = 0; i < attribute.value.Length; i++)
            if (attribute.value[i] == property.stringValue)
                return i;
        return -1;
    }
}