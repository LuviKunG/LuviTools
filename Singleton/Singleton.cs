using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    /// <summary>
    /// get: returns the instance of this singleton.
    /// set: set new instance if this singleton.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("An instance of " + typeof(T) + " is not to be instance object.");
                return null;
            }
            return _instance;
        }
        set
        {
            if (_instance == null) _instance = value;
        }
    }
}