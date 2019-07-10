using UnityEngine;
using StringBuilder = System.Text.StringBuilder;

namespace LuviKunG.RichText
{
    public static class StringBuilderRichTextExtension
    {
        public static StringBuilder Color(this StringBuilder s, string hexString)
        {
            Color color;
            if (ColorUtility.TryParseHtmlString(hexString, out color))
                return Color(s, color);
            else
                return s;
        }

        public static StringBuilder Color(this StringBuilder s, Color color)
        {
            s.Insert(0, ">");
            s.Insert(0, ColorUtility.ToHtmlStringRGB(color));
            s.Insert(0, "<color=#");
            s.Append("</color>");
            return s;
        }

        public static StringBuilder Bold(this StringBuilder s)
        {
            s.Insert(0, "<b>");
            s.Append("</b>");
            return s;
        }

        public static StringBuilder Italic(this StringBuilder s)
        {
            s.Insert(0, "<i>");
            s.Append("</i>");
            return s;
        }

        public static StringBuilder Size(this StringBuilder s, int size)
        {
            s.Insert(0, ">");
            s.Insert(0, size);
            s.Insert(0, "<size=");
            s.Append("</size>");
            return s;
        }
    }
}