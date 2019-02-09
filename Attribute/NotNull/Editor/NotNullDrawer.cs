using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(NotNullAttribute), true)]
public class NotNullDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Object target = property.serializedObject.targetObject;
        bool isComponent = target != null && target is Component;
        if (isComponent && IsNull(property))
        {
            Color cache = GUI.color;
            GUI.color = Color.red;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.color = cache;
        }
        else
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    private bool IsNull(SerializedProperty property)
    {
        return property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue == null;
    }
}