using UnityEditor;
using UnityEngine;

namespace LuviKunG.UI
{
    [CustomEditor(typeof(Touchable))]
    public class TouchableEditor : Editor
    {
        private static readonly GUIContent CONTENT_RAYCAST_TARGET = new GUIContent("Raycast Target");
        private Touchable touchable = default;
        private SerializedProperty raycastTarget;

        private void OnEnable()
        {
            touchable = target as Touchable;
            raycastTarget = serializedObject.FindProperty("m_RaycastTarget");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.HelpBox("This UI will be touchable.", MessageType.Info);
            using (var checkScope = new EditorGUI.ChangeCheckScope())
            {
                raycastTarget.boolValue = EditorGUILayout.Toggle(CONTENT_RAYCAST_TARGET, raycastTarget.boolValue);
                if (checkScope.changed)
                    EditorUtility.SetDirty(touchable);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}