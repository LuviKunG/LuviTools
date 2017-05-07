using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace LuviKunG
{
    public class LocalizationSheetData
    {
        public Dictionary<string, Dictionary<string, string>> localizationKey;

        public LocalizationSheetData()
        {
            localizationKey = new Dictionary<string, Dictionary<string, string>>();
        }
    }

    public class EditorImportLocalizationCSV : EditorWindow
    {
        static Object sourceCSV;
        static string exportPath = "";

        [MenuItem("Window/LuviKunG/Export CSV Localization")]
        public static void WindowOpen()
        {
            //EditorWindow window = GetWindowWithRect<EditorImportLocalizationCSV>(new Rect(0, 0, 200, 100), false, "Import Localization CSV", true);
            //window.Show();
            GetWindow<EditorImportLocalizationCSV>(false, "Import Localization CSV", true);
            exportPath = Application.dataPath + "/Localization/Resources/Languages";
        }

        void OnGUI()
        {
            exportPath = EditorGUILayout.TextField("Export path", exportPath);
            sourceCSV = EditorGUILayout.ObjectField("CSV", sourceCSV, typeof(TextAsset), false);
            if (GUILayout.Button("Select Export Location"))
            {
                exportPath = EditorUtility.OpenFolderPanel("Select localization for export XML file.", Application.dataPath, "");
                if (string.IsNullOrEmpty(exportPath))
                {
                    return;
                }
            }
            if (GUILayout.Button("Export CSV Localization"))
            {
                _settings = settings;
                if (_settings == null)
                {
                    Debug.LogError("Cannot find LocalizationSettings.asset");
                    return;
                }

                if (string.IsNullOrEmpty(exportPath))
                {
                    Debug.LogError("Invalid export path.");
                    return;
                }
                if (sourceCSV == null)
                {
                    Debug.LogError("Source are null.");
                    return;
                }
                if (exportPath == null)
                {
                    string exportPath = EditorUtility.OpenFolderPanel("Select localization for export XML file.", Application.dataPath, "");
                    if (string.IsNullOrEmpty(exportPath))
                    {
                        exportPath = "";
                        return;
                    }
                }
                TextAsset textAssetCSV = sourceCSV as TextAsset;
                //string importFilePath = EditorUtility.OpenFilePanel("Select localization CSV file.", Application.dataPath, "csv");
                //if (string.IsNullOrEmpty(importFilePath))
                //{
                //    return;
                //}

                string exportDirectory = exportPath;
                //string exportDirectory = EditorUtility.OpenFolderPanel("Select localization for export XML file.", Application.dataPath, "");
                //if (string.IsNullOrEmpty(exportDirectory))
                //{
                //    return;
                //}

                string fileName = textAssetCSV.name;
                //string fileName = Path.GetFileNameWithoutExtension(importFilePath);
                List<List<string>> lists = ReadCSVTextAsset(textAssetCSV);
                LocalizationSheetData sheetData = new LocalizationSheetData();
                for (int i = 0; i < lists.Count; i++)
                {
                    if (i == 0) //first column
                    {
                        for (int j = 0; j < lists[i].Count; j++)
                        {
                            if (j == 0) //first row
                            {
                                //nothing to do.
                                continue;
                            }
                            else //other row
                            {
                                sheetData.localizationKey.Add(lists[i][j], new Dictionary<string, string>());
                            }
                        }
                    }
                    else //other column
                    {
                        string key = "";
                        for (int j = 0; j < lists[i].Count; j++)
                        {
                            if (j == 0) //first row
                            {
                                key = lists[i][j];
                            }
                            else //other row
                            {
                                sheetData.localizationKey[lists[0][j]].Add(key, lists[i][j]);
                            }
                        }
                    }
                }

                foreach (KeyValuePair<string, Dictionary<string, string>> lang in sheetData.localizationKey)
                {
                    string exportFilePath = exportDirectory + "/" + lang.Key + "_" + fileName + ".xml";
                    XmlWriterSettings settings = new XmlWriterSettings
                    {
                        Indent = true,
                        IndentChars = "  ",
                        NewLineChars = "\r\n",
                        NewLineHandling = NewLineHandling.Replace
                    };
                    using (XmlWriter xmlWriter = XmlWriter.Create(exportFilePath, settings))
                    {
                        xmlWriter.WriteStartDocument();
                        xmlWriter.WriteStartElement("entries");
                        foreach (KeyValuePair<string, string> pair in lang.Value)
                        {
                            xmlWriter.WriteStartElement("entry");
                            xmlWriter.WriteAttributeString("name", pair.Key);
                            xmlWriter.WriteString(pair.Value);
                            xmlWriter.WriteEndElement();
                        }
                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndDocument();
                        xmlWriter.Close();
                    }
                }
                List<string> sheetTitle = new List<string>(_settings.sheetTitles);
                if (!sheetTitle.Contains(fileName))
                {
                    sheetTitle.Add(fileName);
                }
                _settings.sheetTitles = sheetTitle.ToArray();
                AssetDatabase.Refresh();
            }
        }

        private static LocalizationSettings _settings = null;
        public static LocalizationSettings settings
        {
            get
            {
                if (_settings == null)
                {
                    string settingsFile = "Languages/" + System.IO.Path.GetFileNameWithoutExtension(Language.settingsAssetPath);
                    _settings = (LocalizationSettings)Resources.Load(settingsFile, typeof(LocalizationSettings));
                }
                return _settings;
            }
        }

        public static List<List<string>> ReadCSVTextAsset(TextAsset text, char sepChar = ',', char quoteChar = '"')
        {
            char[] archDelim = new char[] { '\r', '\n' };
            List<List<string>> ret = new List<List<string>>();
            string[] csvRows = text.text.Split(archDelim, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string csvRow in csvRows)
            {
                bool inQuotes = false;
                List<string> fields = new List<string>();
                string field = "";
                for (int i = 0; i < csvRow.Length; i++)
                {
                    if (inQuotes)
                    {
                        if (i < csvRow.Length - 1 && csvRow[i] == quoteChar && csvRow[i + 1] == quoteChar)
                        {
                            i++;
                            field += quoteChar;
                        }
                        else if (csvRow[i] == quoteChar)
                        {
                            inQuotes = false;
                        }
                        else
                        {
                            field += csvRow[i];
                        }
                    }
                    else
                    {
                        if (csvRow[i] == quoteChar)
                        {
                            inQuotes = true;
                            continue;
                        }
                        if (csvRow[i] == sepChar)
                        {
                            fields.Add(field);
                            field = "";
                        }
                        else
                        {
                            field += csvRow[i];
                        }
                    }
                }
                if (!string.IsNullOrEmpty(field))
                {
                    fields.Add(field);
                    field = "";
                }
                ret.Add(fields);
            }
            return ret;
        }

        public static List<List<string>> ReadCSVFileMSStyle(string filePath, char sepChar = ',', char quoteChar = '"')
        {
            List<List<string>> ret = new List<List<string>>();
            string[] csvRows = System.IO.File.ReadAllLines(filePath);
            foreach (string csvRow in csvRows)
            {
                bool inQuotes = false;
                List<string> fields = new List<string>();
                string field = "";
                for (int i = 0; i < csvRow.Length; i++)
                {
                    if (inQuotes)
                    {
                        if (i < csvRow.Length - 1 && csvRow[i] == quoteChar && csvRow[i + 1] == quoteChar)
                        {
                            i++;
                            field += quoteChar;
                        }
                        else if (csvRow[i] == quoteChar)
                        {
                            inQuotes = false;
                        }
                        else
                        {
                            field += csvRow[i];
                        }
                    }
                    else
                    {
                        if (csvRow[i] == quoteChar)
                        {
                            inQuotes = true;
                            continue;
                        }
                        if (csvRow[i] == sepChar)
                        {
                            fields.Add(field);
                            field = "";
                        }
                        else
                        {
                            field += csvRow[i];
                        }
                    }
                }
                if (!string.IsNullOrEmpty(field))
                {
                    fields.Add(field);
                    field = "";
                }
                ret.Add(fields);
            }
            return ret;
        }
    }
}