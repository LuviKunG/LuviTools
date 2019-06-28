using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StringSceneAttribute))]
public class StringSceneDrawer : PropertyDrawer
{
    private const string TYPE_NOT_SUPPORT = "Type not support.";

    private StringSceneAttribute stringScene;
    private string[] scenesPath;
    private string[] scenesName;
    private int currentIndex;

    public override bool CanCacheInspectorGUI(SerializedProperty property)
    {
        if (property.type == "string")
        {
            stringScene = attribute as StringSceneAttribute;
            GetScenes(stringScene, out List<string> paths, out List<string> names);
            scenesPath = paths.ToArray();
            scenesName = names.ToArray();
            currentIndex = GetCurrentIndex(property);
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
                    currentIndex = EditorGUI.Popup(rect, currentIndex, scenesName);
                    GUI.color = cacheColor;
                    if (scopeChange.changed)
                    {
                        property.stringValue = scenesPath[currentIndex];
                        GetScenes(stringScene, out List<string> paths, out List<string> names);
                        scenesPath = paths.ToArray();
                        scenesName = names.ToArray();
                    }
                }
            }
            else
            {
                EditorGUI.HelpBox(rect, TYPE_NOT_SUPPORT, MessageType.Error);
            }
            EditorGUI.indentLevel = indent;
        }
    }

    private int GetCurrentIndex(SerializedProperty property)
    {
        for (int i = 0; i < scenesPath.Length; i++)
            if (scenesPath[i] == property.stringValue)
                return i;
        return -1;
    }

    private void GetScenes(StringSceneAttribute attribute, out List<string> path, out List<string> name)
    {
        var scenes = EditorBuildSettings.scenes;
        path = new List<string>();
        name = new List<string>();
        for (int i = 0; i < scenes.Length; i++)
        {
            if (attribute.excludeDisableScene && !scenes[i].enabled) continue;
            path.Add(scenes[i].path);
            name.Add(Path.GetFileNameWithoutExtension(scenes[i].path));
        }
    }
}