using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsAttributeDrawer : PropertyDrawer
{
    private EnumFlagsAttribute flags;
    public override bool CanCacheInspectorGUI(SerializedProperty property)
    {
        flags = attribute as EnumFlagsAttribute;
        return base.CanCacheInspectorGUI(property);
    }
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        //_property.intValue = EditorGUI.MaskField(_position, _label, _property.intValue, _property.enumNames);
        Enum e = EditorGUI.EnumFlagsField(_position, _label, (Enum)Enum.ToObject(flags.enumFlagType, _property.intValue));
        _property.intValue = (int)Convert.ChangeType(e, e.GetTypeCode());
    }
}