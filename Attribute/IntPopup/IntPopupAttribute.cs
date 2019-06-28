using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class IntPopupAttribute : PropertyAttribute
{
    public string[] name;
    public int[] value;

    public IntPopupAttribute(string[] name, int[] value)
    {
        this.name = name;
        this.value = value;
    }

    public IntPopupAttribute(int[] value)
    {
        name = new string[value.Length];
        for (int i = 0; i < value.Length; i++)
            name[i] = value[i].ToString("N0");
        this.value = value;
    }
}