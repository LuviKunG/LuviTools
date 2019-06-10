using UnityEngine;
using StringBuilder = System.Text.StringBuilder;

namespace LuviKunG.RichText
{
    public static class RichTextExtension
    {
        public static string Color(this string s, string hexString)
        {
            Color color;
            if (ColorUtility.TryParseHtmlString(hexString, out color))
                return Color(s, color);
            else
                return s;
        }
        public static string Color(this string s, Color color)
        {
            StringBuilder str = new StringBuilder();
            str.Append("<color=");
            str.Append(ColorUtility.ToHtmlStringRGB(color));
            str.Append(">");
            str.Append(s);
            str.Append("</color>");
            return str.ToString();
        }
        public static string Bold(this string s)
        {
            StringBuilder str = new StringBuilder();
            str.Append("<b>");
            str.Append(s);
            str.Append("</b>");
            return str.ToString();
        }
        public static string Italic(this string s)
        {
            StringBuilder str = new StringBuilder();
            str.Append("<i>");
            str.Append(s);
            str.Append("</i>");
            return str.ToString();
        }
        public static string Size(this string s, int size)
        {
            StringBuilder str = new StringBuilder();
            str.Append("<size=");
            str.Append(size);
            str.Append(">");
            str.Append(s);
            str.Append("</size>");
            return str.ToString();
        }
    }
}