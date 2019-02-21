using UnityEngine;

namespace LuviKunG.LocaleCore
{
    public class LocaleSettings : ScriptableObject
    {
        public LocaleCode[] availableLocale;
        public string[] sheetTitles;
        public LocaleCode defaultLocale;
        public bool isUseSystemLanguage;

        public bool HasLocale(LocaleCode code)
        {
            for (int i = 0; i < availableLocale.Length; i++)
                if (availableLocale[i] == code)
                    return true;
            return false;
        }
    }
}