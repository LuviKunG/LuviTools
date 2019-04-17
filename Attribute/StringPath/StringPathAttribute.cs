﻿using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class StringPathAttribute : PropertyAttribute
{
    public string extension;
    public StringPathAttribute(string extension)
    {
        this.extension = extension;
    }
}