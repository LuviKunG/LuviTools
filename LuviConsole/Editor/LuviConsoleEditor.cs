using System.Text;
using UnityEditor;
using UnityEngine;

namespace LuviKunG
{
    [CustomEditor(typeof(LuviConsole))]
    [CanEditMultipleObjects]
    public class LuviConsoleEditor : Editor
    {
        private readonly GUIContent contentLogCapacity = new GUIContent("Log Capacity", "The capacity of list that will show debug log on console window.");
        private readonly GUIContent contentExecuteCapacity = new GUIContent("Log Excuted Command Capacity", "The capacity of excute command that have been excuted.");
        private readonly GUIContent contentSwipeRatio = new GUIContent("Swipe Ratio", "The ratio when you swipe up/down on your screen to toggle LuviDebug. 0 when touched, 1 when swipe entire of");
        private readonly GUIContent contentDefaultFontSize = new GUIContent("Default Font Size", "Default font size of console when it getting start.");
        private readonly GUIContent contentAutoShowWarning = new GUIContent("Show Log When Warning", "Automatically show logs when player is get warning log.");
        private readonly GUIContent contentAutoShowError = new GUIContent("Show Log When Error", "Automatically show logs when player is get error log.");
        private readonly GUIContent contentAutoShowException = new GUIContent("Show Log When Exception", "Automatically show logs when player is get exception log.");
        private readonly GUIContent contentCommandLog = new GUIContent("Log Command", "Log the command after you executed.");

        private LuviConsole console;
        private StringBuilder sb;
        private SerializedProperty logCapacity;
        private SerializedProperty excuteCapacity;
        private SerializedProperty swipeRatio;
        private SerializedProperty defaultFontSize;
        private SerializedProperty autoShowWarning;
        private SerializedProperty autoShowError;
        private SerializedProperty autoShowException;
        private SerializedProperty commandLog;

        public void OnEnable()
        {
            console = (LuviConsole)target;
            logCapacity = serializedObject.FindProperty(nameof(console.logCapacity));
            excuteCapacity = serializedObject.FindProperty(nameof(console.excuteCapacity));
            swipeRatio = serializedObject.FindProperty(nameof(console.swipeRatio));
            defaultFontSize = serializedObject.FindProperty(nameof(console.defaultFontSize));
            autoShowWarning = serializedObject.FindProperty(nameof(console.autoShowWarning));
            autoShowError = serializedObject.FindProperty(nameof(console.autoShowError));
            autoShowException = serializedObject.FindProperty(nameof(console.autoShowException));
            commandLog = serializedObject.FindProperty(nameof(console.commandLog));

            sb = sb ?? new StringBuilder();
            sb.Clear();
            sb.Append("Luvi Console Version 2.4.2");
            sb.AppendLine();
#if UNITY_EDITOR
            sb.Append("In editor, Press F1 for toggle LuviDebug.");
#elif UNITY_EDITOR_OSX
            sb.Append("In editor, Press Tap for toggle LuviDebug.");
#endif
#if UNITY_ANDROID
            sb.AppendLine();
            sb.Append("In your Android device, Swipe up direction in your screen for open LuviDebug. And swipe down for close LuviDebug.");
#elif UNITY_IOS
            sb.AppendLine();
            sb.Append("In your iPhone/iPad/iPod device, Swipe up direction in your screen for open LuviDebug. And swipe down for close LuviDebug.");
#elif UNITY_WEBGL
            sb.AppendLine();
            sb.Append("In your WebGL builds, Press Tap for toggle LuviDebug.");
#endif
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            using (var checkScope = new EditorGUI.ChangeCheckScope())
            {
                logCapacity.intValue = EditorGUILayout.IntField(contentLogCapacity, logCapacity.intValue);
                excuteCapacity.intValue = EditorGUILayout.IntField(contentExecuteCapacity, excuteCapacity.intValue);
                swipeRatio.floatValue = EditorGUILayout.Slider(contentSwipeRatio, swipeRatio.floatValue, 0f, 1f);
                defaultFontSize.intValue = EditorGUILayout.IntSlider(contentDefaultFontSize, defaultFontSize.intValue, 8, 64);
                autoShowWarning.boolValue = EditorGUILayout.Toggle(contentAutoShowWarning, autoShowWarning.boolValue);
                autoShowError.boolValue = EditorGUILayout.Toggle(contentAutoShowError, autoShowError.boolValue);
                autoShowException.boolValue = EditorGUILayout.Toggle(contentAutoShowException, autoShowException.boolValue);
                commandLog.boolValue = EditorGUILayout.Toggle(contentCommandLog, commandLog.boolValue);
                if (checkScope.changed)
                    EditorUtility.SetDirty(console);
            }
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.HelpBox(sb.ToString(), MessageType.Info);
        }
    }
}