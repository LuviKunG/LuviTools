using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace LuviKunG
{
    public static class AndroidBuildPipelineMenu
    {
        [MenuItem("Build/Android", false, 0)]
        public static void Build()
        {
            if (BuildPipeline.isBuildingPlayer)
                return;
            string loadedBuildPath = EditorUserBuildSettings.GetBuildLocation(BuildTarget.Android);
            string directoryPath;
            if (string.IsNullOrEmpty(loadedBuildPath))
            {
                directoryPath = EditorUtility.SaveFolderPanel("Choose Location of Build Game", loadedBuildPath, null);
                if (string.IsNullOrEmpty(directoryPath))
                    return;
            }
            else
                directoryPath = loadedBuildPath;
            EditorUserBuildSettings.SetBuildLocation(BuildTarget.Android, directoryPath);
            var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            for (int i = 0; i < scenes.Count; i++)
                if (!scenes[i].enabled)
                    scenes.RemoveAt(i--);
            if (!(scenes.Count > 0))
                return;
            string fileName = new AndroidBuildPipelineSettings().GetFileName();
            string buildPath = Path.Combine(directoryPath, fileName);
            BuildReport report = BuildPipeline.BuildPlayer(scenes.ToArray(), buildPath, BuildTarget.Android, BuildOptions.None);
            BuildSummary summary = report.summary;
            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Build succeeded at '{buildPath}' using {summary.totalTime.TotalSeconds.ToString("N2")} seconds with size of {summary.totalSize} bytes.");
                Application.OpenURL(directoryPath);
            }
            if (summary.result == BuildResult.Failed)
            {
                Debug.LogError($"Build failed...");
            }
        }

        [MenuItem("Build/Settings/Android/Set Build Directory...", false, 1)]
        public static void SetDirectory()
        {
            if (BuildPipeline.isBuildingPlayer)
                return;
            string loadedBuildPath = EditorUserBuildSettings.GetBuildLocation(BuildTarget.Android);
            string newPath = EditorUtility.SaveFolderPanel("Choose Location of Build Game", loadedBuildPath, null);
            if (string.IsNullOrEmpty(newPath))
                return;
            EditorUserBuildSettings.SetBuildLocation(BuildTarget.Android, newPath);
            Debug.Log($"Build directory has been set to: {newPath}");
        }

        [MenuItem("Build/Settings/Android/Open Directory...", true, 2)]
        public static bool OpenDirectoryValidate()
        {
            string loadedBuildPath = EditorUserBuildSettings.GetBuildLocation(BuildTarget.Android);
            return !string.IsNullOrEmpty(loadedBuildPath);
        }

        [MenuItem("Build/Settings/Android/Open Directory...", false, 2)]
        public static void OpenDirectory()
        {
            if (BuildPipeline.isBuildingPlayer)
                return;
            string loadedBuildPath = EditorUserBuildSettings.GetBuildLocation(BuildTarget.Android);
            if (string.IsNullOrEmpty(loadedBuildPath))
                return;
            Application.OpenURL(loadedBuildPath);
        }


        [MenuItem("Build/Settings/Android/Open Name Settings...", false, 3)]
        public static void OpenSetting()
        {
            AndroidBuildPipelineSettingsWindow.OpenWindow();
        }
    }

    public sealed class AndroidBuildPipelineSettings
    {
        private const string EDITOR_PREFS_SETTINGS_NAME_FORMAT = "LuviKunG/BuildPipeline/Android/NameFormat";
        private const string EDITOR_PREFS_SETTINGS_DATE_TIME_FORMAT = "LuviKunG/BuildPipeline/Android/DateTimeFormat";

        public string nameFormat;
        public string dateTimeFormat;

        public AndroidBuildPipelineSettings()
        {
            Load();
        }

        public void Load()
        {
            nameFormat = EditorPrefs.GetString(EDITOR_PREFS_SETTINGS_NAME_FORMAT, "{package}_{date}");
            dateTimeFormat = EditorPrefs.GetString(EDITOR_PREFS_SETTINGS_DATE_TIME_FORMAT, "yyyyMMddHHmmss");
        }

        public void Save()
        {
            EditorPrefs.SetString(EDITOR_PREFS_SETTINGS_NAME_FORMAT, nameFormat);
            EditorPrefs.SetString(EDITOR_PREFS_SETTINGS_DATE_TIME_FORMAT, dateTimeFormat);
        }

        public string GetFileName()
        {
            StringBuilder s = new StringBuilder();
            s.Append(nameFormat);
            s.Replace("{name}", Application.productName);
            s.Replace("{package}", PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android));
            s.Replace("{version}", Application.version);
            s.Replace("{bundle}", PlayerSettings.Android.bundleVersionCode.ToString());
            s.Replace("{date}", DateTime.Now.ToString(dateTimeFormat));
            s.Append(".apk");
            return s.ToString();
        }
    }

    public sealed class AndroidBuildPipelineSettingsWindow : EditorWindow
    {
        private const string NAME_FORMATTING_INFO = @"How to format the file name.

{name} = App Name.
{package} = Android Package Name.
{version} = App Version.
{bundle} = App Bundle.
{date} = Date time. (format)";

        private AndroidBuildPipelineSettings format;

        public static AndroidBuildPipelineSettingsWindow OpenWindow()
        {
            var window = GetWindow<AndroidBuildPipelineSettingsWindow>(true, "Android Build Pipeline Setting", true);
            window.Show();
            return window;
        }

        private void OnEnable()
        {
            format = new AndroidBuildPipelineSettings();
        }

        private void OnGUI()
        {
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                EditorGUILayout.LabelField("File name formatting", EditorStyles.boldLabel);
                format.nameFormat = EditorGUILayout.TextField(format.nameFormat);
                EditorGUILayout.LabelField("Formatted name", format.GetFileName(), EditorStyles.textField);
                EditorGUILayout.HelpBox(NAME_FORMATTING_INFO, MessageType.Info, true);
                format.dateTimeFormat = EditorGUILayout.TextField("Date time format", format.dateTimeFormat);
                if (changeScope.changed)
                    format.Save();
            }
        }
    }
}