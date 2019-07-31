using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class EnumFlagsAttribute : PropertyAttribute
{
    public Type enumFlagType;
    public EnumFlagsAttribute(Type enumFlagType)
    {
        this.enumFlagType = enumFlagType;
    }
}