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

        public void OnEnable()
        {
            console = (LuviConsole)target;
            sb = sb ?? new StringBuilder();
            sb.Clear();
            sb.Append("Luvi Console Version 2.4.1");
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
                console.logCapacity = EditorGUILayout.IntField(contentLogCapacity, console.logCapacity);
                console.excuteCapacity = EditorGUILayout.IntField(contentExecuteCapacity, console.excuteCapacity);
                console.swipeRatio = EditorGUILayout.Slider(contentSwipeRatio, console.swipeRatio, 0f, 1f);
                console.defaultFontSize = EditorGUILayout.IntSlider(contentDefaultFontSize, console.defaultFontSize, 8, 64);
                console.autoShowWarning = EditorGUILayout.Toggle(contentAutoShowWarning, console.autoShowWarning);
                console.autoShowError = EditorGUILayout.Toggle(contentAutoShowError, console.autoShowError);
                console.autoShowException = EditorGUILayout.Toggle(contentAutoShowException, console.autoShowException);
                console.commandLog = EditorGUILayout.Toggle(contentCommandLog, console.commandLog);
                if (checkScope.changed)
                    EditorUtility.SetDirty(console);
            }
            EditorGUILayout.HelpBox(sb.ToString(), MessageType.Info);
            serializedObject.ApplyModifiedProperties();
        }
    }
}