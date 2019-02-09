using UnityEngine;
using UnityEditor;
using LuviKunG.LocaleCore;

namespace LuviKunG.Editor.LocaleCore
{
    [CustomEditor(typeof(LocaleSettings), false, isFallback = false)]
    public class EditorLocaleSettings : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        public void OnEnable()
        {
            if (!PlayerPrefs.HasKey("LocaleLanguage"))
            {
                LocaleSettings localeSettings = (LocaleSettings)target;
                if (localeSettings.defaultLocale != LocaleCode.Null)
                {
                    PlayerPrefs.SetString("LocaleLanguage", localeSettings.defaultLocale.ToString());
                }
            }
        }
    }
}