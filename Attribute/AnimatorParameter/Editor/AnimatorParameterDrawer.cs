using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LuviKunG.Attributes
{
    [CustomPropertyDrawer(typeof(AnimatorParameterAttribute))]
    public class AnimatorParameterDrawer : PropertyDrawer
    {
        private const string TYPE_NOT_SUPPORT = "Type not support.";
        private const string NO_SERIALIZABLE_FOUND = "No serializable found.";
        private const string NO_ANIMATOR_REFERENCE = "No Animator reference.";

        private int currentIndex = -1;
        private string[] parametersName;
        private int[] parametersHash;

        private AnimatorParameterAttribute animatorHash;

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
                if (property.type == "int")
                {
                    animatorHash = animatorHash ?? attribute as AnimatorParameterAttribute;
                    var serializedObject = property.serializedObject.FindProperty(animatorHash.animatorPropertyName);
                    if (serializedObject == null)
                    {
                        EditorGUI.HelpBox(rect, NO_SERIALIZABLE_FOUND, MessageType.Error);
                    }
                    else if (serializedObject.objectReferenceValue == null)
                    {
                        EditorGUI.HelpBox(rect, NO_ANIMATOR_REFERENCE, MessageType.Info);
                    }
                    else
                    {
                        Animator animator = serializedObject.objectReferenceValue as Animator;
                        if (animator == null)
                        {
                            EditorGUI.HelpBox(rect, TYPE_NOT_SUPPORT, MessageType.Error);
                        }
                        else
                        {
                            GetAllAnimatorControllerParameters(ref animator, out parametersName, out parametersHash);
                            currentIndex = GetCurrentIndex(ref parametersHash, property.intValue);
                            using (var scopeChange = new EditorGUI.ChangeCheckScope())
                            {
                                if (currentIndex < 0)
                                    GUI.color = Color.red;
                                currentIndex = EditorGUI.Popup(rect, currentIndex, parametersName);
                                GUI.color = cacheColor;
                                if (scopeChange.changed)
                                {
                                    property.intValue = parametersHash[currentIndex];
                                    GetAllAnimatorControllerParameters(ref animator, out parametersName, out parametersHash);
                                }
                            }
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

        private void GetAllAnimatorControllerParameters(ref Animator animator, out string[] name, out int[] hash)
        {
            name = new string[animator.parameterCount];
            hash = new int[animator.parameterCount];
            for (int i = 0; i < animator.parameterCount; i++)
            {
                var controlParameter = animator.GetParameter(i);
                name[i] = $"{controlParameter.name} ({controlParameter.type})";
                hash[i] = controlParameter.nameHash;
            }
        }

        private int GetCurrentIndex(ref int[] list, int hash)
        {
            for (int i = 0; i < list.Length; i++)
                if (list[i] == hash)
                    return i;
            return -1;
        }
    }
}