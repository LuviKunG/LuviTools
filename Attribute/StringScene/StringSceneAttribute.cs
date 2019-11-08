using System;
using UnityEngine;

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