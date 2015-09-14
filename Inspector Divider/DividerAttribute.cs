using UnityEngine;
using System;

namespace LuviKunG
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public class DividerAttribute : PropertyAttribute
    {
        public readonly float space = 8;
        public readonly string color = "black";
        public readonly float thickness = 1f;
        public readonly float width = 1f;

        public DividerAttribute(string color, float thickness, float widthPct, float space)
        {
            this.color = color;
            this.thickness = thickness;
            this.width = widthPct;
            this.space = space;
        }

        public DividerAttribute(string color, float thickness, float widthPct)
        {
            this.color = color;
            this.thickness = thickness;
            this.width = widthPct;
        }

        public DividerAttribute(string color, float thickness)
        {
            this.color = color;
            this.thickness = thickness;
        }

        public DividerAttribute(string color)
        {
            this.color = color;
        }

        public DividerAttribute(float widthPct, float thickness, float space)
        {
            this.width = widthPct;
            this.thickness = thickness;
            this.space = space;
        }

        public DividerAttribute(float widthPct, float thickness)
        {
            this.width = widthPct;
            this.thickness = thickness;
        }

        public DividerAttribute(float widthPct)
        {
            this.width = widthPct;
        }

        public DividerAttribute()
        {

        }
    }
}