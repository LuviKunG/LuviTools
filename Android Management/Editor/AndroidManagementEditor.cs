using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
        Color cacheColor = GUI.color;
        using (var scopeChange = new EditorGUI.ChangeCheckScope())
        {
            DrawButton<AndroidMultiTouch>();
            DrawButton<AndroidTargetFramerate>();
            DrawButton<AndroidScreenSleepTimeout>();
            DrawButton<AndroidKeyboardInput>();
        }
        GUI.color = cacheColor;
    }

    private void DrawButton<T>() where T : AndroidSetting
    {
        string name = typeof(T).Name;
        if (IsContainSettings(typeof(T)))
        {
            GUI.color = Color.red;
            if (GUILayout.Button($"Remove {name} Setting"))
            {
                T setting = androidManagement.gameObject.GetComponent<T>();
                androidManagement.settings.Remove(setting);
                EditorApplication.delayCall += () => DestroyImmediate(setting);
            }
        }
        else
        {
            GUI.color = Color.green;
            if (GUILayout.Button($"Add {name} Setting"))
            {
                T setting = androidManagement.gameObject.AddComponent<T>();
                androidManagement.settings.Add(setting);
            }
        }
    }

    private bool IsContainSettings(Type type)
    {
        for (int i = 0; i < androidManagement.settings.Count; i++)
            if (androidManagement.settings[i].GetType() == type) return true;
        return false;
    }
}

public class AndroidManagementMenu
{
    [MenuItem("GameObject/LuviKunG/Android Management")]
    public static void Create()
    {
        GameObject obj = new GameObject("Android Management");
        obj.AddComponent(typeof(AndroidManagement));
    }
}