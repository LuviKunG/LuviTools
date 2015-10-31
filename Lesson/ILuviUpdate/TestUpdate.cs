using UnityEngine;
using System.Collections;

public class TestUpdate : MonoBehaviour, ILuviUpdate
{
    public void Start()
    {
        Singleton<TestMainUpdate>.Instance.AddUpdate(this);
    }

    public void HookUpdate()
    {

    }
}