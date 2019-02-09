using UnityEditor;
using UnityEngine;

namespace LuviKunG.Math
{
    [CustomPropertyDrawer(typeof(DeviationFloat))]
    public class DeviationFloatPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            Rect amountRect = new Rect(position.x, position.y, (position.width / 2.0f) - 2.0f, position.height);
            Rect nameRect = new Rect(position.x + (position.width / 2.0f) + 2.0f, position.y, position.width / 2.0f, position.height);
            EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("value"), GUIContent.none);
            EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("deviation"), GUIContent.none);
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}