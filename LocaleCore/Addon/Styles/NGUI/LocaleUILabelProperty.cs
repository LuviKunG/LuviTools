using System;
using UnityEngine;

namespace LuviKunG.LocaleCore.UI
{
    [Serializable]
    public class LocaleUILabelProperty
    {
        public LocaleCode code;
        public Font trueTypeFont;
        public UIFont nguiFont;
        public int size;
        public bool useFloatSpacing;
        public Vector2 floatSpacing;
        public Vector2Int intSpacing;
    }
}