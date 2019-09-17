using UnityEditor;
using UnityEngine;

namespace LuviKunG.UI
{
    [CustomEditor(typeof(TouchableImageMask))]
    public class TouchableImageMaskEditor : Editor
    {
        private static readonly GUIContent CONTENT_RAYCAST_TARGET = new GUIContent("Raycast Target");
        private static readonly GUIContent CONTENT_IMAGE = new GUIContent("Image");
        private static readonly GUIContent CONTENT_ALPHA_THRESHOLD = new GUIContent("Alpha Threshold");
        private TouchableImageMask touchableImageMask = default;

        private SerializedProperty raycastTarget;
        private SerializedProperty image;
        private SerializedProperty alphaThreshold;

        private void OnEnable()
        {
            touchableImageMask = target as TouchableImageMask;
            raycastTarget = serializedObject.FindProperty("m_RaycastTarget");
            image = serializedObject.FindProperty("image");
            alphaThreshold = serializedObject.FindProperty("alphaThreshold");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.HelpBox("This UI will be touchable.", MessageType.Info);
            using (var checkScope = new EditorGUI.ChangeCheckScope())
            {
                raycastTarget.boolValue = EditorGUILayout.Toggle(CONTENT_RAYCAST_TARGET, raycastTarget.boolValue);
                image.objectReferenceValue = EditorGUILayout.ObjectField(CONTENT_IMAGE, image.objectReferenceValue, typeof(UnityEngine.UI.Image), true);
                alphaThreshold.floatValue = EditorGUILayout.Slider(CONTENT_ALPHA_THRESHOLD, alphaThreshold.floatValue, 0.0f, 1.0f);
                if (checkScope.changed)
                    EditorUtility.SetDirty(touchableImageMask);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}