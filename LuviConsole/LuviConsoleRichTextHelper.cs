using System.Text;
using UnityEngine;

namespace LuviKunG
{
    public static class LuviConsoleRichTextHelper
    {
        private static void Color(ref StringBuilder s, string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out Color color))
                Color(ref s, color);
        }

        private static void Color(ref StringBuilder s, Color color)
        {
            s.Insert(0, ">");
            s.Insert(0, ColorUtility.ToHtmlStringRGB(color));
            s.Insert(0, "<color=#");
            s.Append("</color>");
        }

        private static void Bold(ref StringBuilder s)
        {
            s.Insert(0, "<b>");
            s.Append("</b>");
        }

        private static void Italic(ref StringBuilder s)
        {
            s.Insert(0, "<i>");
            s.Append("</i>");
        }
    }
}