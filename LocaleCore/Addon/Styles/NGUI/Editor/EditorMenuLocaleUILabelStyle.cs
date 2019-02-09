using UnityEngine;
using UnityEditor;
using LuviKunG.LocaleCore.UI;

namespace LuviKunG.Locale.Editor
{
    public class EditorMenuLocaleUILabelStyle : EditorWindow
    {
        [MenuItem("LuviKunG/LocaleCore/Create Locale UILabel Style")]
        static void CreateLocaleContentText()
        {
            CreateAsset<LocaleUILabelStyle>("", "NewLocaleUILabelStyle");
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