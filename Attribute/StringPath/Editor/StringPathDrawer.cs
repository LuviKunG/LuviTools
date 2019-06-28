using System.IO;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StringPathAttribute))]
public class StringPathDrawer : PropertyDrawer
{
    private const float BUTTON_SIZE = 56;
    private const string TYPE_NOT_SUPPORT = "Type not support.";
    private static readonly GUIContent BUTTON_LABEL_SELECT = new GUIContent("Select");
    private static readonly GUIContent BUTTON_LABEL_CHANGE = new GUIContent("Change");
    private static readonly GUIContent LABEL_NULL = new GUIContent("No path specified.");

    private StringPathAttribute stringPath;

    public override bool CanCacheInspectorGUI(SerializedProperty property)
    {
        if (property.type == "string")
        {
            stringPath = attribute as StringPathAttribute;
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
            var rectLabel = new Rect(position.x, position.y, position.width - BUTTON_SIZE, position.height);
            var rectButton = new Rect(position.x + position.width - BUTTON_SIZE, position.y, BUTTON_SIZE, position.height);
            if (property.type == "string")
            {
                bool isEmpty = string.IsNullOrEmpty(property.stringValue);
                if (isEmpty)
                    EditorGUI.LabelField(rectLabel, LABEL_NULL);
                else
                    EditorGUI.LabelField(rectLabel, new GUIContent(property.stringValue));
                if (GUI.Button(rectButton, isEmpty ? BUTTON_LABEL_SELECT : BUTTON_LABEL_CHANGE))
                {
                    string path = GetPath(property.stringValue, stringPath.extension);
                    if (!string.IsNullOrEmpty(path))
                        property.stringValue = path;
                }
            }
            else
            {
                EditorGUI.HelpBox(rect, TYPE_NOT_SUPPORT, MessageType.Error);
            }
            EditorGUI.indentLevel = indent;
        }
    }

    private string GetPath(string oldPath, string extension)
    {
        string getFileName = null;
        try { getFileName = Path.GetFileName(oldPath); } catch { }
        string path = EditorUtility.SaveFilePanel("Select path to save", null, getFileName ?? "", extension);
        return path;
    }
}