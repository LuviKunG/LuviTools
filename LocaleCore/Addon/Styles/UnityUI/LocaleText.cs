using UnityEngine;
using UnityEngine.UI;

namespace LuviKunG.LocaleCore.UI
{
    [RequireComponent(typeof(Text))]
    public class LocaleText : CacheBehaviour, ILocale
    {
        public LocaleTextStyle content;
        public Text text;

        public void OnLocaleUpdate(LocaleCode _code)
        {
            LocaleTextProperty property = content.GetContent(_code);
            if (property != null)
            {
                text.font = property.font;
                text.fontSize = property.size;
                text.lineSpacing = property.lineSpacing;
            }
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (text == null)
                text = GetComponent<Text>();
        }
#endif
    }
}
/*
namespace LuviKunG.LocaleCore.UI
{
    [RequireComponent(typeof(Text))]
    public class LocaleText : CacheBehaviour, ILocale
    {
        public LocaleTextStyle content;
        public Text text;

        private void OnEnable()
        {
            Locale.AddLocaleComponent(this);
            OnLocaleUpdate(Locale.currentLocale);
        }

        private void OnDisable()
        {
            Locale.RemoveLocaleComponent(this);
        }

        public void OnLocaleUpdate(LocaleCode _code)
        {
            LocaleTextProperty property = content.GetContent(_code);
            if (property != null)
            {
                text.font = property.font;
                text.fontSize = property.size;
                text.lineSpacing = property.lineSpacing;
            }
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (text == null)
                text = GetComponent<Text>();
        }
#endif
    }
}
*/