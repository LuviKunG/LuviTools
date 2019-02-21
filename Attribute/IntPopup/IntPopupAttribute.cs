using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class IntPopupAttribute : PropertyAttribute
{
    public string[] name;
    public int[] value;

    public IntPopupAttribute(string[] name, int[] value)
    {
        this.name = name;
        this.value = value;
    }
}