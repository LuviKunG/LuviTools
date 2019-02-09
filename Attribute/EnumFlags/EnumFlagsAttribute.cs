using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class EnumFlagsAttribute : PropertyAttribute
{
    public EnumFlagsAttribute() { }
}