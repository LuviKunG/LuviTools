using System;

namespace LuviKunG.LocaleCore
{
    [Serializable]
    public class LocaleKey
    {
        public string Key;
        public string Value;

        public LocaleKey(string _key, string _value)
        {
            Key = _key;
            Value = _value;
        }
    }
}