using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(LuviConsole))]
[CanEditMultipleObjects]
public class LuviConsoleEditor : Editor
{
    string _messageWin = "In editor, Press F1 for toggle LuviDebug.";
    string _messageOSX = "In editor, Press Tap for toggle LuviDebug.";
    string _deviceAndroid = "In your Android device, Swipe up direction in your screen for open LuviDebug. And swipe down for close LuviDebug.";
    string _deviceiOS = "In your iPhone/iPad/iPod device, Swipe up direction in your screen for open LuviDebug. And swipe down for close LuviDebug.";
    string _helpResult = "";
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        LuviConsole console = (LuviConsole)target;
        console.logCapacity = EditorGUILayout.IntField(new GUIContent("Log Capacity", "The capacity of list that will show debug log on console window."), console.logCapacity);
        console.excuteCapacity = EditorGUILayout.IntField(new GUIContent("Log Excuted Command Capacity", "The capacity of excute command that have been excuted."), console.excuteCapacity);
        console.swipeRatio = EditorGUILayout.Slider(new GUIContent("Swipe Ratio", "The ratio when you swipe up/down on your screen to toggle LuviDebug. 0 when touched, 1 when swipe entire of"), console.swipeRatio, 0f, 1f);
        console.defaultFontSize = EditorGUILayout.IntSlider(new GUIContent("Default Font Size", "Default font size of console when it getting start."), console.defaultFontSize, 8, 64);
        console.autoShowWarning = EditorGUILayout.Toggle(new GUIContent("Show Log When Warning", "Automatically show logs when player is get warning log."), console.autoShowWarning);
        console.autoShowError = EditorGUILayout.Toggle(new GUIContent("Show Log When Error", "Automatically show logs when player is get error log."), console.autoShowError);
        console.autoShowException = EditorGUILayout.Toggle(new GUIContent("Show Log When Exception", "Automatically show logs when player is get exception log."), console.autoShowException);
        _helpResult = "";
#if UNITY_EDITOR_OSX
        _helpResult += _messageOSX;
#elif UNITY_EDITOR
        _helpResult += _messageWin;
#endif
#if UNITY_ANDROID
        _helpResult += "\n" + _deviceAndroid;
#elif UNITY_IOS
        _helpResult += "\n" + _deviceiOS;
#endif
        EditorGUILayout.HelpBox(_helpResult, MessageType.Info);
        serializedObject.ApplyModifiedProperties();
    }
}