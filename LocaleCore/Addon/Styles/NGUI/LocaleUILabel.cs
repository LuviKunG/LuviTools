using UnityEngine;
using UnityEngine.UI;

namespace LuviKunG.LocaleCore.UI
{
    [RequireComponent(typeof(UILabel))]
    public class LocaleUILabel : CacheBehaviour, ILocale
    {
        public LocaleUILabelStyle content;
        public UILabel text;

        public void OnLocaleUpdate(LocaleCode _code)
        {
            LocaleUILabelProperty property = content.GetContent(_code);
            if (property != null)
            {
                text.trueTypeFont = property.trueTypeFont;
                text.bitmapFont = property.nguiFont;
                text.fontSize = property.size;
                text.useFloatSpacing = property.useFloatSpacing;
                if (text.useFloatSpacing)
                {
                    text.floatSpacingX = property.floatSpacing.x;
                    text.floatSpacingY = property.floatSpacing.y;
                }
                else
                {
                    text.spacingX = 0;
                    text.spacingX = 0;
                }

            }
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (text == null)
                text = GetComponent<UILabel>();
        }
#endif
    }
}