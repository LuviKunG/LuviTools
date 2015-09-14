using UnityEngine;
using System;

namespace LuviKunG
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public class LabelAttribute : PropertyAttribute
    {
        public readonly string label = "";
        public readonly int size = 12;
        public readonly FontStyle style = FontStyle.Normal;

        public LabelAttribute(string text, int size, bool bold, bool italic)
        {
            this.label = text;
            this.size = size;
            if (bold && italic) style = FontStyle.BoldAndItalic;
            else if (bold) style = FontStyle.Bold;
            else if (italic) style = FontStyle.Italic;
            else style = FontStyle.Normal;
        }

        public LabelAttribute(string text, int size)
        {
            this.label = text;
            this.size = size;
        }

        public LabelAttribute(string text)
        {
            this.label = text;
        }
    }
}