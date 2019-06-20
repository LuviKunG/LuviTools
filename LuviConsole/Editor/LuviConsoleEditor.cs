using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LuviConsole))]
[CanEditMultipleObjects]
public class LuviConsoleEditor : Editor
{
    private const string VERSION_LABEL = "Luvi Console Version 2.3.7";
    private StringBuilder sb;

    public void OnEnable()
    {
        sb = sb ?? new StringBuilder();
        sb.Clear();
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
        LuviConsole console = (LuviConsole)target;
        EditorGUILayout.HelpBox(VERSION_LABEL, MessageType.None);
        console.logCapacity = EditorGUILayout.IntField(new GUIContent("Log Capacity", "The capacity of list that will show debug log on console window."), console.logCapacity);
        console.excuteCapacity = EditorGUILayout.IntField(new GUIContent("Log Excuted Command Capacity", "The capacity of excute command that have been excuted."), console.excuteCapacity);
        console.swipeRatio = EditorGUILayout.Slider(new GUIContent("Swipe Ratio", "The ratio when you swipe up/down on your screen to toggle LuviDebug. 0 when touched, 1 when swipe entire of"), console.swipeRatio, 0f, 1f);
        console.defaultFontSize = EditorGUILayout.IntSlider(new GUIContent("Default Font Size", "Default font size of console when it getting start."), console.defaultFontSize, 8, 64);
        console.autoShowWarning = EditorGUILayout.Toggle(new GUIContent("Show Log When Warning", "Automatically show logs when player is get warning log."), console.autoShowWarning);
        console.autoShowError = EditorGUILayout.Toggle(new GUIContent("Show Log When Error", "Automatically show logs when player is get error log."), console.autoShowError);
        console.autoShowException = EditorGUILayout.Toggle(new GUIContent("Show Log When Exception", "Automatically show logs when player is get exception log."), console.autoShowException);
        EditorGUILayout.HelpBox(sb.ToString(), MessageType.Info);
        serializedObject.ApplyModifiedProperties();
    }
}

public class LuviConsoleMenu
{
    [MenuItem("GameObject/LuviKunG/LuviConsole", false, 10)]
    public static void Create()
    {
        GameObject obj = new GameObject("LuviConsole");
        obj.AddComponent(typeof(LuviConsole));
    }
}