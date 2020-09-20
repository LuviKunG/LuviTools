using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LuviKunG.Attributes
{
    [CustomPropertyDrawer(typeof(AnimatorNameAttribute))]
    public class AnimatorNameDrawer : PropertyDrawer
    {
        private const string TYPE_NOT_SUPPORT = "Type not support.";
        private const string NO_SERIALIZABLE_FOUND = "No serializable found.";
        private const string NO_ANIMATOR_REFERENCE = "No Animator reference.";

        private int currentIndex = -1;
        private string[] parametersName;
        private int[] parametersHash;

        private AnimatorNameAttribute animatorHash;

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
                    animatorHash = animatorHash ?? attribute as AnimatorNameAttribute;
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
                            GetAllAnimatorStateHashes(ref animator, out parametersName, out parametersHash);
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
                                    GetAllAnimatorStateHashes(ref animator, out parametersName, out parametersHash);
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

        private void GetAllAnimatorStateHashes(ref Animator animator, out string[] stateNames, out int[] stateHashes)
        {
            var stateNamesList = new List<string>();
            var stateHashesList = new List<int>();
            var animatorController = animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
            if (animatorController != null)
            {
                foreach (var layer in animatorController.layers)
                {
                    var stateMachine = layer.stateMachine;
                    foreach (var childState in stateMachine.states)
                    {
                        var state = childState.state;
                        stateNamesList.Add($"{layer.name}:{state.name}");
                        stateHashesList.Add(state.nameHash);
                    }
                }
            }
            stateNames = stateNamesList.ToArray();
            stateHashes = stateHashesList.ToArray();
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