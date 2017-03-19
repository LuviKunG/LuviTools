namespace LuviKunG
{
    //This class inspired by Unreal Engine class called "Text".
    //That can hold the string key of localization and translate into the localized string.

    /// <summary>
    /// Class that keep localization key and transfer into localized string.
    /// Using with M2H Localization plugins.
    /// </summary>
    public class Text
    {
        public string key { get; private set; }
        public string group { get; private set; }

        /// <summary>
        /// Keep localization key and transfer into localized string.
        /// </summary>
        /// <param name="key">Localization Key</param>
        public Text(string key)
        {
            this.key = key;
            this.group = null;
        }

        public Text(string key, string group)
        {
            this.key = key;
            this.group = group;
        }

        public string GetString(params string[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
                return GetLocalizedString();
            else
                return string.Format(GetLocalizedString(), parameters);
        }

        private string GetLocalizedString()
        {
            if (key == null)
                return null;
            if (group == null)
                return CleanStringFormat(Language.Get(key));
            else
                return CleanStringFormat(Language.Get(key, group));
        }

        private string CleanStringFormat(string s)
        {
            if (s.Contains("#!#"))
            {
                return "";
            }
            else
            {
                s.Replace("\\n", "\n");
                s.Replace("#value", "");
                s.Replace("&#39;", "'");
                s.Replace("&apos;", "'");
                s.Replace("&quot;", "\"");
                s.Replace("&gt;", ">");
                s.Replace("&lt;", "<");
                s.Replace("&amp;", "&");
                return s;
            }
        }

        #region operator

        public static implicit operator Text(string key)
        {
            return new Text(key);
        }

        public static explicit operator string(Text x)
        {
            return x.GetLocalizedString();
        }
        
        public static bool operator ==(Text c1, Text c2)
        {
            return c1.key == c2.key;
        }

        public static bool operator !=(Text c1, Text c2)
        {
            return c1.key != c2.key;
        }

        #endregion

        #region overide

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (GetType() != obj.GetType())
                return false;
            return key == ((Text)obj).key;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return GetLocalizedString();
        }

        #endregion
    }
}