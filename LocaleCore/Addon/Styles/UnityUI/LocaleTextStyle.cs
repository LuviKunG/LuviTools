using UnityEngine;

namespace LuviKunG.LocaleCore.UI
{
    public class LocaleTextStyle : ScriptableObject
    {
        public LocaleTextProperty[] properties;

        public LocaleTextProperty GetContent(LocaleCode _code)
        {
            if (properties == null || properties.Length == 0)
                return null;
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].code == _code)
                    return properties[i];
            }
            return null;
        }
    }
}