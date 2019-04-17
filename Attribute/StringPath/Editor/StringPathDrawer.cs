using System.IO;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StringPathAttribute))]
public class StringPathDrawer : PropertyDrawer
{
    private readonly float buttonSize = 24;
    private readonly GUIContent labelButton = new GUIContent("?");
    private StringPathAttribute stringPathAttribute;
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) { return EditorGUI.GetPropertyHeight(property, label, true); }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        stringPathAttribute = attribute as StringPathAttribute;
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        Rect rectLabel = new Rect(position.x, position.y, position.width - buttonSize, position.height);
        Rect rectButton = new Rect(position.x + position.width - buttonSize, position.y, buttonSize, position.height);
        EditorGUI.LabelField(rectLabel, new GUIContent(property.stringValue));
        if (GUI.Button(rectButton, labelButton))
        {
            string path = GetPath(property.stringValue, stringPathAttribute.extension);
            if (!string.IsNullOrEmpty(path))
            {
                property.stringValue = path;
                GUI.changed = true;
            }
        }
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
    private string GetPath(string oldPath, string extension)
    {
        string getFileName = null;
        try { getFileName = Path.GetFileName(oldPath); } catch { }
        string path = EditorUtility.SaveFilePanel("Select path to save", null, getFileName ?? "", extension);
        return path;
    }
}