using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class StringPopupAttribute : PropertyAttribute
{
    public string[] name;
    public string[] value;

    public StringPopupAttribute(string[] list)
    {
        name = list;
        value = list;
    }

    public StringPopupAttribute(string[] name, string[] value)
    {
        this.name = name;
        this.value = value;
    }
}