using System;
using UnityEngine;

namespace LuviKunG.Tools
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class StringSceneAttribute : PropertyAttribute
    {
        public bool excludeDisableScene;

        public StringSceneAttribute()
        {
            excludeDisableScene = true;
        }

        public StringSceneAttribute(bool excludeDisableScene)
        {
            this.excludeDisableScene = excludeDisableScene;
        }
    }
}