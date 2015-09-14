using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PrefabSceneList : ScriptableObject
{
    public List<PrefabSceneProperty> list;
}

[Serializable]
public class PrefabSceneProperty
{
    public string name;
    public GameObject scene;
}
