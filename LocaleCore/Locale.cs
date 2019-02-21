using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace LuviKunG.LocaleCore
{
    public static  class Locale
    {
        /// <summary>
        /// This is default path that resources class will direct to find locale core components.
        /// </summary>
        public const string defaultResourcePath = "LocaleCore";
        public const string localeSettingsName = "LocaleSettings";
        public const string prefsLocaleLanguage = "LocaleLanguage";

        public static LocaleCode currentLocale = LocaleCode.Null;
        public static Dictionary<string, LocaleData> preloadLocaleData = new Dictionary<string, LocaleData>();

        private static LocaleSettings _settings;
        public static LocaleSettings settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = Resources.Load<LocaleSettings>(defaultResourcePath + "/" + localeSettingsName);
                    if (_settings == null)
                        throw new MissingLocaleSettingsException("Cannot find \'LocaleSettings\' in resources path.");
                }
                return _settings;
            }
        }

        private static List<ILocale> localeComponents;

        public static void AddLocaleComponent(ILocale localeComponent)
        {
            if (localeComponent != null)
                localeComponents.Add(localeComponent);
        }

        public static void RemoveLocaleComponent(ILocale localeComponent)
        {
            if (localeComponent != null)
                localeComponents.Remove(localeComponent);
        }

        public static void CleanNullReferenceLocaleComponent()
        {
            LuviTools.RemoveNull(ref localeComponents);
            //for (int i = 0; i < localeComponents.Count; i++)
            //{
            //    if (localeComponents[i] == null)
            //        localeComponents.RemoveAt(i--);
            //}
        }

        static Locale()
        {
            localeComponents = new List<ILocale>();
            Initialize();
        }

        public static void Initialize()
        {
            LocaleCode cachedLocaleCode;
            if (PlayerPrefs.HasKey(prefsLocaleLanguage))
            {
                cachedLocaleCode = GetLocaleCodeEnum(PlayerPrefs.GetString(prefsLocaleLanguage));
                if (settings.HasLocale(cachedLocaleCode))
                    LoadLocale(cachedLocaleCode);
                else
                    LoadLocale(settings.defaultLocale);
            }
            else
            {
                if (settings.isUseSystemLanguage)
                {
                    cachedLocaleCode = SystemLanguageToLocaleCode(Application.systemLanguage);
                    if (cachedLocaleCode == LocaleCode.Null)
                    {
                        string langISO = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                        if (langISO != "iv") //IV = InvariantCulture
                            cachedLocaleCode = GetLocaleCodeEnum(langISO);
                        else
                            cachedLocaleCode = settings.defaultLocale;
                    }
                    PlayerPrefs.SetString(prefsLocaleLanguage, cachedLocaleCode.ToString());
                    LoadLocale(cachedLocaleCode);
                }
                else
                {
                    PlayerPrefs.SetString(prefsLocaleLanguage, settings.defaultLocale.ToString());
                    LoadLocale(settings.defaultLocale);
                }
            }
        }

        public static void LoadLocale(LocaleCode code)
        {
            if (currentLocale == code)
                return;
            if (settings.HasLocale(code))
            {
                currentLocale = code;
                PlayerPrefs.SetString(prefsLocaleLanguage, currentLocale.ToString());
                StringBuilder s = new StringBuilder();
                s.Append(defaultResourcePath);
                s.Append('/');
                s.Append(currentLocale.ToString());
                LocaleData[] _list = Resources.LoadAll<LocaleData>(s.ToString());
                foreach (KeyValuePair<string, LocaleData> r in preloadLocaleData)
                    Resources.UnloadAsset(r.Value);
                preloadLocaleData.Clear();
                foreach (LocaleData data in _list)
                {
                    if (preloadLocaleData.ContainsKey(data.name))
                    {
                        preloadLocaleData[data.name] = data;
                        Debug.LogWarning("Found a duplicate locale data \'" + data.name + "\'. Please check from project resources folder and delete an duplicated locale data.");
                    }
                    else
                        preloadLocaleData.Add(data.name, data);
                }
                UpdateLocaleComponent();
            }
            else
            {
                Debug.LogWarning("Locale settings doesn't support for locale \'" + code.ToString() + "\'. Nothing changed.");
            }
        }

        public static void UpdateLocaleComponent()
        {
            //if (onLocaleChange != null)
            //    onLocaleChange();
            for (int i = 0; i < localeComponents.Count; i++)
            {
                if (localeComponents[i] != null)
                    localeComponents[i].OnLocaleUpdate(currentLocale);
                else
                    localeComponents.RemoveAt(i--);
            }
        }

        public static bool HasTable(string table)
        {
            return preloadLocaleData.ContainsKey(table);
        }

        public static string GetString(string key, string table)
        {
            if (string.IsNullOrEmpty(table))
                return "##NULLTABLE##";
            if (HasTable(table))
                return preloadLocaleData[table].Get(key);
            else
                return "##MISSINGTABLE##";
        }

        public static string GetString(string key, string table, params object[] args)
        {
            return string.Format(GetString(key, table), args);
        }

        public static LocaleData LoadTable(string table)
        {
            StringBuilder s = new StringBuilder();
            s.Append(defaultResourcePath);
            s.Append('/');
            s.Append(currentLocale.ToString());
            s.Append('/');
            s.Append(table);
            LocaleData data = Resources.Load<LocaleData>(s.ToString());
            if (data != null)
                return data;
            else
                throw new MissingLocaleDataException("Cannot find a table \'" + table + "\' from path: " + s.ToString());
        }

        public static T GetAsset<T>(string key) where T : UnityEngine.Object
        {
            return Resources.Load<T>(defaultResourcePath + "/" + currentLocale.ToString() + "/" + key);
        }

        public static LocaleCode GetLocaleCodeEnum(string str)
        {
            LocaleCode cached;
            try { cached = (LocaleCode)Enum.Parse(typeof(LocaleCode), str); }
            catch { cached = LocaleCode.Null; }
            return cached;
        }

        public static LocaleCode SystemLanguageToLocaleCode(SystemLanguage name)
        {
            if (name == SystemLanguage.Afrikaans) return LocaleCode.AF;
            else if (name == SystemLanguage.Arabic) return LocaleCode.AR;
            else if (name == SystemLanguage.Basque) return LocaleCode.BA;
            else if (name == SystemLanguage.Belarusian) return LocaleCode.BE;
            else if (name == SystemLanguage.Bulgarian) return LocaleCode.BG;
            else if (name == SystemLanguage.Catalan) return LocaleCode.CA;
            else if (name == SystemLanguage.Chinese) return LocaleCode.ZH;
            else if (name == SystemLanguage.Czech) return LocaleCode.CS;
            else if (name == SystemLanguage.Danish) return LocaleCode.DA;
            else if (name == SystemLanguage.Dutch) return LocaleCode.NL;
            else if (name == SystemLanguage.English) return LocaleCode.EN;
            else if (name == SystemLanguage.Estonian) return LocaleCode.ET;
            else if (name == SystemLanguage.Faroese) return LocaleCode.FA;
            else if (name == SystemLanguage.Finnish) return LocaleCode.FI;
            else if (name == SystemLanguage.French) return LocaleCode.FR;
            else if (name == SystemLanguage.German) return LocaleCode.DE;
            else if (name == SystemLanguage.Greek) return LocaleCode.EL;
            else if (name == SystemLanguage.Hebrew) return LocaleCode.HE;
            else if (name == SystemLanguage.Hungarian) return LocaleCode.HU;
            else if (name == SystemLanguage.Icelandic) return LocaleCode.IS;
            else if (name == SystemLanguage.Indonesian) return LocaleCode.ID;
            else if (name == SystemLanguage.Italian) return LocaleCode.IT;
            else if (name == SystemLanguage.Japanese) return LocaleCode.JA;
            else if (name == SystemLanguage.Korean) return LocaleCode.KO;
            else if (name == SystemLanguage.Latvian) return LocaleCode.LA;
            else if (name == SystemLanguage.Lithuanian) return LocaleCode.LT;
            else if (name == SystemLanguage.Norwegian) return LocaleCode.NO;
            else if (name == SystemLanguage.Polish) return LocaleCode.PL;
            else if (name == SystemLanguage.Portuguese) return LocaleCode.PT;
            else if (name == SystemLanguage.Romanian) return LocaleCode.RO;
            else if (name == SystemLanguage.Russian) return LocaleCode.RU;
            else if (name == SystemLanguage.SerboCroatian) return LocaleCode.SH;
            else if (name == SystemLanguage.Slovak) return LocaleCode.SK;
            else if (name == SystemLanguage.Slovenian) return LocaleCode.SL;
            else if (name == SystemLanguage.Spanish) return LocaleCode.ES;
            else if (name == SystemLanguage.Swedish) return LocaleCode.SW;
            else if (name == SystemLanguage.Thai) return LocaleCode.TH;
            else if (name == SystemLanguage.Turkish) return LocaleCode.TR;
            else if (name == SystemLanguage.Ukrainian) return LocaleCode.UK;
            else if (name == SystemLanguage.Vietnamese) return LocaleCode.VI;
            else if (name == SystemLanguage.Hungarian) return LocaleCode.HU;
            else if (name == SystemLanguage.Unknown) return LocaleCode.Null;
            return LocaleCode.Null;
        }
    }

    public class MissingLocaleSettingsException : Exception
    {
        public MissingLocaleSettingsException() { }
        public MissingLocaleSettingsException(string message) : base(message) { }
    }

    public class MissingLocaleDataException : Exception
    {
        public MissingLocaleDataException() { }
        public MissingLocaleDataException(string message) : base(message) { }
    }
}