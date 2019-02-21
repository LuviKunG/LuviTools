using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AndroidManagement))]
public class AndroidManagementEditor : Editor
{
    private AndroidManagement androidManagement;

    private void OnEnable()
    {
        androidManagement = target as AndroidManagement;
        androidManagement.settings = new List<AndroidSetting>(androidManagement.gameObject.GetComponents<AndroidSetting>());
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();
        if (IsContainSettings(typeof(AndroidMultiTouch)))
        {
            if (GUILayout.Button("Remove Multitouch Setting"))
            {
                AndroidMultiTouch setting = androidManagement.gameObject.GetComponent<AndroidMultiTouch>();
                androidManagement.settings.Remove(setting);
                DestroyImmediate(setting);
            }
        }
        else
        {
            if (GUILayout.Button("Add Multitouch Setting"))
            {
                AndroidMultiTouch setting = androidManagement.gameObject.AddComponent<AndroidMultiTouch>();
                androidManagement.settings.Add(setting);
            }
        }

        if (IsContainSettings(typeof(AndroidTargetFramerate)))
        {
            if (GUILayout.Button("Remove Target Framerate Setting"))
            {
                AndroidTargetFramerate setting = androidManagement.gameObject.GetComponent<AndroidTargetFramerate>();
                androidManagement.settings.Remove(setting);
                DestroyImmediate(setting);
            }
        }
        else
        {
            if (GUILayout.Button("Add Target Framerate Setting"))
            {
                AndroidTargetFramerate setting = androidManagement.gameObject.AddComponent<AndroidTargetFramerate>();
                androidManagement.settings.Add(setting);
            }
        }

        if (IsContainSettings(typeof(AndroidScreenSleepTimeout)))
        {
            if (GUILayout.Button("Remove Sleep Timeout Setting"))
            {
                AndroidScreenSleepTimeout setting = androidManagement.gameObject.GetComponent<AndroidScreenSleepTimeout>();
                androidManagement.settings.Remove(setting);
                DestroyImmediate(setting);
            }
        }
        else
        {
            if (GUILayout.Button("Add Sleep Timeout Setting"))
            {
                AndroidScreenSleepTimeout setting = androidManagement.gameObject.AddComponent<AndroidScreenSleepTimeout>();
                androidManagement.settings.Add(setting);
            }
        }
        EditorGUI.EndChangeCheck();
    }

    private bool IsContainSettings(Type type)
    {
        for (int i = 0; i < androidManagement.settings.Count; i++)
            if (androidManagement.settings[i].GetType() == type) return true;
        return false;
    }
}
