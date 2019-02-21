using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class AndroidManagement : MonoBehaviour
{
    [HideInInspector]
    public List<AndroidSetting> settings = new List<AndroidSetting>();
    public bool destroyWhenExecuted;

    public void Awake()
    {
        for (int i = 0; i < settings.Count; i++)
        {
            if (settings[i] == null)
                throw new AndroindManagementException("Cannot find settings.");
            settings[i].Execute();
        }
        if (destroyWhenExecuted)
            Destroy(gameObject);
    }
}

public sealed class AndroindManagementException : Exception
{
    public AndroindManagementException() { }
    public AndroindManagementException(string message) : base(message) { }
}