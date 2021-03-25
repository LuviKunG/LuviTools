using UnityEditor;
using UnityEngine;

namespace LuviKunG
{
    [CustomEditor(typeof(CameraAspectRatio))]
    public class CameraAspectRatioEditor : Editor
    {
        private const string HELPBOX_CAMERA = "Please add the target camera first.";
        private static readonly GUIContent CONTENT_TARGET_CAMERA = new GUIContent("Target Camera");
        private static readonly GUIContent CONTENT_RUN_ONLY_ONCE = new GUIContent("Run Only Once");
        private static readonly GUIContent CONTENT_MODE = new GUIContent("Mode");
        private static readonly GUIContent CONTENT_SIZE = new GUIContent("Size");
        private static readonly GUIContent CONTENT_ZOOM = new GUIContent("Zoom");

        private CameraAspectRatio cameraAspect;

        private SerializedProperty targetCamera;
        private SerializedProperty runOnlyOnce;
        private SerializedProperty mode;
        private SerializedProperty size;
        private SerializedProperty zoomScale;

        private void OnEnable()
        {
            cameraAspect = target as CameraAspectRatio;
            targetCamera = serializedObject.FindProperty("targetCamera");
            runOnlyOnce = serializedObject.FindProperty("runOnlyOnce");
            mode = serializedObject.FindProperty("mode");
            size = serializedObject.FindProperty("size");
            zoomScale = serializedObject.FindProperty("m_zoomScale");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(targetCamera, CONTENT_TARGET_CAMERA);
            if (cameraAspect.targetCamera == null)
            {
                EditorGUILayout.HelpBox(HELPBOX_CAMERA, MessageType.Error, true);
            }
            else
            {
                EditorGUILayout.PropertyField(runOnlyOnce, CONTENT_RUN_ONLY_ONCE);
                EditorGUILayout.PropertyField(mode, CONTENT_MODE);
                if (cameraAspect.mode == CameraAspectRatio.Mode.ByHeight ||
                    cameraAspect.mode == CameraAspectRatio.Mode.ByWidth)
                {
                    float sizeWidth = EditorGUILayout.FloatField(CONTENT_SIZE, size.vector2Value.x);
                    size.vector2Value = new Vector2(sizeWidth, size.vector2Value.y);
                }
                else
                {
                    EditorGUILayout.PropertyField(size, CONTENT_SIZE);
                }
                if (EditorGUILayout.PropertyField(zoomScale, CONTENT_ZOOM))
                    cameraAspect.zoomScale = zoomScale.floatValue;
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            if (cameraAspect.targetCamera == null || !cameraAspect.enabled)
                return;
            cameraAspect?.UpdateCameraAspect();
        }
    }
}
