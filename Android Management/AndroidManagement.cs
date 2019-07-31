using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class AndroidManagement : MonoBehaviour
{
    [HideInInspector]
    public List<AndroidSetting> settings = new List<AndroidSetting>();
    [SerializeField]
    private bool destroyWhenExecuted = default;

    public void Awake()
    {
        for (int i = 0; i < settings.Count; i++)
            settings[i]?.Execute();
        if (destroyWhenExecuted)
            Destroy(gameObject);
    }
}