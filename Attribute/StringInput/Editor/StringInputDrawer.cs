using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LuviKunG.Editor
{
    [CustomPropertyDrawer(typeof(StringInputAttribute))]
    public class StringInputDrawer : PropertyDrawer
    {
        private const string TYPE_NOT_SUPPORT = "Type not support.";

        private string[] axesName;
        private int currentIndex;

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
                    var names = new List<string>();
                    var inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset");
                    SerializedObject obj = new SerializedObject(inputManager);
                    SerializedProperty axes = obj.FindProperty("m_Axes");
                    if (axes.arraySize == 0)
                        Debug.Log("No Axes");
                    for (int i = 0; i < axes.arraySize; ++i)
                    {
                        var axis = axes.GetArrayElementAtIndex(i);
                        var name = axis.FindPropertyRelative("m_Name").stringValue;
                        names.Add(name);
                    }
                    axesName = names.ToArray();
                    currentIndex = GetCurrentIndex(property);
                    using (var scopeChange = new EditorGUI.ChangeCheckScope())
                    {
                        if (currentIndex < 0)
                            GUI.color = Color.red;
                        currentIndex = EditorGUI.Popup(rect, currentIndex, axesName);
                        GUI.color = cacheColor;
                        if (scopeChange.changed)
                        {
                            property.stringValue = axesName[currentIndex];
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
            for (int i = 0; i < axesName.Length; i++)
                if (axesName[i] == property.stringValue)
                    return i;
            return -1;
        }
    }
}