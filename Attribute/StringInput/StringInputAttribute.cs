using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LuviKunG
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class StringInputAttribute : PropertyAttribute
    {
        public StringInputAttribute() { }
    }
}