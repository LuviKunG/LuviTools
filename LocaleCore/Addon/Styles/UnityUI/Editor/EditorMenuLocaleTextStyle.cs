using UnityEngine;
using UnityEditor;
using LuviKunG.LocaleCore.UI;

namespace LuviKunG.Locale.Editor
{
    public class EditorMenuLocaleTextStyle : EditorWindow
    {
        [MenuItem("LuviKunG/LocaleCore/Create Locale Text Style")]
        static void CreateLocaleContentText()
        {
            CreateAsset<LocaleTextStyle>("", "NewLocaleTextStyle");
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