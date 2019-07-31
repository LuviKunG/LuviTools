using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Text (Thai Glyph)")]
    public class TextThaiGlyph : Text
    {
        public override string text { get => base.text; set => base.text = ThaiFontAdjuster.Adjust(value); }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (Application.isPlaying)
                return;
            base.OnValidate();
            text = ThaiFontAdjuster.Adjust(text);
        }
#endif
    }
}