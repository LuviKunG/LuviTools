using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace LuviKunG.LocaleCore.Editor
{
    public class LocaleSheetData
    {
        public string LocaleName;
        public LocaleKey[] List;
    }

    public class EditorWindowLocaleCore : EditorWindow
    {
        private static string exportPath = "";
        private static Object sourceCSV;
        private static LocaleSettings cachedLocaleSettings = null;
        private static bool isPreload;

        public static LocaleSettings GetLocaleSettings()
        {
            if (cachedLocaleSettings == null)
            {
                cachedLocaleSettings = Resources.Load<LocaleSettings>(Locale.defaultResourcePath + "/" + Locale.localeSettingsName);
                if (cachedLocaleSettings == null)
                {
                    LocaleSettings _settings = CreateAsset<LocaleSettings>("Resources/" + Locale.defaultResourcePath, Locale.localeSettingsName);
                    _settings.availableLocale = new LocaleCode[0];
                    _settings.sheetTitles = new string[0];
                    _settings.defaultLocale = LocaleCode.EN;
                    _settings.isUseSystemLanguage = true;
                    EditorUtility.SetDirty(_settings);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
            return cachedLocaleSettings;
        }

        [MenuItem("LuviKunG/LocaleCore/Import CSV Wizard")]
        public static void WindowOpen()
        {
            GetWindow<EditorWindowLocaleCore>(false, "Locale Core", true);
            string editorCachePath = EditorPrefs.GetString("LocaleExportPath");
            if (string.IsNullOrEmpty(editorCachePath))
            {
                EditorPrefs.SetString("Locale Export Path", "Resources/" + Locale.defaultResourcePath);
            }
            else
            {
                exportPath = editorCachePath;
            }
            cachedLocaleSettings = GetLocaleSettings();
            isPreload = true;
        }

        void OnGUI()
        {
            exportPath = EditorGUILayout.TextField("Export path", exportPath);
            if (GUILayout.Button("Select Export Location"))
                BatchSelectLocation();
            sourceCSV = EditorGUILayout.ObjectField("CSV", sourceCSV, typeof(TextAsset), false);
            GUI.enabled = sourceCSV != null;
            if (GUILayout.Button("Export CSV Localization"))
                BatchExport();
            GUI.enabled = true;
            isPreload = GUILayout.Toggle(isPreload, "Make this import as pre-load");
        }

        public static void BatchSelectLocation()
        {
            exportPath = EditorUtility.OpenFolderPanel("Select localization for export XML file.", Application.dataPath, "");
            exportPath = exportPath.Replace(Application.dataPath + "/", "");
            if (string.IsNullOrEmpty(exportPath))
                return;
            EditorPrefs.SetString("LocaleExportPath", exportPath);
        }

        public static void BatchExport()
        {
            LocaleSettings _settings = GetLocaleSettings();
            if (_settings == null)
            {
                EditorUtility.DisplayDialog("Error", "Cannot perform this action because the editor cannot find any \'LocalizationSettings\' in resource path.", "Okay");
                Debug.LogError("Cannot find LocalizationSettings.asset");
                return;
            }
            if (sourceCSV == null)
            {
                EditorUtility.DisplayDialog("Error", "Cannot perform this action because CSV source are null.", "Okay");
                Debug.LogError("Source are null.");
                return;
            }
            if (string.IsNullOrEmpty(exportPath))
            {
                string exportPath = EditorUtility.OpenFolderPanel("Select localization for export XML file.", Application.dataPath, "");
                if (string.IsNullOrEmpty(exportPath))
                {
                    exportPath = "";
                    return;
                }
                else
                {
                    EditorPrefs.SetString("LocaleExportPath", exportPath);
                }
            }
            TextAsset textAssetCSV = sourceCSV as TextAsset;
            string fileName = textAssetCSV.name;
            string[][] table = CSVTextAsset(textAssetCSV);
            if (table == null)
                return;
            List<LocaleSheetData> sheets = new List<LocaleSheetData>();
            for (int c = 1; c < table[0].Length; c++)
            {
                LocaleSheetData sheet = new LocaleSheetData();
                sheet.LocaleName = table[0][c];
                sheet.List = new LocaleKey[table.Length - 1];
                for (int r = 1; r < table.GetLength(0); r++)
                    sheet.List[r - 1] = new LocaleKey(table[r][0], table[r][c]);
                sheets.Add(sheet);
            }

            List<LocaleCode> availableLocale;
            if (_settings.availableLocale != null)
                availableLocale = new List<LocaleCode>(_settings.availableLocale);
            else
                availableLocale = new List<LocaleCode>();
            foreach (LocaleSheetData sheet in sheets)
            {
                try
                {
                    LocaleCode code = LocaleCode.Null;
                    LocaleData localeData = null;
                    code = (LocaleCode)System.Enum.Parse(typeof(LocaleCode), sheet.LocaleName);
#if LOCALE_DATATH
                    if (code == LocaleCode.TH)
                        localeData = CreateAsset<LocaleDataTH>(exportPath + "/" + sheet.LocaleName, fileName);
                    else
                        localeData = CreateAsset<LocaleData>(exportPath + "/" + sheet.LocaleName, fileName);
#else
                localeData = CreateAsset<LocaleData>(exportPath + "/" + sheet.LocaleName, fileName);
#endif
                    localeData.list = sheet.List;
                    if (code != LocaleCode.Null && !availableLocale.Contains(code))
                    {
                        availableLocale.Add(code);
                    }
                    EditorUtility.SetDirty(localeData);
                }
                catch
                {
                    EditorUtility.DisplayDialog("Error", "Cannot parse \'" + sheet.LocaleName + "\' into \'LocaleCode\' enum. Please check your LocaleCode in CSV.", "Okay");
                }
            }
            _settings.availableLocale = availableLocale.ToArray();

            if (isPreload)
            {
                List<string> sheetTitle;
                if (_settings.availableLocale != null)
                    sheetTitle = new List<string>(_settings.sheetTitles);
                else
                    sheetTitle = new List<string>();
                if (!sheetTitle.Contains(fileName))
                {
                    sheetTitle.Add(fileName);
                }
                _settings.sheetTitles = sheetTitle.ToArray();
                EditorUtility.SetDirty(_settings);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static string[][] CSVTextAsset(TextAsset text, char seperate = ',', char quote = '"')
        {
            StringBuilder reader = new StringBuilder(text.text);
            StringBuilder line = new StringBuilder();
            List<string> row = new List<string>();
            List<List<string>> lines = new List<List<string>>();
            bool inQuotes = false;
            for (int i = 0; i < reader.Length; i++)
            {
                if (inQuotes)
                {
                    // csv will create double quote in quote for telling character are single quote
                    if (i < reader.Length - 1 && reader[i] == quote && reader[i + 1] == quote)
                    {
                        line.Append(quote);
                        i++;
                        continue;
                    }
                    else if (reader[i] == quote)
                    {
                        inQuotes = false;
                        continue;
                    }
                    else
                    {
                        line.Append(reader[i]);
                        continue;
                    }
                }
                else
                {
                    if (reader[i] == quote)
                    {
                        inQuotes = true;
                        continue;
                    }
                    else if (reader[i] == '\r' || reader[i] == '\n')
                    {
                        if (line.Length > 0)
                        {
                            row.Add(line.ToString());
                            line.Length = 0;
                            lines.Add(new List<string>(row.ToArray()));
                            row.Clear();
                        }
                        continue;
                    }
                    else if (reader[i] == seperate)
                    {
                        row.Add(line.ToString());
                        line.Length = 0;
                        continue;
                    }
                    else if (i < reader.Length - 1 && reader[i] == '\\')
                    {
                        if (reader[i + 1] == 'n')
                        {
                            line.Append("\n");
                            i++;
                            continue;
                        }
                        else if (reader[i + 1] == 'r')
                        {
                            line.Append("\r");
                            i++;
                            continue;
                        }
                    }
                    else
                    {
                        line.Append(reader[i]);
                        continue;
                    }
                }
            }
            // add final line
            row.Add(line.ToString());
            line.Length = 0;
            lines.Add(new List<string>(row.ToArray()));
            row.Clear();

            string[][] table = new string[lines.Count][];
            for(int i = 0; i < lines.Count; i++)
            {
                table[i] = new string[lines[i].Count];
                for (int j = 0; j < lines[i].Count; j++)
                {
                    table[i][j] = lines[i][j];
                }
            }
            return table;
        }

        public static T CreateAsset<T>(string _assetPath, string _name) where T : ScriptableObject
        {
            T asset = CreateInstance<T>();
            string path = "Assets/" + _assetPath + "/" + _name + ".asset";
            System.IO.Directory.CreateDirectory("Assets/" + _assetPath);
            AssetDatabase.CreateAsset(asset, path);
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return asset;
        }
    }
}